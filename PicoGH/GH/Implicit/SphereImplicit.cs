using System;
using System.Numerics;

using Leap71.ShapeKernel;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGH.Classes;
using PicoGH.PicoGH.Classes;

namespace PicoGH
{
    public class SphereImplicit : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImplicitSphere class.
        /// </summary>
        public SphereImplicit()
          : base("PicoImplicitSphere", "Sphere",
              "Generates a sphere implicitly.",
              "PicoGH", "Implicit")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Centre", "C", "Centre", GH_ParamAccess.item,
                Rhino.Geometry.Plane.WorldXY);
            pManager.AddNumberParameter("Radius", "R", "Radius", GH_ParamAccess.item, 5.0d);
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
            var basePlane = new Rhino.Geometry.Plane();
            if (!DA.GetData(0, ref basePlane)) return;

            var radius = new GH_Number();
            if (!DA.GetData(1, ref radius)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            var origin = new Vector3(
                (float)basePlane.OriginX, 
                (float)basePlane.OriginY, 
                (float)basePlane.OriginZ);

            var sphere = new ImplicitSphere(origin, (float)radius.Value);

            var _voxels = new PicoGHImplicitSphere(sphere);

            DA.SetData(0, _voxels);
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
            get { return new Guid("065D0C24-A7D4-490A-B0AA-BADA4B42C0E3"); }
        }
    }
}