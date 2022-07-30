@echo off
set /p "name=Enter migration name: "
dotnet ef migrations add %name%
pause