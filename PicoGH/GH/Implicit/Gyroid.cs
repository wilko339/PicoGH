using System;
using System.Collections.Generic;
using System.Numerics;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.LatticeLibrary;
using PicoGH.Classes;
using PicoGH.Classes.TPMS;
using Rhino.Geometry;

namespace PicoGH.GH.Implicit
{
    public class Gyroid : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PicoGHGyroid class.
        /// </summary>
        public Gyroid()
          : base("Gyroid", "Gyroid",
              "Creates a gyroid lattice inside another voxel object.",
              "PicoGH", "Implicit")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("CellSize", "S", "Unit cell size.", GH_ParamAccess.item, 5);
            pManager.AddNumberParameter("WallThickness", "T", "Wall thickness", GH_ParamAccess.item, 0.5);
            pManager.AddGenericParameter("Region", "R", "Input voxel region.", GH_ParamAccess.item);
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
            GH_Number cellSize = new GH_Number();
            if (!DA.GetData(0, ref cellSize)) return;

            GH_Number wallThickness = new GH_Number();
            if (!DA.GetData(1, ref wallThickness)) return;

            PicoGHVoxels voxelRegion = new PicoGHVoxels();
            if (!DA.GetData(2, ref voxelRegion)) return;

            PicoGHSettings settings = new PicoGHSettings();
            if (!DA.GetData("Settings", ref settings)) return;

            // Set the PicoGK library settings. 
            Utilities.SetGlobalSettings(settings);

            ImplicitSplitWallGyroid gyroid = new ImplicitSplitWallGyroid(
                (float)cellSize.Value, 
                new Vector3((float)voxelRegion.Centroid.X,
                (float)voxelRegion.Centroid.Y,
                (float)voxelRegion.Centroid.Z),
                (float)wallThickness.Value);

            PicoGHGyroid picoGHGyroid = new PicoGHGyroid(gyroid, voxelRegion);

            DA.SetData(0, picoGHGyroid);
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
            get { return new Guid("86B41653-95A8-468A-8A1A-12A894B794EF"); }
        }
    }
}