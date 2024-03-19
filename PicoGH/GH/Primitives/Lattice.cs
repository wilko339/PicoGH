using System;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGH.Classes;

namespace PicoGH.PicoGH.Primitives
{
    public class Lattice : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Lattice class.
        /// </summary>
        public Lattice()
          : base("Lattice", "Lattice",
              "Creates a lattice",
              "PicoGH", "Lattice")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CellArray", "C", "Cell array", GH_ParamAccess.item);
            pManager.AddGenericParameter("BeamRadius", "R", "Beam radius", GH_ParamAccess.item);
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
            PicoGHConformalCellArray conformalArray = new PicoGHConformalCellArray();
            if (!DA.GetData("CellArray", ref conformalArray)) return;

            GH_Number beamRadius = new GH_Number();
            if(!DA.GetData("BeamRadius", ref beamRadius)) return;

            PicoGHLattice lattice = new PicoGHLattice(conformalArray._CellArray, (float)beamRadius.Value);

            DA.SetData(0, lattice);
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
            get { return new Guid("1E7E1B1E-B8E5-49BD-9BD0-BB9E262710AC"); }
        }
    }
}