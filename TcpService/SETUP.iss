;Change below section between versions
#define appVer "4.0"
#define MyAppExeName "IPeople.Connect.Echo.dll"
#define MySetupAppExeName "IPeople.Connect"
#define OutputDirectory "\\buildserver\Release_Writable\Setups\IPeople.Connect\"
#define MyInputFileName "\\buildserver\Release\Assemblies\IPeople.Connect\" + MyAppExename
#define OutputNameProductVersion GetStringFileInfo(MyInputFileName,"FileVersion")
#define OutputNameFileVersion GetFileVersion(MyInputFileName)
#define MyOutputName MySetupAppExeName + "_" + OutputNameProductVersion
#define appIcon "IPeopleLogo-IconOnly-ApricotLogo.ico"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{DEB7346E-A6F4-42ED-A8D7-1D6B124F1145}
AppName=IPeople.Connect
AppVersion={#OutputNameProductVersion}
AppVerName=IPeople.Connect.4.0
AppPublisher=Interface People, LP
AppPublisherURL=http://www.ipeople.com/
AppSupportURL=http://www.ipeople.com/
AppUpdatesURL=http://www.ipeople.com/
DefaultDirName=C:\IPEOPLE\Connect
DefaultGroupName=IPeople
DisableProgramGroupPage=yes
OutputDir={#OutputDirectory}
OutputBaseFilename={#MyOutputName}
SetupIconFile={#appIcon}
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
Uninstallable=FALSE
 
[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "\\buildserver\Release\Assemblies\IPeople.Connect\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

