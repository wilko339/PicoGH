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

using Leap71.LatticeLibrary;
using PicoGK;

namespace PicoGH.Classes.TPMS
{
    public class PicoGHGyroid : PicoGHVoxels
    {
        ImplicitSplitWallGyroid _gyroid;
        BBox3 _bbox;
        PicoGHVoxels _voxelRegion;

        /// <summary>
        /// Constructor for generating gyroid within a region
        /// </summary>
        /// <param name="gyroid"></param>
        /// <param name="region"></param>
        public PicoGHGyroid(ImplicitSplitWallGyroid gyroid, BBox3 region)
        {
            _gyroid = gyroid;
            _bbox = region;
        }

        /// <summary>
        /// Constructor to generate lattice within another PicoGH object
        /// </summary>
        /// <param name="gyroid"></param>
        /// <param name="intersectionRegion"></param>
        public PicoGHGyroid(ImplicitSplitWallGyroid gyroid, PicoGHVoxels intersectionRegion)
        {
            _gyroid = gyroid;
            _voxelRegion = intersectionRegion;
        }

        public override Voxels GenerateVoxels()
        {
            if (_voxelRegion == null)
            {
                return new Voxels(_gyroid, _bbox);
            }
            else
            {
                return new Voxels(_gyroid, _voxelRegion.PicoBoundingBox, _voxelRegion.PVoxels);
            }
        }

        public override Mesh GeneratePMesh()
        {
            return null;
        }
    }
}
