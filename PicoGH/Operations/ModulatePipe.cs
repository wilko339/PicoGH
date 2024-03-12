using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.PicoGH.Types;
using PicoGH.Types;
using Rhino.Geometry;

namespace PicoGH.PicoGH.Operations
{
    public class ModulatePipe : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModulatePipe class.
        /// </summary>
        public ModulatePipe()
          : base("PicoModulatePipe", "ModPipe",
              "Applies surface modulations to change the inner and outer radii of a pipe or pipe segment.",
              "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Voxels", "V", "Input pipe or pipe segment voxels.", GH_ParamAccess.item);
            pManager.AddGenericParameter("InnerMod", "I", "Modulation to apply to the inner radius.", GH_ParamAccess.item);
            pManager.AddGenericParameter("OuterMod", "I", "Modulation to apply to the outer radius.", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OutputVoxels", "V", "Output voxels.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PicoGHPipeSegment pipeSegment = null;
            if (!DA.GetData(0, ref pipeSegment)) return;

            PicoGHModulation innerMod = null;
            PicoGHModulation outerMod = null;

            if (!DA.GetData(1, ref innerMod)) return;
            if (!DA.GetData(2, ref outerMod)) return;

            SurfaceModulation innerSurf = null;
            SurfaceModulation outerSurf = null;

            if (innerMod._sModulation is null)
            {
                innerSurf = new SurfaceModulation(innerMod._lModulation);
            }
            else
            {
                innerSurf = innerMod._sModulation;
            }

            if (outerMod._sModulation is null)
            {
                outerSurf = new SurfaceModulation(outerMod._lModulation);
            }
            else
            {
                outerSurf = outerMod._sModulation;
            }

            pipeSegment._pipeSegment.SetRadius(innerSurf, outerSurf);

            // I need some kind of method that sets the mesh resolution automatically...
            pipeSegment._pipeSegment.SetLengthSteps(100);

            PicoGHPipeSegment output = new PicoGHPipeSegment(pipeSegment._pipeSegment);

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
            get { return new Guid("58BC858D-CAD3-44C9-B43D-A2CABFB47EAF"); }
        }
    }
}