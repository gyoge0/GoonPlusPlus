
# Goon++

The premier text editor uesd by several people worldwide.


## Features

- Modern acryllic design
- Syntax highglighting for 35+ languages
- Instant startup
- Cross platform
## Installation

- Windows: Download and run the `.msi` file for your CPU from [the releases section](https://github.com/gyoge0/GoonPlusPlus/releases).
- MacOS: Download the binaries from [the releases section](https://github.com/gyoge0/GoonPlusPlus/releases) and run the `GoonPlusPlus` file (Native `.app` file coming soon).
- Linux: Download the binaries from [the releases section](https://github.com/gyoge0/GoonPlusPlus/releases) and run the `GoonPlusPlus` file.

## Building source

Building the source code can be donein Rider, Visual Studio, or from the dotnet cli:
```ps1
dotnet publish GoonPlusPlus\GoonPlusPlus.csproj
```

The installer should be built from Visual Studio. Simply right click the `GoonPlusPlus.Installer` project and hit build.

**IMPORTANT**

The installer expects the corresponding windows binaries to have already been built and placed inside of the `GoonPlusPlus\bin\Release\win-{{platform}}\` folder.
## Roadmap

- Create project files to restore tab position.

- Create native MacOS app.

- Create explorer tab.

- Add context menu and entry to `$PATH`.


## Tech Stack

- [.NET 6.0](https://dotnet.microsoft.com/en-us/)
- [Avalonia](https://avaloniaui.net/)
- [Avalonia Edit](https://github.com/AvaloniaUI/AvaloniaEdit)
- [WiX 3.11](https://wixtoolset.org/)
## Feedback + Questions

Email me at [gyogesh2015@outlook.com](mailto:gyogesh2015@outlook.com)!

## License
```
Copyright 2022 Yogesh Thambidurai

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
