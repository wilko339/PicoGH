using System.Numerics;
using Leap71.LatticeLibrary;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGH.Interfaces;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHBox : PicoGHVoxels, IModulate, IConformalArray
    {
        public BaseBox _BaseBox;
        public PicoGHBox(BaseBox baseBox)
        {
            _BaseBox = baseBox;
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            _BaseBox.SetWidth(modulation1._lModulation);
            _BaseBox.SetDepth(modulation2._lModulation);

            if (!(modulation1._lModulation.m_aDiscreteLengthRatios is null))
            {
                _BaseBox.SetLengthSteps((uint)modulation1._lModulation.m_aDiscreteLengthRatios.Count);
            }

            if (!(modulation2._lModulation.m_aDiscreteLengthRatios is null))
            {
                _BaseBox.SetLengthSteps((uint)modulation2._lModulation.m_aDiscreteLengthRatios.Count);
            }

            _BaseBox.SetWidthSteps(100);
            _BaseBox.SetDepthSteps(100);
        }

        ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BaseBox,  nx, ny, nz);
        }

        public Vector3 PointAtParameter(float p)
        {
            return _BaseBox.m_aFrames.vecGetSpineAlongLength(p);
        }

        public PicoGHVoxels DeepCopy()
        {
            return new PicoGHBox(_BaseBox);
        }

        public override Mesh GeneratePMesh()
        {
            return _BaseBox.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BaseBox.voxConstruct();
        }

        ConformalCellArray IConformalArray.GenerateConformalArray(uint nx, uint ny, uint nz)
        {
            return new ConformalCellArray(_BaseBox, (uint)nx, (uint)ny, (uint)nz);
        }
    }
}
