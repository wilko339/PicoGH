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
    public class PicoGHPipeSegment : PicoGHVoxels, IModulate, IConformalArray
    {
        BasePipeSegment _BasePipeSegment;

        uint _LengthDivisions = 20;
        uint _RadialSteps = 20;
        uint _PolarSteps = 20;

        public PicoGHPipeSegment(BasePipeSegment pipeSegment)
        {
            _BasePipeSegment = pipeSegment;
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public PicoGHPipeSegment(BasePipeSegment pipeSegment, uint lengthDivisions, uint radialDivisions, uint heightDivisions)
        {
            _BasePipeSegment= pipeSegment;
            _LengthDivisions = lengthDivisions;
            _RadialSteps = radialDivisions;
            _PolarSteps = heightDivisions;

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation surfMod1;
            SurfaceModulation surfMod2;

            if ((modulation1._lModulation is null) & (modulation2._lModulation is null))
            {
                surfMod1 = modulation1._sModulation;
                surfMod2 = modulation2._sModulation;
            }
            else
            {
                surfMod1 = new SurfaceModulation(modulation1._lModulation);
                surfMod2 = new SurfaceModulation(modulation2._lModulation);
            }

            uint lengthDivisions = 1;

            if (!(modulation1._lModulation.m_aDiscreteLengthRatios is null))
            {
                lengthDivisions = (uint)modulation1._lModulation.m_aDiscreteLengthRatios.Count;
            }

            if (!(modulation2._lModulation.m_aDiscreteLengthRatios is null))
            {
                lengthDivisions = (uint)modulation2._lModulation.m_aDiscreteLengthRatios.Count;
            }

            _LengthDivisions = lengthDivisions;

            _BasePipeSegment.SetRadius(surfMod1, surfMod2);

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public Vector3 PointAtParameter(float p)
        {
            return _BasePipeSegment.m_aFrames.vecGetSpineAlongLength(p);
        }

        public PicoGHVoxels DeepCopy()
        {
            return new PicoGHPipeSegment(_BasePipeSegment);
        }

        public override Mesh GeneratePMesh()
        {
            _BasePipeSegment.SetLengthSteps(_LengthDivisions);
            _BasePipeSegment.SetRadialSteps(_RadialSteps);
            _BasePipeSegment.SetLengthSteps(_LengthDivisions);
            return _BasePipeSegment.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            if (PVoxels == null)
            {
                PVoxels = _BasePipeSegment.voxConstruct();
            }
            return PVoxels;
        }

        public ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BasePipeSegment, nx, ny, nz);
        }
    }
}
