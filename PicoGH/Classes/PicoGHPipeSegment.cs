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
using Grasshopper.Kernel.Special;
using Leap71.LatticeLibrary;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHPipeSegment : PicoGHVoxels, IModulate<PicoGHPipeSegment>, IConformalArray
    {
        BasePipeSegment _basePipeSegment;

        uint _lengthDivisions;
        uint _radialSteps;
        uint _polarSteps;

        public PicoGHPipeSegment(BasePipeSegment pipeSegment, uint lengthDivisions, uint radialDivisions, uint polarDivisions)
        {
            _basePipeSegment = pipeSegment;

            _lengthDivisions = lengthDivisions;
            _radialSteps = radialDivisions;
            _polarSteps = polarDivisions;
        }

        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation surfMod1;
            SurfaceModulation surfMod2;

            if ((modulation1.LineModulation is null) & (modulation2.LineModulation is null))
            {
                surfMod1 = modulation1.SurfaceModulation;
                surfMod2 = modulation2.SurfaceModulation;
            }
            else
            {
                surfMod1 = new SurfaceModulation(modulation1.LineModulation);
                surfMod2 = new SurfaceModulation(modulation2.LineModulation);
            }

            _basePipeSegment.SetRadius(surfMod1, surfMod2);

            // Clear the old data to trigger a regeneration when needed.
            _rMesh = null;
            _pVoxels = null;
        }

        public Vector3 PointAtParameter(float p)
        {
            return _basePipeSegment.m_aFrames.vecGetSpineAlongLength(p);
        }

        public override Mesh GeneratePMesh()
        {
            _basePipeSegment.SetLengthSteps(_lengthDivisions);
            _basePipeSegment.SetRadialSteps(_radialSteps);
            _basePipeSegment.SetPolarSteps(_polarSteps);
            var mesh = _basePipeSegment.mshConstruct();

            return mesh;
        }

        public override Voxels GenerateVoxels()
        {
            if (_pVoxels == null)
            {
                _pVoxels = _basePipeSegment.voxConstruct();
            }
            return _pVoxels;
        }

        public ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_basePipeSegment, nx, ny, nz);
        }

        public PicoGHPipeSegment DeepCopy()
        {
            PicoGHPipeSegment clone = (PicoGHPipeSegment)MemberwiseClone();

            // Reference types
            clone._basePipeSegment = _basePipeSegment.DeepClone();

            return clone;
        }
    }
}
