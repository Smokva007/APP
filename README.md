# PeskovHost (Windows) - WebView2 WPF App Skeleton

This is a ready-to-open Visual Studio WPF project that wraps https://host.peskov.xyz using WebView2.
It includes native features:
- Navigation toolbar (Back, Forward, Reload, Open in default browser)
- Progress bar while loading
- Simple bookmarks saved locally (bookmarks.json)
- Window icon placeholder and app metadata

## Requirements
- Windows 10 or 11
- Visual Studio 2022 or later with .NET desktop development workload
- .NET 6.0 or later SDK installed (project targets .NET 6.0)
- Microsoft Edge WebView2 Runtime installed (https://developer.microsoft.com/en-us/microsoft-edge/webview2/#download-section)

## How to open
1. Unzip the folder.
2. Open `PeskovHost.Wpf.csproj` in Visual Studio.
3. Restore NuGet packages (Tools -> NuGet Package Manager -> Restore).
4. Build and Run (F5).

## Notes
- To produce an installer or single-file exe, use MSIX packaging or publish as single-file with dotnet publish options.
- You can customize the start URL in `MainWindow.xaml.cs` (default: https://host.peskov.xyz).


## How to enable CI build
1. Create a GitHub repo and push this folder to the `main` branch.
2. The workflow will run automatically on push; go to Actions -> build-and-create-installer to see artifacts.
3. Download the `peskovhost-installer` artifact from the workflow run.

Note: The produced installer will be unsigned. To sign it in CI, add your PFX cert as a secret and adapt the workflow.
