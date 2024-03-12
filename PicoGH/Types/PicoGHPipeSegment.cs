using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Leap71.ShapeKernel;

namespace PicoGH.Types
{
    public class PicoGHPipeSegment : PicoGHVoxels
    {
        public BasePipeSegment _pipeSegment;

        public PicoGHPipeSegment(BasePipeSegment pipeSegment)
        {
            _pipeSegment = pipeSegment;
            _pvoxels = _pipeSegment.voxConstruct();
            _pmesh = _pipeSegment.mshConstruct();

            _rmesh = Utilities.PicoMeshToRhinoMesh( _pmesh );
        }
    }
}
