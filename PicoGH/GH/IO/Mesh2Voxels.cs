// Copyright 2024 Toby Wilkinson
//
//  Licensed under the Apache License, Version 2.0 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0 
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the License for the specific language governing permissions and 
//  limitations under the License.

using System;

using Grasshopper.Kernel;
using PicoGK;

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
            if (!DA.GetData(0, ref inputMesh)) return;

            // PicoGK.Mesh pMesh = Utilities.RhinoMeshToPicoMesh(inputMesh);

            // Voxels voxels = new Voxels(pMesh);

            var output = new PicoGHVoxels(inputMesh);
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