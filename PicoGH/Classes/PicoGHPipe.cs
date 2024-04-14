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
using Grasshopper.Kernel;
using Leap71.LatticeLibrary;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGK;
using Rhino;

namespace PicoGH.Classes
{
    public class PicoGHPipe : PicoGHVoxels, IModulate<PicoGHPipe>, IConformalArray
    {
        BasePipe _basePipe;
        uint _lengthDivisions = 10;
        uint _radialSteps = 20;
        uint _polarSteps = 20;

        public PicoGHPipe(BasePipe pipe)
        {
            _basePipe = pipe;
        }

        public PicoGHPipe(BasePipe pipe, uint lengthDivisions, uint radialDivisions, uint heightDivisions)
        {
            _basePipe = pipe;
            _lengthDivisions = lengthDivisions;
            _radialSteps = radialDivisions;
            _polarSteps = heightDivisions;
        }

        /// <summary>
        /// Returns a point at the specified point parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector3 PointAtParameter(float p)
        {
            return _basePipe.m_aFrames.vecGetSpineAlongLength(p);
        }
        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation innerMod;
            SurfaceModulation outerMod;

            if (modulation1.SurfaceModulation is null & modulation2.SurfaceModulation is null)
            {
                innerMod = new SurfaceModulation(modulation1.LineModulation);
                outerMod = new SurfaceModulation(modulation2.LineModulation);
            }
            else
            {
                innerMod = modulation1.SurfaceModulation;
                outerMod = modulation2.SurfaceModulation;
            }

            _basePipe.SetRadius(innerMod, outerMod);

            // Clear the old data to trigger a regeneration when needed.
            _rMesh = null;
            _pVoxels = null;
        }

        public override Mesh GeneratePMesh()
        {
            _basePipe.SetLengthSteps(_lengthDivisions);
            _basePipe.SetRadialSteps(_radialSteps);
            _basePipe.SetPolarSteps(_polarSteps);
            return _basePipe.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _basePipe.voxConstruct();
        }

        public ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_basePipe, nx, ny, nz);
        }

        public PicoGHPipe DeepCopy()
        {
            PicoGHPipe clone = (PicoGHPipe)this.MemberwiseClone();

            // Reference types
            clone._basePipe = _basePipe.DeepClone();

            return clone;
        }
    }
}
