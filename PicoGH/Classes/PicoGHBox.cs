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

using System.Numerics;
using Leap71.LatticeLibrary;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHBox : PicoGHVoxels, IModulate<PicoGHBox>, IConformalArray
    {
        BaseBox _baseBox;
        uint _lengthDivisions = 10;
        uint _widthDivisions = 10;
        uint _depthDivisions = 10;

        public PicoGHBox(BaseBox baseBox)
        {
            _baseBox = baseBox;
        }

        public PicoGHBox(BaseBox baseBox, uint lengthDivisions, uint widthDivisions, uint depthDivisions) : this(baseBox)
        {
            _lengthDivisions = lengthDivisions;
            _widthDivisions = widthDivisions;
            _depthDivisions = depthDivisions;
        }

        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            LineModulation widthMod = modulation1.LineModulation;
            LineModulation depthMod = modulation2.LineModulation;

            _baseBox.SetWidth(widthMod);
            _baseBox.SetDepth(depthMod);

            //_rMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());

            // Clear the old data to trigger a regeneration when needed.
            _rMesh = null;
            _pVoxels = null;
        }

        public Vector3 PointAtParameter(float p)
        {
            return _baseBox.m_aFrames.vecGetSpineAlongLength(p);
        }

        public override Mesh GeneratePMesh()
        {
            return _baseBox.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            _baseBox.SetDepthSteps(_depthDivisions);
            _baseBox.SetWidthSteps(_widthDivisions);
            _baseBox.SetLengthSteps(_lengthDivisions);
            return _baseBox.voxConstruct();
        }

        ConformalCellArray IConformalArray.GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_baseBox, (uint)nx, (uint)ny, (uint)nz);
        }

        public PicoGHBox DeepCopy()
        {
            PicoGHBox clone = (PicoGHBox)MemberwiseClone();

            // Reference types
            clone._baseBox = _baseBox.DeepClone();

            return clone;
        }
    }
}
