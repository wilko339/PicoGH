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
using System.Collections.Generic;

using Grasshopper.Kernel;
using PicoGK;

namespace PicoGH
{
    public class ListUnion : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ListUnion class.
        /// </summary>
        public ListUnion()
          : base("PicoListUnion", "ListUnion",
              "Unions a list of voxel objects",
              "PicoGH", "Operations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Input", "I", "Input list of voxel objects to union.", GH_ParamAccess.list);
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
            List<PicoGHVoxels> input = new List<PicoGHVoxels>();
            if (!DA.GetDataList(0, input)) return;

            Voxels boolVox = new Voxels();

            foreach (PicoGHVoxels vox in input)
            {
                if (vox.PVoxels is null)
                {
                    Voxels voxels = vox.GenerateVoxels();
                    boolVox.BoolAdd(vox.GenerateVoxels());
                }
                else
                {
                    boolVox.BoolAdd(vox.PVoxels);
                }
            }

            PicoGHVoxels output = new PicoGHVoxels(boolVox);

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
            get { return new Guid("5A35211D-AC04-4816-97A1-249B34B00D0B"); }
        }
    }
}