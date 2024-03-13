using PicoGK;
using Leap71.ShapeKernel;

namespace PicoGH.Classes
{
    public class PicoGHSphere : PicoGHVoxels
    {
        BaseSphere _BaseSphere;

        public PicoGHSphere(BaseSphere baseSphere)
        {
            _BaseSphere = baseSphere;
            RMesh = Utilities.PicoMeshToRhinoMesh(GeneratePMesh());
        }

        public override Mesh GeneratePMesh()
        {
            return _BaseSphere.mshConstruct();
        }

        public override Voxels GenerateVoxels()
        {
            return _BaseSphere.voxConstruct();
        }
    }
}
