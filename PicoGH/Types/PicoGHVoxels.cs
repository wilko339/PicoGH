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

        Rhino.Geometry.Mesh mesh;

        public PicoGHVoxels() { }

        public PicoGHVoxels(PicoGK.Voxels pvoxels, PicoGK.Mesh pmesh)
        {
            _pvoxels = pvoxels;
            _pmesh = pmesh;

            mesh = GenerateMesh();
        }

        public Rhino.Geometry.Mesh GenerateMesh()
        {
            int triangleCount = _pmesh.nTriangleCount();
            int vertexCount = _pmesh.nVertexCount();

            var previewMeshFaces = new Rhino.Geometry.MeshFace[triangleCount];
            var meshVertices = new Point3d[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = _pmesh.vecVertexAt(i);
                meshVertices[i] = new Point3d(vertex.X, vertex.Y, vertex.Z);
            }

            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();

            mesh.Vertices.AddVertices(meshVertices);

            for (int i = 0; i < triangleCount; i++)
            {
                var triangle = _pmesh.oTriangleAt(i);
                previewMeshFaces[i] = new MeshFace(triangle.A, triangle.B, triangle.C);
            }

            mesh.Faces.AddFaces(previewMeshFaces);
            mesh.RebuildNormals();

            return mesh;
        }

        public BoundingBox ClippingBox
        {
            get { return Boundingbox; }
        }

        public override BoundingBox Boundingbox
        {
            get { return mesh.GetBoundingBox(false); }
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
            args.Pipeline.DrawMeshShaded(mesh, args.Material);
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            args.Pipeline.DrawMeshWires(mesh, args.Color);
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
