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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicoGH.Classes.TPMS;
using PicoGH.GH.Implicit;
using PicoGK;

namespace PicoGH.PicoGH.Classes.TPMS
{
    public class PicoGHConformalGyroid : PicoGHVoxels
    {
        BaseConformalGyroid _baseConformalGyroid;
        BBox3 _bbox;
        PicoGHVoxels _voxelRegion;

        public PicoGHConformalGyroid(BaseConformalGyroid baseConformalGyroid, BBox3 region)
        {
            _baseConformalGyroid = baseConformalGyroid;
            _bbox = region;
        }

        public PicoGHConformalGyroid(BaseConformalGyroid baseConformalGyroid, PicoGHVoxels intersectionRegion)
        {
            _baseConformalGyroid = baseConformalGyroid;
            _voxelRegion = intersectionRegion;
            _bbox = intersectionRegion.PicoBoundingBox;
        }

        public override Voxels GenerateVoxels()
        {
            if ( _voxelRegion == null )
            {
                return new Voxels(_baseConformalGyroid, _bbox);
            }
            else
            {
                return new Voxels(_baseConformalGyroid, _bbox, _voxelRegion.PVoxels);
            }
        }
    }


}
