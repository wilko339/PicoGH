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


namespace PicoGH.PicoGH.Classes
{
    public class PicoGHSettings
    {
        float _voxelSize;
        float _meshAdaptivity;
        bool _triangulateMeshes;
        uint _meshCoarseningFactor;

        public float VoxelSize
        { 
            get 
            { 
                return _voxelSize; 
            } 
            private set 
            { 
                _voxelSize = value; 
            }
        }

        public float MeshAdaptivity
        {
            get { return _meshAdaptivity; }
            private set { _meshAdaptivity = value; }
        }

        public bool TriangulateMeshes
        {
            get
            {
                return _triangulateMeshes;
            }

            private set
            {
                _triangulateMeshes = value;
            }
        }

        public uint MeshCoarseningFactor
        {
            get
            {
                return _meshCoarseningFactor;
            }
            
            private set
            {
                _meshCoarseningFactor = value;
            }
        }

        public PicoGHSettings()
        {
            _voxelSize = 0.5f;
            _meshAdaptivity = 0.0f;
            _triangulateMeshes = false;
            _meshCoarseningFactor = 4;
        }

        public PicoGHSettings(float voxelSize, float meshAdaptivity, bool triangulateMeshes, uint meshCoarseningFactor)
        {
            _voxelSize = voxelSize;
            _meshAdaptivity = meshAdaptivity;
            _triangulateMeshes = triangulateMeshes;
            _meshCoarseningFactor = meshCoarseningFactor;
        }
    }
}
