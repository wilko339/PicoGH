using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGH.PicoGH.Types;

namespace PicoGH.PicoGH.Classes
{
    public class PicoGHPipe : PicoGHVoxels, IModulate
    {
        BasePipe _pipe;
        public PicoGHPipe(BasePipe pipe)
        {
            _pipe = pipe;

            _pmesh = pipe.mshConstruct();
            _pvoxels = pipe.voxConstruct();

            _rmesh = Utilities.PicoMeshToRhinoMesh(_pmesh);
        }
        public PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {
            PicoGHPipe output = new PicoGHPipe(_pipe);

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

            output._pipe.SetRadius(innerMod, outerMod);
            output._pipe.SetLengthSteps(100);

            return output;
        }
    }
}
