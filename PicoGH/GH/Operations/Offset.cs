﻿// Copyright 2024 Toby Wilkinson
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
using PicoGK;

namespace PicoGH.GH.Operations
{
    public class Offset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Offset class.
        /// </summary>
        public Offset()
          : base("PicoOffset", "Offset",
              "Offsets a voxel object by the given amount.",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "Input voxels.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "O", "Offset distance", GH_ParamAccess.item);
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
            PicoGHVoxels inputVoxels = new PicoGHVoxels();
            if (!DA.GetData(0, ref inputVoxels)) return;

            GH_Number offset = new GH_Number();
            if (!DA.GetData(1, ref offset)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            Voxels outputVoxels = new Voxels(inputVoxels.PVoxels);

            outputVoxels.Offset((float)offset.Value);
            PicoGHVoxels output = new PicoGHVoxels(outputVoxels);

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
            get { return new Guid("6E84586E-E4B6-4985-8BF5-20CDBA796E69"); }
        }
    }
}