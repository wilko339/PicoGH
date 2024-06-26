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
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Classes;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.Primitives
{
    public class Pipe : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Pipe class.
        /// </summary>
        public Pipe()
          : base("PicoPipe", "Pipe",
              "A pipe.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Input Curve/s", GH_ParamAccess.list);
            pManager.AddNumberParameter("InnerRadius", "I", "Inner _radius.", GH_ParamAccess.list);
            pManager.AddNumberParameter("OuterRadius", "O", "Outer _radius.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("CurveDivs", "D", "Number of curve divisions to create the pipe", GH_ParamAccess.item);
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
            List<GH_Curve> curves = new List<GH_Curve>();
            if (!DA.GetDataList(0, curves)) return;

            List<GH_Number> innerRadii = new List<GH_Number>();
            if (!DA.GetDataList(1, innerRadii)) return;

            List<GH_Number> outerRadii = new List<GH_Number>();
            if (!DA.GetDataList(2, outerRadii)) return;

            GH_Integer curveDivisions = new GH_Integer();
            if (!DA.GetData(3, ref curveDivisions)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            List<double> normalisedCurveParameters = new List<double>();

            for (int i = 0; i < curveDivisions.Value + 1; i++)
            {
                normalisedCurveParameters.Add((double)i * 1 / curveDivisions.Value);
            }

            List<PicoGHPipe> outputPipes = new List<PicoGHPipe>();

            float outerRadius = 1;
            float innerRadius = 0;

            for(int i = 0; i < curves.Count; i++)
            {
                Curve curve = curves[i].Value;

                if (outerRadii.Count == curves.Count)
                {
                    outerRadius = (float)outerRadii[i].Value;
                }
                else if (outerRadii.Count == 1)
                {
                    outerRadius = (float)outerRadii[0].Value;
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radii list must equal the number of curves or only contain a single value.");
                }

                if (innerRadii.Count == curves.Count)
                {
                    innerRadius = (float)innerRadii[i].Value;
                }
                else if (innerRadii.Count == 1)
                {
                    innerRadius = (float)innerRadii[0].Value;
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Inner radii list must equal the number of curves or only contain a single value.");
                }

                List<Plane> curveFrames = new List<Plane>();

                foreach (double parameter in normalisedCurveParameters)
                {
                    double curveParam = curve.Domain.ParameterAt(parameter);
                    curve.PerpendicularFrameAt(curveParam, out Plane frame);
                    curveFrames.Add(frame);
                }

                Frames picoFrames = Utilities.RhinoPlanesToPicoFrames(curveFrames);
                BasePipe pipe = new BasePipe(picoFrames, innerRadius, outerRadius);

                // Here we make sure the construction / preview mesh isn't too overkill
                double curveLength = curve.GetLength();

                uint lengthSteps;

                if (curve.Degree > 1)
                {
                    lengthSteps = (uint)Math.Ceiling(curveLength / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));
                }
                else
                {
                    lengthSteps = 2;
                }

                uint radialSteps = (uint)Math.Ceiling(
                    (outerRadius - innerRadius) / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

                double circumference = 2 * Math.PI * outerRadius;
                uint polarSteps = (uint)Math.Ceiling(circumference / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

                outputPipes.Add(new PicoGHPipe(pipe, lengthSteps, radialSteps, polarSteps));
            }

            DA.SetDataList(0, outputPipes);
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
            get { return new Guid("53D17CD4-EBB5-4D8A-BEEC-4DB083BE3712"); }
        }
    }
}