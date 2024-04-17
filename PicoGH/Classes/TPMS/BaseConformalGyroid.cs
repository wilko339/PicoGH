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
using System.Numerics;
using Leap71.LatticeLibrary;
using Rhino.Geometry;

namespace PicoGH.Classes.TPMS
{
    public class BaseConformalGyroid : ImplicitSplitWallGyroid
    {
        Surface _baseSurface;
        float _wOffset;
        public BaseConformalGyroid(Surface baseSurface, float wOffset, float fUnitSize, Vector3 vCentre, float fWallThickness) :
            base (fUnitSize, vCentre, fWallThickness)
        {
            _baseSurface = baseSurface;
            _wOffset = wOffset;
        }

        public override float fSignedDistance(in Vector3 vecPt)
        {
            float fFinalDist = float.MaxValue;
            double normalisedDistToSurf;

            Point3d globalPoint = new Point3d(vecPt.X, vecPt.Y, vecPt.Z);
            Point3d surfacePoint;
            double u;
            double v;

            if (_baseSurface.ClosestPoint(globalPoint, out u, out v))
            {
                surfacePoint = _baseSurface.PointAt(u, v);
                if (surfacePoint != Point3d.Unset)
                {
                    normalisedDistToSurf = (globalPoint - surfacePoint).Length;

                    //calculate the gyroid surface equation
                    double dDist =      Math.Sin(m_fFrequencyScale * u) * Math.Cos(m_fFrequencyScale * v) +
                                        Math.Sin(m_fFrequencyScale * v) * Math.Cos(m_fFrequencyScale * normalisedDistToSurf) +
                                        Math.Sin(m_fFrequencyScale * normalisedDistToSurf) * Math.Cos(m_fFrequencyScale * u);

                    float absDist = (float)Math.Abs(dDist);

                    fFinalDist =    (float)Math.Min(
                                        (float)Math.Max(dDist, absDist - 0.5f * m_fWallThickness),
                                        (float)Math.Max(-dDist, absDist - 0.5f * m_fWallThickness));

                    return fFinalDist;
                }
            }
            return fFinalDist;
        }
    }
}
