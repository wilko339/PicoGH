using System;
using System.Collections.Generic;
using System.Numerics;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace PicoGH.Operations
{
    public class PicoVoxRayIntersection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PicoVoxRayIntersection class.
        /// </summary>
        public PicoVoxRayIntersection()
          : base("PicoVoxRayIntersection", "VoxRayIntersection",
              "Calculates the intersection between a ray and a voxel field. ",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Voxels", "I", "Input voxels.", GH_ParamAccess.item);
            pManager.AddPointParameter("Origin", "O", "Ray origina.", GH_ParamAccess.list);
            pManager.AddVectorParameter("Direction", "D", "Ray direction", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "First intersection point.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Hit", "H", "Boolean indicating hit or miss.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PicoGHVoxels inputVoxels = new PicoGHVoxels();
            if (!DA.GetData(0, ref inputVoxels)) return;

            List<GH_Point> origins = new List<GH_Point>();
            if (!DA.GetDataList(1, origins)) return;

            List<GH_Vector> directions = new List<GH_Vector>();
            if (!DA.GetDataList(2, directions)) return;

            List<GH_Point> outputPoints = new List<GH_Point>();
            List<bool> success = new List<bool>();

            for (int i = 0; i < origins.Count; i++)
            {
                var _origin = origins[i].Value;
                Vector3 origin = new Vector3((float)_origin.X, (float)_origin.Y, (float)_origin.Z);

                Vector3 direction;

                if (directions.Count != origins.Count)
                {
                    var _direction = directions[0].Value;
                    direction = new Vector3((float)_direction.X, (float)_direction.Y, (float)_direction.Z);
                }
                else
                {
                    var _direction = directions[i].Value;
                    direction = new Vector3((float)_direction.X, (float)_direction.Y, (float)_direction.Z);
                }

                if (inputVoxels.PVoxels.bRayCastToSurface(origin, direction, out Vector3 surfacePoint))
                {
                    outputPoints.Add(new GH_Point(new Point3d(surfacePoint.X,  surfacePoint.Y, surfacePoint.Z)));
                    success.Add(true);
                }
                else
                {
                    outputPoints.Add(null);
                    success.Add(false);
                }
            }
            DA.SetDataList(0, outputPoints);
            DA.SetDataList(1, success);
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
            get { return new Guid("E8E8F6D5-A7E2-43D9-B79E-FA8297C4E2D0"); }
        }
    }
}