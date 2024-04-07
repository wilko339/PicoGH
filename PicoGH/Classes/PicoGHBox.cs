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
        public BaseBox _BaseBox;
        public PicoGHBox(BaseBox baseBox)
        {
            _BaseBox = baseBox;
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            _BaseBox.SetWidth(modulation1._lModulation);
            _BaseBox.SetDepth(modulation2._lModulation);

            if (!(modulation1._lModulation.m_aDiscreteLengthRatios is null))
            {
                _BaseBox.SetLengthSteps((uint)modulation1._lModulation.m_aDiscreteLengthRatios.Count);
            }

            if (!(modulation2._lModulation.m_aDiscreteLengthRatios is null))
            {
                _BaseBox.SetLengthSteps((uint)modulation2._lModulation.m_aDiscreteLengthRatios.Count);
            }

            _BaseBox.SetWidthSteps(100);
            _BaseBox.SetDepthSteps(100);
        }

        public Vector3 PointAtParameter(float p)
        {
            return _BaseBox.m_aFrames.vecGetSpineAlongLength(p);
        }

        public override Mesh GeneratePMesh()
        {
            return _BaseBox.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BaseBox.voxConstruct();
        }

        ConformalCellArray IConformalArray.GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BaseBox, (uint)nx, (uint)ny, (uint)nz);
        }

        public PicoGHBox DeepCopy()
        {
            PicoGHBox clone = (PicoGHBox)MemberwiseClone();

            // Reference types
            clone._BaseBox = _BaseBox.DeepClone();

            return clone;
        }
    }
}
