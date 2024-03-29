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
using PicoGH.Classes;

namespace PicoGH.Interfaces
{
    public interface IModulate<TSelf> where TSelf : IModulate<TSelf>
    {
        void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2);

        /// <summary>
        /// Performs a deep copy of the underlying data.
        /// This is necessary because in Grasshopper we expect the output of an operation to be a new object. We dont want to modify the previous object, as this might cause problems if it is connected to other modifiers. 
        /// </summary>
        /// <returns></returns>
        TSelf DeepCopy();

        Vector3 PointAtParameter(float p);
    }
}
