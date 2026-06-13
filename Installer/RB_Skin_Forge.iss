; RB Skin Forge - Windows installer script (Inno Setup 6)
; Builds RB_Skin_Forge_installer.exe from the self-contained EXE package.

#define MyAppName "RB Skin Forge"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "SpaceSquare"
#define MyAppExeName "RB_Skin_Forge.exe"
#define SourceDir "D:\Code\RB_Skin_Forge\Package_Program_EXE"

[Setup]
; A unique, fixed AppId so upgrades/uninstall are tracked correctly.
AppId={{8F2C1A34-5B6D-4E7F-9A0B-1C2D3E4F5061}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppContact=chin52696411@gmail.com
; Default install location: C:\Program Files\RB Skin Forge (user can change it in the wizard).
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=D:\Code\RB_Skin_Forge\LICENSE
OutputDir=D:\Code\RB_Skin_Forge\Installer
OutputBaseFilename=RB_Skin_Forge_installer
SetupIconFile=D:\Code\RB_Skin_Forge\Package_Program_EXE\appicon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
; Shown on the "Select Additional Tasks" page so the user can choose a desktop shortcut.
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "{#SourceDir}\*"; DestDir: "{app}"; Flags: recursesubdirs createallsubdirs ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
