// Copyright 2024 Toby Wilkinson
//
//  Licensed under the Apache License, Version 2.0 (the "License")
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0 
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the License for the specific language governing permissions and 
//  limitations under the License.

using System;
using System.Drawing;
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
        public override string AuthorName => "Toby Wilkinson";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}