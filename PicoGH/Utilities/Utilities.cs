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

using System.Collections.Generic;
using System.Numerics;
using Leap71.ShapeKernel;
using PicoGH.PicoGH.Classes;
using PicoGK;
using Rhino.Geometry;

namespace PicoGH
{
    public class Utilities
    {
        public static void SetGlobalSettings(PicoGHSettings settings)
        {
            Library.InitLibrary(settings.VoxelSize, settings.MeshAdaptivity, settings.TriangulateMeshes, settings.MeshCoarseningFactor);
        }
        public static Rhino.Geometry.Mesh PicoMeshToRhinoMesh(PicoGK.Mesh input)
        {
            int triangleCount = input.nTriangleCount();
            int quadCount = input.nQuadCount();
            int vertexCount = input.nVertexCount();

            var previewMeshFaces = new List<Rhino.Geometry.MeshFace>();
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
                previewMeshFaces.Add(new MeshFace(triangle.A, triangle.B, triangle.C));
            }

            for (int i = 0; i < quadCount; i++)
            {
                var quad = input.oQuadAt(i);
                previewMeshFaces.Add(new MeshFace(quad.A, quad.B, quad.C, quad.D));
            }

            mesh.Faces.AddFaces(previewMeshFaces);
            mesh.RebuildNormals();

            return mesh;
        }

        public static PicoGK.Mesh RhinoMeshToPicoMesh(Rhino.Geometry.Mesh inputMesh) 
        {
            PicoGK.Mesh pMesh = new PicoGK.Mesh();
            inputMesh.Vertices.CombineIdentical(true, true);

            foreach (var vertex in inputMesh.Vertices)
            {
                pMesh.nAddVertex(new Vector3((float)vertex.X, (float)vertex.Y, (float)vertex.Z));
            }

            foreach (var meshFace in inputMesh.Faces)
            {
                if (meshFace.IsQuad)
                {
                    pMesh.nAddQuad(new Quad(meshFace.A, meshFace.B, meshFace.C, meshFace.D));
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
