using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace PicoGH
{
    public class PicoGHInfo : GH_AssemblyInfo
    {
        public override string Name => "PicoGH";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("e7ba07d8-402e-4f2e-8389-d0248838eb46");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}