using System;
using System.Numerics;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH
{
    public class Sphere : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Sphere class.
        /// </summary>
        public Sphere()
          : base("PicoSphere", "Sphere",
              "A simple sphere.",
              "PicoGH", "Primitives")
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
            Rhino.Geometry.Plane basePlane = new Rhino.Geometry.Plane();
            if (!DA.GetData(0, ref basePlane)) return;

            GH_Number radius = new GH_Number();
            if (!DA.GetData(1, ref radius)) return;

            Point3d origin = basePlane.Origin;
            LocalFrame baseFrame = new LocalFrame(new Vector3((float)origin.X, (float)origin.Y, (float)origin.Z));

            BaseSphere sphere = new BaseSphere(baseFrame, (float)radius.Value);
            sphere.SetPolarSteps(10);
            sphere.SetAzimuthalSteps(10);

            Voxels voxels = sphere.voxConstruct();
            PicoGK.Mesh mesh = sphere.mshConstruct();

            var _voxels = new PicoGHVoxels(voxels, mesh);

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
            get { return new Guid("C0048C47-E40D-4014-85AD-7F078A1EF7E4"); }
        }
    }
}