using System;
using System.Numerics;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Leap71.ShapeKernel;
using Rhino.Geometry;
using PicoGH.PicoGH.Types;
using PicoGH.PicoGH.Classes;

namespace PicoGH.PicoGH.Primitives
{
    public class Box : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Box class.
        /// </summary>
        public Box()
          : base("PicoBox", "Box",
              "Creates a box with a spine and modulation.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Frames", "F", "Frames", GH_ParamAccess.list);
            pManager.AddGenericParameter("WidthMod", "WMod", "Width modulation.", GH_ParamAccess.item);
            pManager.AddGenericParameter("DepthMod", "DMod", "Depth modulation.", GH_ParamAccess.item);
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

            if (frames.Count == 0) return;

            PicoGHModulation wMod = null;
            if (!DA.GetData(1, ref wMod)) return;

            PicoGHModulation hMod = null;
            if (!DA.GetData(2, ref hMod)) return;

            List<Vector3> aPoints = new List<Vector3>();

            for (int i = 0; i < frames.Count; i++)
            {
                Rhino.Geometry.Plane frame = frames[i];
                aPoints.Add(new Vector3((float)frame.OriginX, (float)frame.OriginY, (float)frame.OriginZ));
            }

            Frames localFrames = new Frames(aPoints, Frames.EFrameType.SPHERICAL);

            BaseBox box  = new BaseBox(localFrames);
            box.SetWidth(wMod._lModulation);
            box.SetDepth(hMod._lModulation);
            box.SetDepthSteps(100);
            box.SetWidthSteps(100);
            box.SetLengthSteps(100);

            PicoGHBox output = new PicoGHBox(box);

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
            get { return new Guid("A1883A08-B4D1-46FF-956D-1260EBE31B76"); }
        }
    }
}