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
        private BasePipe _BasePipe;
        private uint _LengthDivisions = 10;
        private uint _RadialSteps = 20;
        private uint _PolarSteps = 20;

        public PicoGHPipe(BasePipe pipe)
        {
            _BasePipe = pipe;

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public PicoGHPipe(BasePipe pipe, uint lengthDivisions, uint radialDivisions, uint heightDivisions)
        {
            _BasePipe = pipe;
            _LengthDivisions = lengthDivisions;
            _RadialSteps = radialDivisions;
            _PolarSteps = heightDivisions;

            _rMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        /// <summary>
        /// Returns a point at the specified point parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector3 PointAtParameter(float p)
        {
            return _BasePipe.m_aFrames.vecGetSpineAlongLength(p);
        }
        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation innerMod;
            SurfaceModulation outerMod;

            if (modulation1._sModulation is null & modulation2._sModulation is null)
            {
                innerMod = new SurfaceModulation(modulation1._lModulation);
                outerMod = new SurfaceModulation(modulation2._lModulation);
            }
            else
            {
                innerMod = modulation1._sModulation;
                outerMod = modulation2._sModulation;
            }

            _BasePipe.SetRadius(innerMod, outerMod);

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());

            //return output;
        }

        public override Mesh GeneratePMesh()
        {
            _BasePipe.SetLengthSteps(_LengthDivisions);
            _BasePipe.SetRadialSteps(_RadialSteps);
            _BasePipe.SetPolarSteps(_PolarSteps);
            return _BasePipe.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BasePipe.voxConstruct();
        }

        public ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BasePipe, nx, ny, nz);
        }

        public PicoGHPipe DeepCopy()
        {
            PicoGHPipe clone = (PicoGHPipe)this.MemberwiseClone();

            // Reference types
            clone._BasePipe = _BasePipe.DeepClone();

            return clone;
        }
    }
}
