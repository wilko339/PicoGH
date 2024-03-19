using System;

using Grasshopper.Kernel;
using PicoGH.Interfaces;
using PicoGH.Classes;

namespace PicoGH.Operations
{
    public class ApplyModulation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModulatePipe class.
        /// </summary>
        public ApplyModulation()
          : base("PicoApplyModulation", "ApplyMod",
              "Applies surface modulations to change the inner and outer radii of a compatible object.",
              "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape", "S", "Input shape to apply modulations.", GH_ParamAccess.item);
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
            IModulate inputData = null;
            if (!DA.GetData(0, ref inputData)) return;

            if (!(inputData is IModulate)) return;

            PicoGHModulation modulation1 = null;
            PicoGHModulation modulation2 = null;

            if (!DA.GetData(1, ref modulation1)) return;
            if (!DA.GetData(2, ref modulation2)) return;

            IModulate output = inputData.DeepCopy() as IModulate;

            // Need to look at setting the mesh steps when we apply modulations...
            output.SetModulation(modulation1, modulation2);

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