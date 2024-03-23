using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.PicoGH.GH.Operations
{
    public class DoubleOffset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DoubleOffset class.
        /// </summary>
        public DoubleOffset()
          : base("PicoDoubleOffset", "DoubleOffset",
              "\"Performs a double offset by the specified amount (out, in). Useful for removing sharp internal and external features while maintaining wall thicknesses. Use a negative number to reverse the order of operations. Thin features may disappear.\"",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "Input voxels.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset1", "O1", "Offset distance for the first operation.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset2", "O2", "Offset distance for the second operation.", GH_ParamAccess.item);
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
            PicoGHVoxels inputVoxels = new PicoGHVoxels();
            if (!DA.GetData(0, ref inputVoxels)) return;

            GH_Number offset1 = new GH_Number();
            if (!DA.GetData(1, ref offset1)) return;

            GH_Number offset2 = new GH_Number();
            if (!DA.GetData(2, ref offset2)) return;

            Voxels outputVoxels = new Voxels(inputVoxels.PVoxels);
            outputVoxels.DoubleOffset((float)offset1.Value, (float)offset2.Value);

            DA.SetData(0, new PicoGHVoxels(outputVoxels));
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
            get { return new Guid("286C92E0-AF70-42A7-9CE9-3D15B15BAEEE"); }
        }
    }
}