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
using Leap71.ShapeKernel;
using PicoGH.Classes;

namespace PicoGH.PicoGH.GH.Modulations
{
    public class DiscreteLineModulation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DiscreteLineModulation class.
        /// </summary>
        public DiscreteLineModulation()
          : base("DiscreteLineModulation", "DiscreteMod",
              "Creates a line modulation from a list of scalar values and a list of line parameters. If no line parameters are supplied, the list of scalar values is even distributed across the line.",
              "PicoGH", "Modulations")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "V", "Input values", GH_ParamAccess.list);
            pManager.AddNumberParameter("Parameters", "P", "Line parameter values. These will be automatically normalised to between 0 and 1.", GH_ParamAccess.list);
            pManager[1].Optional = true;
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
            List<GH_Number> modValues = new List<GH_Number>();
            if (!DA.GetDataList(0, modValues)) return;

            List<GH_Number> parameters = new List<GH_Number>();
            DA.GetDataList(1, parameters);

            LineModulation lineModulation = null;

            if (parameters.Count == 0)
            {
                // The case when only a single mod value is passed in, we return a constant modulation.
                if (modValues.Count == 1)
                {
                    lineModulation = new LineModulation((float)modValues[0].Value);
                }

                // Here, we evenly distribute the values across the curve based on the number of parameters provided
                if (modValues.Count > 1)
                {
                    List<float> lengthParameters = new List<float>();
                    List<float> modulationValues = new List<float>();

                    for (int i = 0; i < modValues.Count; i++)
                    {
                        modulationValues.Add((float)modValues[i].Value);
                        lengthParameters.Add((float)i * 1 / ((float)modValues.Count - 1));
                    }
                    lineModulation = new LineModulation(modulationValues, lengthParameters);
                }
            }

            PicoGHModulation outputModulation = new PicoGHModulation(lineModulation);

            DA.SetData(0, outputModulation);
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
            get { return new Guid("D684B50E-AA76-49E8-BDA6-E40A05100BB0"); }
        }
    }
}