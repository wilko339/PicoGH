using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGH.Types;
using PicoGK;

namespace PicoGH.PicoGH.Classes
{
    public class PicoGHPipe : PicoGHVoxels, IModulate
    {
        BasePipe _BasePipe;
        public PicoGHPipe(BasePipe pipe)
        {
            _BasePipe = pipe;

            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }
        public PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            PicoGHPipe output = new PicoGHPipe(_BasePipe);

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

            output._BasePipe.SetRadius(innerMod, outerMod);
            output._BasePipe.SetLengthSteps(100);

            return output;
        }

        public override Mesh GeneratePMesh()
        {
            return _BasePipe.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BasePipe.voxConstruct();
        }
    }
}
