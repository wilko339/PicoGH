using System;
using System.Numerics;
using System.Collections.Generic;

using Grasshopper.Kernel;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.PicoGH.IO
{
    public class Mesh2Voxels : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Mesh2Voxels class.
        /// </summary>
        public Mesh2Voxels()
          : base("PicoMesh2Voxels", "Mesh2Voxels",
              "Converts an input triangle _rmesh to a voxel representation",
              "PicoGH", "IO")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "Input _rmesh to convert", GH_ParamAccess.item);
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
            Rhino.Geometry.Mesh inputMesh = new Rhino.Geometry.Mesh();
            if (!DA.GetData(0, ref inputMesh)) { return; }

            PicoGK.Mesh pMesh = new PicoGK.Mesh();

            foreach (var vertex in  inputMesh.Vertices)
            {
                pMesh.nAddVertex(new Vector3((float)vertex.X, (float)vertex.Y, (float)vertex.Z));
            }

            foreach (var meshFace in inputMesh.Faces)
            {
                Triangle triangle = new Triangle(meshFace.A, meshFace.B, meshFace.C);
                pMesh.nAddTriangle(triangle);
            }

            Voxels voxels = new Voxels(pMesh);

            var output = new PicoGHVoxels(voxels);
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
            get { return new Guid("F0F9388A-64FF-4822-82E5-D72C898739C2"); }
        }
    }
}