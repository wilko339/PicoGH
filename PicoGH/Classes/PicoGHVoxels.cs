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

        private Rhino.Geometry.Mesh _RMesh;

        public Rhino.Geometry.Mesh RMesh
        {  get
            {
                if (_RMesh == null)
                {
                    _RMesh = Utilities.PicoMeshToRhinoMesh(PMesh);
                }
                return _RMesh;
            }
           set
            {
                _RMesh = value;
            }
        }

        public PicoGHVoxels() { }
        public PicoGHVoxels(PicoGK.Voxels voxels)
        {
            PVoxels = voxels;
        }

        public virtual Voxels GenerateVoxels()
        {
            throw new NotImplementedException("Child must override this method.");
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
                return new BoundingBox(-100, -100, -100, 100, 100, 100);
                if (_RMesh == null)
                {
                    _RMesh = Utilities.PicoMeshToRhinoMesh(PMesh);
                }
                return _RMesh?.GetBoundingBox(false) ?? BoundingBox.Empty;
            }
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                return new BoundingBox(-100, -100, -100, 100, 100, 100);

                if (_RMesh == null)
                {
                    _RMesh = Utilities.PicoMeshToRhinoMesh(PMesh);
                }
                return _RMesh?.GetBoundingBox(false) ?? BoundingBox.Empty;
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
            
        }

        public override IGH_GeometricGoo DuplicateGeometry()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
