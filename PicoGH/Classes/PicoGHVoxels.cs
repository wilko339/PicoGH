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
        public PicoGK.Mesh PMesh;
        public PicoGK.Voxels PVoxels;
        public Rhino.Geometry.Mesh RMesh;

        public PicoGHVoxels() { }
        public PicoGHVoxels(PicoGK.Voxels voxels)
        {
            PVoxels = voxels;
            PMesh = GeneratePMesh();
            RMesh = Utilities.PicoMeshToRhinoMesh(PMesh);
        }

        public virtual Voxels GenerateVoxels()
        {
            if (PVoxels == null)
            {
                throw new NotImplementedException("Child must override this method.");
            }
            return PVoxels;
        }

        public virtual PicoGK.Mesh GeneratePMesh()
        {
            // This is usually overridden by a child class, but sometimes we only have the voxel field (such as converting a mesh to voxels).
            if (PMesh == null)
            {
                return PVoxels.mshAsMesh();
            }
            return PMesh;
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return RMesh.GetBoundingBox(false);
            }
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                return RMesh.GetBoundingBox(false);
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
            args.Pipeline.DrawMeshShaded(RMesh, args.Material);
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
            throw new NotImplementedException();
        }

        public override IGH_GeometricGoo Morph(SpaceMorph xmorph)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "PicoGH Voxels Object";
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            RMesh.Transform(xform);
            PMesh = Utilities.RhinoMeshToPicoMesh(RMesh);
            PVoxels = new Voxels(PMesh);

            return new PicoGHVoxels(PVoxels);
        }
    }
}
