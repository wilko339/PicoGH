using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using PicoGK;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Render;

namespace PicoGH
{
    public class PicoGHVoxels : GH_GeometricGoo<Rhino.Geometry.Mesh>, IGH_PreviewData
    {
        public PicoGK.Mesh _pmesh;
        public PicoGK.Voxels _pvoxels;

        public Rhino.Geometry.Mesh _rmesh;

        public PicoGHVoxels() { }

        public PicoGHVoxels(PicoGK.Voxels pvoxels, PicoGK.Mesh pmesh)
        {
            _pvoxels = pvoxels;
            _pmesh = pmesh;

            _rmesh = GenerateMesh();
        }

        public Rhino.Geometry.Mesh GenerateMesh()
        {
            return Utilities.PicoMeshToRhinoMesh(_pmesh);
        }

        public BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }

        public override BoundingBox Boundingbox
        {
            get { return _rmesh.GetBoundingBox(false); }
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
            args.Pipeline.DrawMeshShaded(_rmesh, args.Material);
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
