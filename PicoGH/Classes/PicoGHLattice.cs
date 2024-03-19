using Leap71.LatticeLibrary;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHLattice : PicoGHVoxels
    {
        public ICellArray _CellArray;
        public ILatticeType _CellType;
        public IBeamThickness _BeamThickness;

        private uint _subSample = 2;

        public PicoGHLattice(ICellArray cellArray, float beamDiameter) 
        {
            _CellArray = cellArray;
            _CellType = new BodyCentreLattice();
            _BeamThickness = new ConstantBeamThickness(beamDiameter);

            PVoxels = GenerateVoxels();
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public override Voxels GenerateVoxels()
        {
            Lattice lattice = new Lattice();
            
            foreach (var unitCell in  _CellArray.aGetUnitCells()) 
            {
                _BeamThickness.UpdateCell(unitCell);
                _CellType.AddCell(ref lattice, unitCell, _BeamThickness, _subSample);
            }
            Voxels voxels = new Voxels(lattice);
            return voxels;
        }
    }
}
