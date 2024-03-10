using System;
using System.Numerics;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using PicoGK;
using Leap71.ShapeKernel;
using System.Diagnostics;
using Rhino.Geometry.Collections;
using Rhino.DocObjects;

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
            Rhino.Geometry.Plane basePlane = new Rhino.Geometry.Plane();
            if (!DA.GetData(0, ref basePlane)) return;

            GH_Number edgeLength = new GH_Number();
            if (!DA.GetData(1, ref edgeLength)) return;

            Point3d origin = basePlane.Origin;
            LocalFrame baseFrame = new LocalFrame(new Vector3((float)origin.X, (float)origin.Y, (float)origin.Z));
            BaseBox box = new BaseBox(baseFrame, (float)edgeLength.Value, (float)edgeLength.Value, (float)edgeLength.Value);

            Voxels voxels = box.voxConstruct();
            PicoGK.Mesh mesh = box.mshConstruct();

            var _voxels = new PicoGHVoxels(voxels, mesh);

            DA.SetData(0, _voxels);
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