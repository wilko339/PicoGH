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
using PicoGH.PicoGH.Classes;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH
{
    public class PipeSegment : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PipeSegment class.
        /// </summary>
        public PipeSegment()
          : base("PicoPipeSegment", "PipeSegment",
              "A pipe segment.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Input Curve/s", GH_ParamAccess.item);
            pManager.AddNumberParameter("InnerRadius", "I", "Inner _radius.", GH_ParamAccess.item);
            pManager.AddNumberParameter("OuterRadius", "O", "Outer _radius.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("CurveDivs", "D", "Number of curve divisions to create the pipe", GH_ParamAccess.item);
            pManager.AddGenericParameter("StartMod", "SMod", "Start modulation. Modulates the circumferential position of the pipe segment (twisting).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RangeMod", "RMod", "Range modulation. Modulates the range of the pipe segment circumferentially.", GH_ParamAccess.item);
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
            GH_Curve curve = new GH_Curve();
            if (!DA.GetData(0, ref curve)) return;

            GH_Number innerRadius = new GH_Number();
            if (!DA.GetData(1, ref innerRadius)) return;

            GH_Number outerRadius = new GH_Number();
            if (!DA.GetData(2, ref outerRadius)) return;

            GH_Integer curveDivisions = new GH_Integer();
            if (!DA.GetData(3, ref curveDivisions)) return;

            PicoGHModulation sMod = null;
            if (!DA.GetData(4, ref sMod)) return;

            PicoGHModulation rMod = null;
            if (!DA.GetData(5, ref rMod)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            List<double> normalisedCurveParameters = new List<double>();

            for (int i = 0; i < curveDivisions.Value + 1; i++)
            {
                normalisedCurveParameters.Add((double)i * 1 / curveDivisions.Value);
            }

            List<Plane> curveFrames = new List<Plane>();

            foreach (double parameter in normalisedCurveParameters)
            {
                double curveParam = curve.Value.Domain.ParameterAt(parameter);
                curve.Value.PerpendicularFrameAt(curveParam, out Plane frame);
                curveFrames.Add(frame);
            }

            Frames picoFrames = Utilities.RhinoPlanesToPicoFrames(curveFrames);
            BasePipeSegment pipe = new BasePipeSegment(
                picoFrames, 
                (float)innerRadius.Value, 
                (float)outerRadius.Value, 
                sMod.LineModulation, 
                rMod.LineModulation, 
                BasePipeSegment.EMethod.MID_RANGE);

            // Here we make sure the construction / preview mesh isn't too overkill
            double curveLength = curve.Value.GetLength();

            uint lengthSteps = 2;

            if (curve.Value.Degree > 1) lengthSteps = (uint)Math.Ceiling(curveLength / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

            uint radialSteps = (uint)Math.Ceiling(
                (outerRadius.Value - innerRadius.Value) / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

            double circumference = 2 * Math.PI * outerRadius.Value;
            uint polarSteps = (uint)Math.Ceiling(circumference / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

            PicoGHPipeSegment outputPipe = new PicoGHPipeSegment(pipe, lengthSteps, radialSteps, polarSteps);

            DA.SetData(0, outputPipe);
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
            get { return new Guid("5D9D7FD0-B521-443C-8068-1AACD2F8BF69"); }
        }
    }
}