using System;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Leap71.ShapeKernel;
using PicoGH.Classes;

namespace PicoGH.Modulations
{
    public class ConstantLineModulation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConstantLineModulation class.
        /// </summary>
        public ConstantLineModulation()
          : base("PicoConstantLineModulation", "ConstLineMod",
              "Constructs a constant line modulation.",
              "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Value", "V", "Constant value", GH_ParamAccess.item, (float)Math.PI);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Output Modulation", "M", "Output modulation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Number input = new GH_Number();
            if (!DA.GetData(0, ref input)) return; 

            LineModulation lineMod = new LineModulation((float)input.Value);
            PicoGHModulation output = new PicoGHModulation(lineMod);

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
            get { return new Guid("EFA31BA1-0752-47F0-A86D-7B41247A8C09"); }
        }
    }
}