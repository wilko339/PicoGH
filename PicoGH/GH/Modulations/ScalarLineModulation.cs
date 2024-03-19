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
using Leap71.ShapeKernel;
using PicoGH.Classes;

namespace PicoGH.Modulations
{
    public class ScalarLineModulation : GH_Component
    {
        private static float A;
        private static float B;

        /// <summary>
        /// Initializes a new instance of the ScalarLineModulation class.
        /// </summary>
        public ScalarLineModulation()
          : base("PicoScalarLineModulation", "ScalarLineMod",
              "Returns a scalar line modulation of the form A + Bx",
              "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("A", "A", "Term A in A + Bx.", GH_ParamAccess.item, 0f);
            pManager.AddNumberParameter("B", "B", "Term B in A + Bx.", GH_ParamAccess.item, 1f);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Output Modulation", "M", "Output modulation.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Number a = new GH_Number();
            if (!DA.GetData(0, ref a)) return;

            GH_Number b = new GH_Number();
            if (!DA.GetData(1, ref b)) return;

            A = (float)a.Value;
            B = (float)b.Value;

            PicoGHModulation modulation = new PicoGHModulation(new LineModulation(ScalarFunction));

            DA.SetData(0, modulation);
        }

        protected static float ScalarFunction(float x)
        {
            return A + B * x;
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
            get { return new Guid("D7174974-E410-4FE7-AB45-55D195EAA48B"); }
        }
    }
}