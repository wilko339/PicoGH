using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PicoGH.Types;

namespace PicoGH.Interfaces
{
    public interface IModulate
    {
        PicoGHVoxels SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2);
    }
}
