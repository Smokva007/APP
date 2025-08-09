#define MyAppPublishDir "D:\a\APP\APP\app\publish"

[Setup]
AppName=PeskovHost
AppVersion=1.0
DefaultDirName={pf}\PeskovHost
DefaultGroupName=PeskovHost
OutputDir=output
OutputBaseFilename=PeskovHostInstaller
Compression=lzma
SolidCompression=yes

[Files]
Source: "{#MyAppPublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\PeskovHost"; Filename: "{app}\PeskovHost.exe"
Name: "{commondesktop}\PeskovHost"; Filename: "{app}\PeskovHost.exe"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"

[Run]
Filename: "{tmp}\MicrosoftEdgeWebView2Setup.exe"; Parameters: "/silent"; StatusMsg: "Installing WebView2 runtime..."; Flags: runhidden waituntilterminated
Filename: "{app}\PeskovHost.exe"; Description: "Run PeskovHost"; Flags: nowait postinstall skipifsilent
