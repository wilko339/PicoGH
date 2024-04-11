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
using Grasshopper.Kernel.Types;
using PicoGH.Classes;

namespace PicoGH.Config
{
    public class PicoGHConfig : GH_Component
    {
        // Set the global voxel size
        private float _VoxelSize = 0.5f;

        // Set a coarsening factor for the intermediate ShapeKernel meshes
        public int _meshCoarseFactor = 4;

        /// <summary>
        /// Initializes a new instance of the PicoGHConfig class.
        /// </summary>
        public PicoGHConfig()
          : base("PicoGHSettings", "Settings",
              "Sets some options for PicoGK",
              "PicoGH", "Settings")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("VoxelSize", "V", "Sets the global voxel size for PicoGK. Set higher for faster generation, lower for better resolution.", GH_ParamAccess.item, 0.5);
            pManager.AddNumberParameter("MeshAdaptivity", "A", "Sets the mesh adaptivity (0-1). Higher values give more decimation.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("MeshCoarsen", "M", "A coarsening factor for the intermediate meshes. Higher is more coarse (faster).", GH_ParamAccess.item, 4);
            pManager.AddBooleanParameter("Triangulate", "T", "Triangulate output meshes.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Settings", "S", "PicoGH Settings", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Number voxelSize = new GH_Number();
            if (!DA.GetData(0, ref voxelSize)) return;

            GH_Number meshAdaptivity = new GH_Number();
            if (!DA.GetData(1, ref meshAdaptivity)) return;

            GH_Integer meshCoarseningFactor = new GH_Integer();
            if (!DA.GetData(2, ref  meshCoarseningFactor)) return;

            GH_Boolean triangulateMeshes = new GH_Boolean();
            if (!DA.GetData(3, ref triangulateMeshes)) return;

            PicoGHSettings settings = new PicoGHSettings(
                (float)voxelSize.Value, 
                (float)meshAdaptivity.Value,
                triangulateMeshes.Value, 
                (uint)meshCoarseningFactor.Value);

            Utilities.SetGlobalSettings(settings);

            DA.SetData(0, settings);
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