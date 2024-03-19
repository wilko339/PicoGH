# PicoGH
Welcome to PicoGH. This library is a wrapper on top of Leap71's PicoGK, ShapeKernel, and LatticeLibrary repos. Under the hood is the PicoGK C++ runtime, which performs the critical voxel operations. 

This project is a work in progress. There are bugs, and things will not work as you expect them to. Please raise issues, and help us out with the development. 

## Structure
This project contains three submodule dependencies, namely the LEAP71_LatticeLibrary, LEAP71_ShapeKernel, and PicoGK. These are all forks from the originals, and have been made to work for .NET7, which is what Grasshopper components need to use. Many features have been removed from those libraries, such as anything to do with preview, since we don't need it here. I am sure more things can be removed. 

The main PicoGH stuff is within the folder named PicoGH. All the front-end Grasshopper components are located in GH, and further organised into folders depending on their function. The other folders contain all the custom data types we use to go between Grasshopper and PicoGK. Many of these are just simple wrappers of the equivalent ShapeKernel shapes, but there are some Interfaces and Utilities to help us out. 

## Classes
Inside the Classes folder in PicoGH, we find the classes that will manage all our important data. 

### PicoGHVoxels
PicoGHVoxels is the base class for all geometric shapes within PicoGH. This inherits from GH_GeometricGoo, so there are some important functions we need to implement (like bounding boxes). We implement the IGH_PreviewData interface from Grasshopper so that we can manage the previews. 

This class has three important fields:
- PMesh: the PicoGK mesh that will be used to create the voxel fields for the shape.
- PVoxels: the PicoGK voxels used for geometric operations like booleans.
- RMesh: a Rhino mesh that will be previewed in the Rhino viewports. This is equivalent to the PMesh.

Usually, this class will be subclassed into something like a PicoGHPipeSegment which takes care of generating the meshes and voxel fields as required. Sometimes, there may be a reason to directly instantiate a PicoGHVoxels object (such as when we already have our CAD mesh). In this case, the voxel field will be generated directly from the RMesh (via a PMesh). If we want to regenerate our meshes, we call the corresponding PicoGK function to convert the voxel field into a mesh. 

## Modulations
Modulations come from ShapeKernel. These are a way of modifying certain geometric parameters with functions, such as varying the radius of a pipe along the length, or around the circumferential direction. Shapes that can be modulated need to implement the IModulate interface. 

I am currently not sure what the best way to deal with these is, since we cannot give the user a way to write their own modulation function, like they would using ShapeKernel as a C# code. There are also both 2D (surface) and 1D (line) modulations to deal with. I have provided some examples of premade modulations, but I am sure there is a less black-box approach here. We cannot possibly imagine all of the possible modulations a user might wish to use. 

## Latticing
There are some wrappers for the LatticeLibrary, namely PicoGHLattice and PicoGHConformalCellArray. The conformal array uses a PicoGH shape that implements the IConformalArray interface to generate the list of unit cells. 

Aside from this, there is a component called Struts that allows the user to pass in a list of line/curve segments and a radius, and this will create a lattice object from this list of lines/curves. Since there are already so many amazing tools in Grasshopper for doing this (Intralattice, voronoi, delauney, etc), I am not sure how much of the functionality of LatticeLibrary we need to wrap. We just need to make sure the user is able to interface well with lines and curves from Grasshopper, and make it performant :rocket:.

## Implicits
There are no implicits in PicoGH for now, but these will be a huge addition so these should be wrapped soon. This library isn't a real library until it can make gyroids, right? :eyes:

## IO
This library should not replace, but complement a preexisting workflow. The user should be able to seamlessly exchange components on their canvas with an optimised PicoGH alternative that provides more reliability, speed, etc. 

The library will therefore need to operate seamlessly with the main Grasshopper types, such as meshes (quads and tris), BReps, curves, lines, points, vectors, planes, bounding boxes, and any others I missed. 

## Roadmap
As a baseline, this project should keep up with important features implemented in the PicoGK / ShapeKernel / LatticeLibrary, and should be updated as and when major functionality is added. 

There will be a GitHub project associated with this repo to track new features, issues, suggestions etc. 

Since this library is made possible thanks to a huge amount of work done by Josefine and Lin from Leap71, any extra functionality or new features discovered during the development of this library that would benefit the PicoGK / ShapeKernel / LatticeLibrary projects should be brought to their attention. 

# Current Contributors

Toby Wilkinson - https://www.linkedin.com/in/tobywilkinson339/