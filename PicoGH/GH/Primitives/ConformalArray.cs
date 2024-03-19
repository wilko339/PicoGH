using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGH.Classes;
using PicoGH.Interfaces;
using Leap71.LatticeLibrary;

namespace PicoGH.Primitives
{
    public class ConformalArray : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConformalArray class.
        /// </summary>
        public ConformalArray()
          : base("ConformalArray", "ConformalArray",
              "Creates a conformal cell array",
              "PicoGH", "Cell Arrays")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Guide", "G", "Shape to guide conformal generation.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("CellsX", "NX", "Cell count in X.", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("CellsY", "NY", "Cell count in Y.", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("CellsZ", "NZ", "Cell count in Z.", GH_ParamAccess.item, 5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Array", "A", "Conformal cell array", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IConformalArray inputData = null;
            if (!DA.GetData(0, ref inputData)) return;

            if (!(inputData is IConformalArray)) return;

            GH_Number nx = new GH_Number();
            GH_Number ny = new GH_Number();
            GH_Number nz = new GH_Number();
            if (!DA.GetData("CellsX", ref nx)) return;
            if (!DA.GetData("CellsY", ref ny)) return;
            if (!DA.GetData("CellsZ", ref nz)) return;

            ConformalCellArray conformalCellArray = inputData.GenerateConformalArray((uint)nx.Value, (uint)ny.Value, (uint)nz.Value);

            PicoGHConformalCellArray cellArray = new PicoGHConformalCellArray(conformalCellArray);

            DA.SetData(0, cellArray);
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
            get { return new Guid("26EE053B-3B70-491A-A169-7BB9F97EF510"); }
        }
    }
}