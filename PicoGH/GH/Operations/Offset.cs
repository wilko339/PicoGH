using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.PicoGH.GH.Operations
{
    public class Offset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Offset class.
        /// </summary>
        public Offset()
          : base("PicoOffset", "Offset",
              "Offsets a voxel object by the given amount.",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "Input voxels.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "O", "Offset distance", GH_ParamAccess.item);
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

            GH_Number offset = new GH_Number();
            if (!DA.GetData(1, ref offset)) return;

            Voxels outputVoxels = new Voxels(inputVoxels.PVoxels);
            outputVoxels.Offset((float)offset.Value);

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
            get { return new Guid("6E84586E-E4B6-4985-8BF5-20CDBA796E69"); }
        }
    }
}