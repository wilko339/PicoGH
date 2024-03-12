using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap71.ShapeKernel;

namespace PicoGH.PicoGH.Types
{
    public class PicoGHModulation
    {
        public LineModulation _lModulation;
        public SurfaceModulation _sModulation;

        public PicoGHModulation(LineModulation modulation) 
        {
            _lModulation = modulation;
        }

        public PicoGHModulation(SurfaceModulation modulation)
        {
            _sModulation = modulation;
        }
    }
}
