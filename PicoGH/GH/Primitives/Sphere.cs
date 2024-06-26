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
using System.Numerics;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Classes;

namespace PicoGH
{
    public class Sphere : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Sphere class.
        /// </summary>
        public Sphere()
          : base("PicoSphere", "Sphere",
              "A simple sphere.",
              "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Centre", "C", "Centre", GH_ParamAccess.item,
                Rhino.Geometry.Plane.WorldXY);
            pManager.AddNumberParameter("Radius", "R", "Radius", GH_ParamAccess.item, 5.0d);
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
            var basePlane = new Rhino.Geometry.Plane();
            if (!DA.GetData(0, ref basePlane)) return;

            var radius = new GH_Number();
            if (!DA.GetData(1, ref radius)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            var origin = basePlane.Origin;
            var baseFrame = new LocalFrame(new Vector3((float)origin.X, (float)origin.Y, (float)origin.Z));

            var sphere = new BaseSphere(baseFrame, (float)radius.Value);
            sphere.SetPolarSteps(10);
            sphere.SetAzimuthalSteps(10);

            var _voxels = new PicoGHSphere(sphere);

            DA.SetData(0, _voxels);
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
            get { return new Guid("C0048C47-E40D-4014-85AD-7F078A1EF7E4"); }
        }
    }
}