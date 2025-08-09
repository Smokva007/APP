; PeskovHostInstaller.iss
[Setup]
AppName=Peskov Host
AppVersion=1.0
DefaultDirName={pf}\PeskovHost
DefaultGroupName=PeskovHost
OutputBaseFilename=PeskovHost-Installer
Compression=lzma
SolidCompression=yes

[Files]
Source: "installer\app\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "installer\MicrosoftEdgeWebView2Setup.exe"; DestDir: "{tmp}"; Flags: ignoreversion

[Icons]
Name: "{group}\PeskovHost"; Filename: "{app}\PeskovHost.exe"
Name: "{userdesktop}\PeskovHost"; Filename: "{app}\PeskovHost.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"

[Run]
Filename: "{tmp}\MicrosoftEdgeWebView2Setup.exe"; Parameters: "/silent"; StatusMsg: "Installing WebView2 runtime..."; Flags: runhidden waituntilterminated
Filename: "{app}\PeskovHost.exe"; Description: "Run PeskovHost"; Flags: nowait postinstall skipifsilent
