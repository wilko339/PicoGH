using System.Numerics;
using PicoGH.Classes;

namespace PicoGH.Interfaces
{
    public interface IModulate
    {
        void SetModulation(PicoGHModulation modulation1, PicoGHModulation modulation2);
        
        // This exists cause we dont wanna keep modifying the original object when we apply modulations.
        // In reality we actually do, but for expected behaviour in grasshopper, we don't...
        PicoGHVoxels DeepCopy();
        Vector3 PointAtParameter(float p);
    }
}
