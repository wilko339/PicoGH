using Leap71.ShapeKernel;

namespace PicoGH.Classes
{
    public class PicoGHModulation
    {
        public LineModulation _lModulation;
        public SurfaceModulation _sModulation;

        protected float A;
        protected float B;

        public PicoGHModulation(LineModulation modulation) 
        {
            _lModulation = modulation;
        }

        public PicoGHModulation(LineModulation modulation, float a, float b)
        {
            _lModulation = modulation;
            A = a;
            B = b;
        }

        public PicoGHModulation(SurfaceModulation modulation)
        {
            _sModulation = modulation;
        }
    }
}
