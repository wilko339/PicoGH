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

using Leap71.ShapeKernel;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHSphere : PicoGHVoxels
    {
        BaseSphere _baseSphere;

        public PicoGHSphere(BaseSphere baseSphere)
        {
            _baseSphere = baseSphere;
        }

        public override Mesh GeneratePMesh()
        {
            return _baseSphere.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _baseSphere.voxConstruct();
        }
    }
}
