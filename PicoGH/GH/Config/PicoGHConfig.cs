using System;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.Config
{
    public class PicoGHConfig : GH_Component
    {
        float _VoxelSize = 0.5f;

        /// <summary>
        /// Initializes a new instance of the PicoGHConfig class.
        /// </summary>
        public PicoGHConfig()
          : base("PicoGHConfig", "Config",
              "Sets some options for PicoGK",
              "PicoGH", "Config")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("VoxelSize", "V", "Sets the global voxel size for PicoGK.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Number voxelSize = new GH_Number();
            if (!DA.GetData(0, ref voxelSize)) return;

            Library.SetVoxelSize((float)voxelSize.Value);
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
            get { return new Guid("5EF191F8-14D0-48C3-B30E-CB526D858EA1"); }
        }
    }
}