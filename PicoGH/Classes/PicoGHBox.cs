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
    public class PicoGHBox : PicoGHVoxels, IModulate
    {
        BaseBox _basebox;

        public PicoGHBox(BaseBox basebox)
        {
            _basebox = basebox;
            _pmesh = _basebox.mshConstruct();
            _pvoxels = _basebox.voxConstruct();

            _rmesh = Utilities.PicoMeshToRhinoMesh(_pmesh);
        }

        public PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2)
        {

            PicoGHBox output = new PicoGHBox(_basebox);

            output._basebox.SetWidth(modulation1._lModulation);
            output._basebox.SetDepth(modulation2._lModulation);

            output._basebox.SetWidthSteps(100);
            output._basebox.SetDepthSteps(100);

            return output;
        }
    }
}
