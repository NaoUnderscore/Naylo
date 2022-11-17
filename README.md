![nayloclrpng](https://user-images.githubusercontent.com/60253860/202510751-7400c1ce-61d7-4066-9b24-ea5aa54c0b68.png)


# Naylo - CS2MSIL Calculator

Naylo is a calculator written in IL with the help of [Harmony](https://github.com/pardeike/Harmony), by patching all methods declared in the same assembly via transpilers.
<br>This is an assignment which I, personally, wanted to perform in this way as it was boring using raw C#.

__Disclaimer: Harmony was needed due to the assignment, Mono.Cecil wasn't allowed.__

## Troubleshooting & Known Issues
- .NET Framework 4.7.2 (Runtime) is needed in order to start this application.
- Harmony _(0Harmony.dll)_ is a project dependency and it must be placed in the same directory as the application.
- For some reason people may have issues using the release version by compiling it manually.
