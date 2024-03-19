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

namespace PicoGH.Primitives
{
    public class Struts : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Struts class.
        /// </summary>
        public Struts()
          : base("PicoLines", "Lines",
              "Voxelise a list of beams. This only works for straight beams.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Beams", "B", "Input list of beams", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radius", "R", "Radius", GH_ParamAccess.item);
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
            List<GH_Curve> beams = new List<GH_Curve>();
            if (!DA.GetDataList(0, beams)) return;

            GH_Number radius = new GH_Number();
            if (!DA.GetData(1, ref radius)) return;

            Lattice lattice = new Lattice();

            // This needs to be set somehow. Some kind of global tolerance?
            int splineSubdivision = 10;
            double[] normalisedSplinePoints = new double[splineSubdivision];
            for (int i = 1; i < splineSubdivision; i++)
            {
                normalisedSplinePoints[i] = (double)i * (1 / (double)splineSubdivision);
            }

            foreach (GH_Curve inputBeam in beams)
            {
                Curve beam = inputBeam.Value;

                if (beam.Degree == 1)
                {
                    lattice.AddBeam(
                    new System.Numerics.Vector3(
                        (float)beam.PointAtStart.X,
                        (float)beam.PointAtStart.Y,
                        (float)beam.PointAtStart.Z),
                    (float)radius.Value,

                    new System.Numerics.Vector3(
                        (float)beam.PointAtEnd.X,
                        (float)beam.PointAtEnd.Y,
                        (float)beam.PointAtEnd.Z),
                    (float)radius.Value);
                }

                else
                {
                    List<Point3d> beamPoints = new List<Point3d>();
                    beamPoints.Add(beam.PointAtStart);
                    for (int i = 0; i < splineSubdivision; i++)
                    {
                        beamPoints.Add(beam.PointAtNormalizedLength(normalisedSplinePoints[i]));
                    }
                    beamPoints.Add(beam.PointAtEnd);

                    List<System.Numerics.Vector3> splinePoints = new List<System.Numerics.Vector3>();

                    foreach (Point3d point in beamPoints)
                    {
                        splinePoints.Add(new System.Numerics.Vector3((float)point.X, (float)point.Y, (float)point.Z));
                    }

                    for (int i = 1; i < splinePoints.Count; i++)
                    {
                        lattice.AddBeam(splinePoints[i - 1], (float)radius.Value, splinePoints[i], (float)radius.Value);
                    }
                }

            }

            PicoGHVoxels outputVoxels = new PicoGHVoxels(new Voxels(lattice));
            DA.SetData(0, outputVoxels);
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
            get { return new Guid("5479A286-4949-4DAE-B3ED-B1CF71D015B6"); }
        }
    }
}