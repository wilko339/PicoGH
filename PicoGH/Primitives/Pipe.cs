using System;
using System.Numerics;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using Rhino.Geometry;

namespace PicoGH.PicoGH.Primitives
{
    public class Pipe : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Pipe class.
        /// </summary>
        public Pipe()
          : base("PicoPipe", "Pipe",
              "A pipe.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Frames", "F", "Frames", GH_ParamAccess.list);
            pManager.AddNumberParameter("OuterRadius", "O", "Outer radius.", GH_ParamAccess.item);
            pManager.AddNumberParameter("InnerRadius", "I", "Inner radius.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Voxels", "V", "Output voxels", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Rhino.Geometry.Plane> frames = new List<Rhino.Geometry.Plane>();
            if (!DA.GetDataList(0, frames)) return;

            GH_Number outerRadius = new GH_Number();
            if (!DA.GetData(1, ref outerRadius)) return;

            GH_Number innerRadius = new GH_Number();
            if (!DA.GetData(2, ref outerRadius)) return;
            
            List<Vector3> aPoints = new List<Vector3>();


            for (int i = 0; i < frames.Count; i++)
            {
                Rhino.Geometry.Plane frame = frames[i];
                aPoints.Add(new Vector3((float)frame.OriginX, (float)frame.OriginY, (float)frame.OriginZ));
            }

            Frames localFrames = new Frames(aPoints, Frames.EFrameType.SPHERICAL);

            BasePipe pipe = new BasePipe(localFrames, (float)innerRadius.Value, (float)outerRadius.Value);

            PicoGHVoxels output = new PicoGHVoxels(
                pipe.voxConstruct(), pipe.mshConstruct());

            DA.SetData(0, output);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("53D17CD4-EBB5-4D8A-BEEC-4DB083BE3712"); }
        }
    }
}