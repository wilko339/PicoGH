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

            foreach (var unitCell in _CellArray.aGetUnitCells())
            {
                _BeamThickness.UpdateCell(unitCell);
                _CellType.AddCell(ref lattice, unitCell, _BeamThickness, _subSample);
            }
            Voxels voxels = new Voxels(lattice);
            return voxels;
        }
    }
}
