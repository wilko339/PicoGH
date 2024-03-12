using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGH.PicoGH.Types;

namespace PicoGH.Types
{
    public class PicoGHPipeSegment : PicoGHVoxels, IModulate
    {
        public BasePipeSegment _pipeSegment;

        public PicoGHPipeSegment(BasePipeSegment pipeSegment)
        {
            _pipeSegment = pipeSegment;
            _pvoxels = _pipeSegment.voxConstruct();
            _pmesh = _pipeSegment.mshConstruct();

            _rmesh = Utilities.PicoMeshToRhinoMesh( _pmesh );
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

            PicoGHPipeSegment output = new PicoGHPipeSegment(_pipeSegment);

            output._pipeSegment.SetRadius(surfMod1, surfMod2);
            output._pipeSegment.SetLengthSteps(100);
            output._pipeSegment.SetRadialSteps(10);
            output._pipeSegment.SetPolarSteps(10);
            
            return output;
        }
    }
}
