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
using System.Numerics;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.ShapeKernel;
using PicoGH.Classes;

namespace PicoGH
{
    public class PicoCube : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PicoCube()
          : base("PicoCube", "Cube",
            "A simple cube.",
            "PicoGH", "Primitives")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("BasePlane", "P", "Base plane", GH_ParamAccess.item,
                Rhino.Geometry.Plane.WorldXY);
            pManager.AddNumberParameter("EdgeLength", "L", "Edge length", GH_ParamAccess.item, 10.0d);
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
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var basePlane = new Rhino.Geometry.Plane();
            if (!DA.GetData(0, ref basePlane)) return;

            var edgeLength = new GH_Number();
            if (!DA.GetData(1, ref edgeLength)) return;

            var origin = basePlane.Origin;
            var baseFrame = new LocalFrame(new Vector3((float)origin.X, (float)origin.Y, (float)origin.Z));
            var box = new BaseBox(baseFrame, (float)edgeLength.Value, (float)edgeLength.Value, (float)edgeLength.Value);

            var voxels = new PicoGHBox(box);

            DA.SetData(0, voxels);
        }


        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("4030b5bb-ed9e-4cf4-9933-20e1bb2d7a87");
    }
}