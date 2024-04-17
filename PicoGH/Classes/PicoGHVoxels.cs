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

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH
{
    public class PicoGHVoxels : GH_GeometricGoo<Rhino.Geometry.Mesh>, IGH_PreviewData
    {
        protected Voxels _pVoxels;
        protected PicoGK.Mesh _pMesh;
        protected Rhino.Geometry.Mesh _rMesh;
        protected Point3d _centroid; 

        public PicoGK.Voxels PVoxels
        {
            get 
            { 
                if (_pVoxels != null)
                {
                    _pVoxels.CalculateProperties(out var vol, out var _);
                    if (vol < 0.001)
                    {
                        _pVoxels = GenerateVoxels();
                    }
                }
                if (_pVoxels is null)
                {
                    _pVoxels = GenerateVoxels();
                }
                return _pVoxels; 
            }
        }
        public PicoGK.Mesh PMesh
        {
            get
            { 
                if (_pMesh is null)
                {
                    _pMesh = GeneratePMesh();
                }
                return _pMesh;
            }
        }
        public Rhino.Geometry.Mesh RMesh
        {
            get
            {
                if (_rMesh is null)
                {
                    _rMesh = Utilities.PicoMeshToRhinoMesh(PMesh);
                }
                return _rMesh;
            }
            set
            {
                _rMesh = value;
            }
        }

        // This should be implemented per shape
        public Point3d Centroid
        {
            get
            {
                return RMesh.GetBoundingBox(false).Center;
            }
        }
        public PicoGHVoxels() 
        {
            _pMesh = null;
            _rMesh = null;
            _pVoxels = null;
        }
        public PicoGHVoxels(Voxels voxels)
        {
            _pVoxels = voxels;
            _pMesh = null;
            _rMesh = null;
        }

        public PicoGHVoxels(Rhino.Geometry.Mesh mesh)
        {
            _rMesh = mesh;
            _pMesh = Utilities.RhinoMeshToPicoMesh(_rMesh);
            _pVoxels = null;
        }

        public virtual Voxels GenerateVoxels()
        {
            if (PMesh.FaceCount > 0)
            {
                _pVoxels = new Voxels(PMesh);
                return _pVoxels;
            }

            if (_rMesh.Faces.Count > 0)
            {
                _pMesh = Utilities.RhinoMeshToPicoMesh(_rMesh);
                _pVoxels = new Voxels(_pMesh);
                return (_pVoxels);
            }

            else
            {
                throw new Exception("Unable to generate voxels");
            }
        }

        public virtual PicoGK.Mesh GeneratePMesh()
        {
            // This is usually overridden by a child class, but sometimes we only have the voxel field (such as converting a mesh to voxels).
            return PVoxels.mshAsMesh();
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return RMesh == null ? new BoundingBox(): RMesh.GetBoundingBox(false);
            }
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                return RMesh.GetBoundingBox(false);
            }
        }

        public BBox3 PicoBoundingBox
        {
            get
            {
                PVoxels.CalculateProperties(out var _, out var bbox);
                return bbox;
            }
        }

        public override string TypeName
        {
            get { return "PicoGK Voxels"; }
        }

        public override string TypeDescription
        {
            get { return "PicoGK voxels wrapper"; }
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            if (RMesh != null)
            {
                args.Pipeline.DrawMeshShaded(RMesh, args.Material);
            }
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            // Uncomment to draw mesh edges for previews
            //args.Pipeline.DrawMeshWires(RMesh, args.Color);
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            return new PicoGHVoxels(GenerateVoxels());
        }

        public override BoundingBox GetBoundingBox(Transform xform)
        {
            return Boundingbox;
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            Rhino.Geometry.Mesh outputMesh = RMesh.Duplicate() as Rhino.Geometry.Mesh;
            if (!xmorph.Morph(RMesh))
            {
                throw new Exception("Unable to apply transformation.");
            }
            PicoGHVoxels transformed = new PicoGHVoxels(outputMesh);

            return transformed;
        }

        public override string ToString()
        {
            return "PicoGH Voxels Object";
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            RMesh.Transform(xform);
            _pMesh = Utilities.RhinoMeshToPicoMesh(RMesh);
            _pVoxels = new Voxels(PMesh);

            return new PicoGHVoxels(PVoxels);
        }
    }
}
