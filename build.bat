@echo off
echo Building game...
powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\build.ps1 %*
pause
