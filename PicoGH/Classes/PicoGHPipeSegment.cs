using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGK;

namespace PicoGH.Types
{
    public class PicoGHPipeSegment : PicoGHVoxels, IModulate
    {
        BasePipeSegment _BasePipeSegment;

        public PicoGHPipeSegment(BasePipeSegment pipeSegment)
        {
            _BasePipeSegment = pipeSegment;
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            SurfaceModulation surfMod1;
            SurfaceModulation surfMod2;

            if ((modulation1._lModulation is null) & (modulation2._lModulation is null))
            {
                surfMod1 = modulation1._sModulation;
                surfMod2 = modulation2._sModulation;
            }
            else
            {
                surfMod1 = new SurfaceModulation(modulation1._lModulation);
                surfMod2 = new SurfaceModulation(modulation2._lModulation);
            }

            PicoGHPipeSegment output = new PicoGHPipeSegment(_BasePipeSegment);

            output._BasePipeSegment.SetRadius(surfMod1, surfMod2);
            output._BasePipeSegment.SetLengthSteps(100);
            output._BasePipeSegment.SetRadialSteps(10);
            output._BasePipeSegment.SetPolarSteps(10);
            
            return output;
        }

        public override Mesh GeneratePMesh()
        {
            return _BasePipeSegment.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BasePipeSegment.voxConstruct();
        }
    }
}
