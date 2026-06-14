RB Skin Forge - HTML (Web) Version
===================================

To run:  double-click  RB_Skin_Forge_launcher.bat

This starts a small local web server (no admin rights or installs needed) and
opens RB Skin Forge in your default browser. Close the launcher window to stop.

Note: the web app must be served this way - opening app\index.html directly from
disk will NOT work, because browsers block the files a Blazor app needs to load
over file:// .  The launcher handles this for you.

If port 5500 is busy, run from a terminal:  serve.ps1 -Port 5600

Folder contents:
  RB_Skin_Forge_launcher.bat  - the launcher (start here)
  serve.ps1                   - the local server it uses
  app\                        - the published web application

Creators: SpaceSquare & Claude Code | Owner: SpaceSquare
