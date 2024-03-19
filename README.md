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
- PVoxels: PicoGK voxels used for geometric operations like booleans.
- RMesh: a Rhino mesh that will be previewed in the Rhino viewports. This is equivalent to the PMesh.

Usually, this class will be subclassed into something like a PicoGHPipeSegment which takes care of generating the meshes and voxel fields as required. Sometimes, there may be a reason to directly instantiate a PicoGHVoxels object (such as when we already have our CAD mesh). In this case, the voxel field will be generated directly from the RMesh (via a PMesh). If we want to regenerate our meshes, we call the corresponding PicoGK function to convert the voxel field into a mesh. 
