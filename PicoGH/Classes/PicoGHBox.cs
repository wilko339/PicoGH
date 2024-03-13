using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap71.ShapeKernel;
using PicoGH.Interfaces;
using PicoGH.Types;
using PicoGK;

namespace PicoGH.Classes
{
    public class PicoGHBox : PicoGHVoxels, IModulate
    {
        BaseBox _BaseBox;
        public PicoGHBox(BaseBox baseBox)
        {
            _BaseBox = baseBox;
        }

        public PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {

            PicoGHBox output = new PicoGHBox(_BaseBox);

            output._BaseBox.SetWidth(modulation1._lModulation);
            output._BaseBox.SetDepth(modulation2._lModulation);

            output._BaseBox.SetWidthSteps(100);
            output._BaseBox.SetDepthSteps(100);

            return output;
        }

        public override Mesh GeneratePMesh()
        {
            return _BaseBox.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BaseBox.voxConstruct();
        }
    }
}
