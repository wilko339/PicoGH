using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Classes;
using Rhino.Geometry;

namespace PicoGH
{
    public class GenericImplicit : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PicoGHGenericImplicit class.
        /// </summary>
        public GenericImplicit()
          : base("PicoGenericImplicit", "GenericImplicit",
              "Evaluates the input implicit equation in the bounding box.",
              "PicoGH", "Implicit")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Function", "F", "Input implicit function of the form Func<float float float float>.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Region", "R", "Input voxel region.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings", "S", "PicoGH Settings", GH_ParamAccess.item);
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
            GH_ObjectWrapper implicitFunction = null;
            if (!DA.GetData(0, ref implicitFunction)) return;

            PicoGHVoxels voxelRegion = new PicoGHVoxels();
            if (!DA.GetData(1, ref voxelRegion)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            ImplicitGeneric implicitGeneric = new ImplicitGeneric(
                (Func<float, float, float, float>)implicitFunction.Value,
                voxelRegion.PicoBoundingBox);

            PicoGHGenericImplicit output = new PicoGHGenericImplicit(implicitGeneric, voxelRegion);

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
            get { return new Guid("70FB70F4-8D3A-4D6E-AD43-7F4E5100389B"); }
        }
    }
}