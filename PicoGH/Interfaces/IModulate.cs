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
using PicoGH.Classes;

namespace PicoGH.Interfaces
{
    public interface IModulate
    {
        void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2);

        // This exists cause we dont wanna keep modifying the original object when we apply modulations.
        // In reality we actually do, but for expected behaviour in grasshopper, we don't...
        PicoGHVoxels DeepCopy();
        Vector3 PointAtParameter(float p);
    }
}
