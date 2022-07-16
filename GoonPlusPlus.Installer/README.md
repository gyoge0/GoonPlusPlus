# Build instructions

To build the installer, open the project in Visual Studio and build from there. The installer expects the corresponding windows binaries to have already been built and placed inside of the `GoonPlusPlus\bin\Release\win-{{platform}}\` folder. 

Building will exit with an error trying to move the installer (bug), but the installer will still have built properly. Any other errors are actual errors and should be looked into.
