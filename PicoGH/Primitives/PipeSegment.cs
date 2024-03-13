using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Types;

namespace PicoGH
{
    public class PipeSegment : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PipeSegment class.
        /// </summary>
        public PipeSegment()
          : base("PicoPipeSegment", "PipeSegment",
              "A pipe segment (with modulations...).",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Frames", "F", "Frames", GH_ParamAccess.list);
            pManager.AddNumberParameter("OuterRadius", "O", "Outer radius.", GH_ParamAccess.item, 10.0);
            pManager.AddNumberParameter("InnerRadius", "I", "Inner radius.", GH_ParamAccess.item, 5.0);
            pManager.AddGenericParameter("StartMod", "SMod", "Start modulation. Modulates the circumferential position of the pipe segment (twisting).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RangeMod", "RMod", "Range modulation. Modulates the range of the pipe segment circumferentially.", GH_ParamAccess.item);
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
            if (!DA.GetData(2, ref innerRadius)) return;

            PicoGHModulation sMod = null;
            if (!DA.GetData(3, ref sMod))
            {
                sMod = new PicoGHModulation(new LineModulation(0));
            }

            PicoGHModulation rMod = null;
            if (!DA.GetData(4, ref rMod))
            {
                rMod = new PicoGHModulation(new LineModulation((float)Math.PI * 2));
            }

            Frames localFrames = Utilities.RhinoPlanesToPicoFrames(frames);
            BasePipeSegment pipeSegment = new BasePipeSegment(localFrames, (float)innerRadius.Value, (float)outerRadius.Value, sMod._lModulation, rMod._lModulation, BasePipeSegment.EMethod.MID_RANGE);

            PicoGHPipeSegment output = new PicoGHPipeSegment(pipeSegment);

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
            get { return new Guid("5D9D7FD0-B521-443C-8068-1AACD2F8BF69"); }
        }
    }
}