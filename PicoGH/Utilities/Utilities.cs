using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Leap71.ShapeKernel;
using PicoGK;
using Rhino.Geometry;
using Rhino.Render.ChangeQueue;

namespace PicoGH
{
    public class Utilities
    {
        public static Rhino.Geometry.Mesh PicoMeshToRhinoMesh(PicoGK.Mesh input)
        {
            int triangleCount = input.nTriangleCount();
            int vertexCount = input.nVertexCount();

            var previewMeshFaces = new Rhino.Geometry.MeshFace[triangleCount];
            var meshVertices = new Point3d[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                var vertex = input.vecVertexAt(i);
                meshVertices[i] = new Point3d(vertex.X, vertex.Y, vertex.Z);
            }

            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();

            mesh.Vertices.AddVertices(meshVertices);

            for (int i = 0; i < triangleCount; i++)
            {
                var triangle = input.oTriangleAt(i);
                previewMeshFaces[i] = new MeshFace(triangle.A, triangle.B, triangle.C);
            }

            mesh.Faces.AddFaces(previewMeshFaces);
            mesh.RebuildNormals();

            return mesh;
        }

        public static PicoGK.Mesh RhinoMeshToPicoMesh(Rhino.Geometry.Mesh inputMesh) 
        {
            PicoGK.Mesh pMesh = new PicoGK.Mesh();

            foreach (var vertex in inputMesh.Vertices)
            {
                pMesh.nAddVertex(new Vector3((float)vertex.X, (float)vertex.Y, (float)vertex.Z));
            }

            foreach (var meshFace in inputMesh.Faces)
            {
                // If we find a quad face, this needs to be triangulated to work with PicoGK
                if (meshFace.IsQuad)
                {
                    pMesh.nAddTriangle(new Triangle(meshFace.A, meshFace.B, meshFace.C));
                    pMesh.nAddTriangle(new Triangle(meshFace.A, meshFace.C, meshFace.D));
                }
                else
                {
                    pMesh.nAddTriangle(new Triangle(meshFace.A, meshFace.B, meshFace.C));
                }
            }

            return pMesh;
        }

        public static Frames RhinoPlanesToPicoFrames(List<Rhino.Geometry.Plane> planes)
        {
            List<Vector3> aPoints = new List<Vector3>();

            for (int i = 0; i < planes.Count; i++)
            {
                Rhino.Geometry.Plane frame = planes[i];
                aPoints.Add(new Vector3((float)frame.OriginX, (float)frame.OriginY, (float)frame.OriginZ));
            }

            Frames localFrames = new Frames(aPoints, Frames.EFrameType.MIN_ROTATION);

            return localFrames;
        }
    }
}
