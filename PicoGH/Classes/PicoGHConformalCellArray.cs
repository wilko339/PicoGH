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
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Leap71.LatticeLibrary;
using Rhino.Geometry;

namespace PicoGH.Classes
{
    public class PicoGHConformalCellArray : GH_GeometricGoo<Rhino.Geometry.HiddenLineDrawing>, IGH_PreviewData
    {
        public ConformalCellArray _CellArray;
        protected BoundingBox _BoundingBox;

        public PicoGHConformalCellArray() { }

        public PicoGHConformalCellArray(ConformalCellArray cellArray)
        {
            _CellArray = cellArray;
        }

        public BoundingBox ClippingBox
        {
            get
            {
                return _BoundingBox;
            }
        }

        public override BoundingBox Boundingbox
        {
            get
            {
                return _BoundingBox;
            }
        }

        public override string TypeName => throw new NotImplementedException();

        public override string TypeDescription => throw new NotImplementedException();

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {

        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            foreach (var unitCell in _CellArray.aGetUnitCells())
            {
                var cornerPoints = unitCell.aGetCornerPoints().Select(v => new Point3d(v.X, v.Y, v.Z)).ToList();
                Line[] lines =
                {
                    new Line(cornerPoints[0], cornerPoints[1]),
                    new Line(cornerPoints[1], cornerPoints[2]),
                    new Line(cornerPoints[2], cornerPoints[3]),
                    new Line(cornerPoints[3], cornerPoints[0]),
                    new Line(cornerPoints[4], cornerPoints[5]),
                    new Line(cornerPoints[5], cornerPoints[6]),
                    new Line(cornerPoints[6], cornerPoints[7]),
                    new Line(cornerPoints[7], cornerPoints[4]),
                    new Line(cornerPoints[0], cornerPoints[4]),
                    new Line(cornerPoints[1], cornerPoints[5]),
                    new Line(cornerPoints[2], cornerPoints[6]),
                    new Line(cornerPoints[3], cornerPoints[7]),
                    new Line(cornerPoints[3], cornerPoints[7])
                };
                args.Pipeline.DrawLines(lines, Color.Black);
            }
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
            return "Conformal Cell Array with " + _CellArray.aGetUnitCells().Count + " cells.";
        }

        public override IGH_GeometricGoo Transform(Transform xform)
        {
            throw new NotImplementedException();
        }
    }
}
