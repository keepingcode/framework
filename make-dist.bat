@rem
@rem Build release binaries for this project
@rem

dotnet publish -c Release -r win10-x64 -f net462         --output "%cd%\dist\bin\win10-x64\net462"
dotnet publish -c Release -r win10-x64 -f netcoreapp2.2  --output "%cd%\dist\bin\win10-x64\netcoreapp2.2"
dotnet publish -c Release -r linux-x64 -f netcoreapp2.2  --output "%cd%\dist\bin\linux-x64\netcoreapp2.2"

pause
