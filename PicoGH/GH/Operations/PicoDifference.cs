using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using PicoGH.Classes;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.PicoGH.GH.Operations
{
    public class PicoDifference : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PicoDifference class.
        /// </summary>
        public PicoDifference()
          : base("PicoDifference", "Difference",
              "Calculates the boolean difference between two objects.",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OperandA", "A", "Operand A", GH_ParamAccess.item);
            pManager.AddGenericParameter("OperandB", "B", "Operand B", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings", "S", "PicoGH Settings", GH_ParamAccess.item);
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
            PicoGHVoxels a = new PicoGHVoxels();
            PicoGHVoxels b = new PicoGHVoxels();
            if (!DA.GetData(0, ref a)) return;
            if (!DA.GetData(1, ref b)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            Voxels result = new Voxels(a.PVoxels);
            result.BoolSubtract(b.PVoxels);

            PicoGHVoxels output = new PicoGHVoxels(result);

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
            get { return new Guid("204A6B89-19F8-4AD5-9625-C224AA700C41"); }
        }
    }
}