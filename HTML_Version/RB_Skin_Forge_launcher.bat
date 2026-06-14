@echo off
title RB Skin Forge (HTML)
REM Launches the RB Skin Forge web version in your default browser.
REM No installation required - uses a small local server (Windows PowerShell).
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0serve.ps1" %*
echo.
echo Server stopped. Press any key to close.
pause >nul
