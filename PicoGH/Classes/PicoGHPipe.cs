using System.Numerics;
using Leap71.LatticeLibrary;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHPipe : PicoGHVoxels, IModulate, IConformalArray
    {
        BasePipe _BasePipe;

        uint _LengthDivisions = 10;
        uint _RadialSteps = 20;
        uint _PolarSteps = 20;

        public PicoGHPipe(BasePipe pipe)
        {
            _BasePipe = pipe;

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public PicoGHPipe(BasePipe pipe, uint lengthDivisions, uint radialDivisions, uint heightDivisions)
        {
            _BasePipe = pipe;
            _LengthDivisions = lengthDivisions;
            _RadialSteps = radialDivisions;
            _PolarSteps = heightDivisions;

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        /// <summary>
        /// Returns a point at the specified point parameter
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector3 PointAtParameter(float p)
        {
            return _BasePipe.m_aFrames.vecGetSpineAlongLength(p);
        }
        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation innerMod;
            SurfaceModulation outerMod;

            if (modulation1._sModulation is null &  modulation2._sModulation is null)
            {
                innerMod = new SurfaceModulation(modulation1._lModulation);
                outerMod = new SurfaceModulation(modulation2._lModulation);
            }
            else
            {
                innerMod = modulation1._sModulation;
                outerMod = modulation2._sModulation;
            }

            uint lengthDivisions = 1;

            if (!(modulation1._lModulation.m_aDiscreteLengthRatios is null))
            {
                lengthDivisions = (uint)modulation1._lModulation.m_aDiscreteLengthRatios.Count;
            }

            if (!(modulation2._lModulation.m_aDiscreteLengthRatios is null))
            {
                lengthDivisions = (uint)modulation2._lModulation.m_aDiscreteLengthRatios.Count;
            }

            _LengthDivisions = lengthDivisions;
            _BasePipe.SetRadius(innerMod, outerMod, lengthDivisions);

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());

            //return output;
        }

        public PicoGHVoxels DeepCopy()
        {
            return new PicoGHPipe(_BasePipe);
        }

        public override Mesh GeneratePMesh()
        {
            _BasePipe.SetLengthSteps(_LengthDivisions);
            _BasePipe.SetRadialSteps(_RadialSteps);
            _BasePipe.SetPolarSteps(_PolarSteps);
            return _BasePipe.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BasePipe.voxConstruct();
        }

        public ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BasePipe, nx, ny, nz);
        }
    }
}
