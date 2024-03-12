using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PicoGH.PicoGH.IO
{
    public class Voxels2Mesh : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Voxels2Mesh class.
        /// </summary>
        public Voxels2Mesh()
          : base("PicoVoxels2Mesh", "Voxels2Mesh",
              "Converts the voxel object to a _rmesh.",
              "PicoGH", "IO")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("InputVoxels", "V", "Input voxel object.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Output Mesh", "M", "Output _rmesh.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PicoGHVoxels inputVoxels = new PicoGHVoxels();
            if (!DA.GetData(0, ref inputVoxels)) return;

            PicoGK.Mesh outputMesh = new PicoGK.Mesh(inputVoxels._pvoxels);

            DA.SetData(0, Utilities.PicoMeshToRhinoMesh(outputMesh));

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
            get { return new Guid("4AB4BC6C-0162-41E9-A91D-87C406DCA150"); }
        }
    }
}