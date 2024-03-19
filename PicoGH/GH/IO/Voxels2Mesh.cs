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

            PicoGK.Mesh pMesh = inputVoxels.GeneratePMesh();
            Rhino.Geometry.Mesh rMesh = Utilities.PicoMeshToRhinoMesh(pMesh);

            if (!rMesh.IsValid)
            {
                PicoGK.Mesh tempMesh = new PicoGK.Mesh(inputVoxels.GenerateVoxels());
                rMesh = Utilities.PicoMeshToRhinoMesh(tempMesh);
            }

            if (!rMesh.IsValid)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Output mesh is invalid.");
            }

            DA.SetData(0, rMesh);
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