using System;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Types;

namespace PicoGH
{
    public class CosModulation : GH_Component
    {

        public float A = 0;
        public float B = 0;
        public float C = 0;

        /// <summary>
        /// Initializes a new instance of the CosModulation class.
        /// </summary>
        public CosModulation()
            : base("PicoCosineModulation", "CosMod",
                "Constructs a 1D cosine modulation.",
                "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("AdditionTerm", "A", "Term A in y = A + B * Cos(C)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mult", "B", "Term B in A + B * Cos(C)", GH_ParamAccess.item);
            pManager.AddNumberParameter("Arg", "C", "Term C in A + B * Cos(C)", GH_ParamAccess.item);
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
            GH_Number _a = new GH_Number();
            GH_Number _b = new GH_Number();
            GH_Number _c = new GH_Number();

            if (!DA.GetData(0, ref _a)) return;
            if (!DA.GetData(1, ref _b)) return;
            if (!DA.GetData(2, ref _c)) return;

            A = (float)_a.Value;
            B = (float)_b.Value;
            C = (float)_c.Value;

            LineModulation lineMod = new LineModulation(CosFunction);
            PicoGHModulation modulation = new PicoGHModulation(lineMod);

            DA.SetData(0, modulation);
        }

        protected float CosFunction(float fPhi)
        {
            return A + B * (float)Math.Cos((double)C * (double)fPhi);
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
            get { return new Guid("675FA626-0A6C-4DDB-B17F-75C75BE15656"); }
        }
    }
}
