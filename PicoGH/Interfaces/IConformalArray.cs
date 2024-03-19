using Leap71.LatticeLibrary;

namespace PicoGH.Interfaces
{
    public interface IConformalArray
    {
        ConformalCellArray GenerateConformalArray(uint nx, uint ny, uint nz);
    }
}
