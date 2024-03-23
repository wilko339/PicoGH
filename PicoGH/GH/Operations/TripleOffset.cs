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
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.PicoGH.GH.Operations
{
    public class TripleOffset : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Smoothen class.
        /// </summary>
        public TripleOffset()
          : base("PicoTripleOffset", "TripleOffset",
              "Performs a triple offset by the specified amaount (in, out, in). Useful for removing sharp internal and external features while maintaining wall thicknesses. Use a negative number to reverse the order of operations. Thin features may disappear.",
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

            Voxels outputVoxels = new Voxels(inputVoxels.PVoxels);
            outputVoxels.TripleOffset((float)offset.Value);

            DA.SetData(0, new PicoGHVoxels(outputVoxels));
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
            get { return new Guid("E7043982-A093-4020-8ED6-320FD583DA58"); }
        }
    }
}