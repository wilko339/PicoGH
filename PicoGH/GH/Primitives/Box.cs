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
using PicoGK;
using Rhino.Geometry;

namespace PicoGH.Primitives
{
    public class Box : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Box class.
        /// </summary>
        public Box()
          : base("PicoBox", "Box",
              "Creates a box along a curve.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "Input Curve/s", GH_ParamAccess.list);
            pManager.AddNumberParameter("Width", "W", "Width", GH_ParamAccess.list);
            pManager.AddNumberParameter("Depth", "D", "Depth", GH_ParamAccess.list);
            pManager.AddIntegerParameter("CurveDivs", "D", "Number of curve divisions to create the box", GH_ParamAccess.item);
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

            List<GH_Number> widths = new List<GH_Number>();
            if (!DA.GetDataList(1, widths)) return;

            List<GH_Number> depths = new List<GH_Number>();
            if (!DA.GetDataList(2, depths)) return;

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

            List<PicoGHBox> outputBoxes = new List<PicoGHBox>();

            float width = 1;
            float depth = 0;

            for (int i = 0; i < curves.Count; i++)
            {
                Curve curve = curves[i].Value;

                if (widths.Count == curves.Count)
                {
                    width = (float)widths[i].Value;
                }
                else if (widths.Count == 1)
                {
                    width = (float)widths[0].Value;
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Outer radii list must equal the number of curves or only contain a single value.");
                }

                if (depths.Count == curves.Count)
                {
                    depth = (float)depths[i].Value;
                }
                else if (depths.Count == 1)
                {
                    depth = (float)depths[0].Value;
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
                BaseBox box = new BaseBox(picoFrames, width, depth);

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

                uint widthSteps = (uint)Math.Ceiling(
                    width / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

                uint depthSteps = (uint)Math.Ceiling(
                    depth / (Library.fVoxelSizeMM * Library.iMeshCoarseningFactor));

                outputBoxes.Add(new PicoGHBox(box, lengthSteps, widthSteps, depthSteps));
            }

            DA.SetDataList(0, outputBoxes);
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
            get { return new Guid("A1883A08-B4D1-46FF-956D-1260EBE31B76"); }
        }
    }
}