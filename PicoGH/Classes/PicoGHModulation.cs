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

namespace PicoGH.Classes
{
    public class PicoGHModulation
    {
        LineModulation _lModulation;
        SurfaceModulation _sModulation;

        public LineModulation LineModulation
        {
            get
            {
                return _lModulation;
            }
        }

        public SurfaceModulation SurfaceModulation
        {
            get
            {
                return _sModulation;
            }
        }

        protected float A;
        protected float B;

        public PicoGHModulation(LineModulation modulation)
        {
            _lModulation = modulation;
        }

        public PicoGHModulation(LineModulation modulation, float a, float b)
        {
            _lModulation = modulation;
            A = a;
            B = b;
        }

        public PicoGHModulation(SurfaceModulation modulation)
        {
            _sModulation = modulation;
        }
    }
}
