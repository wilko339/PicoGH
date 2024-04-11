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
using Leap71.ShapeKernel;
using PicoGK;

namespace PicoGH.PicoGH.Classes
{
    public class PicoGHImplicitSphere : PicoGHVoxels
    {
        ImplicitSphere _sphere;
        float _radius;
        Vector3 _center;

        public PicoGHImplicitSphere(ImplicitSphere sphere)
        {
            _sphere = sphere;
        }

        public override Mesh GeneratePMesh()
        {
            return _sphere.Mesh;
        }

        public override Voxels GenerateVoxels()
        {
            return _sphere.Voxels;
        }
    }
}
