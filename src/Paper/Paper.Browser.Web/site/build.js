var
  shell = require('shelljs'),
  path = require('path')

const { execSync } = require('child_process')

//clean build
shell.rm('-rf', path.resolve(__dirname, 'dist/*'))
shell.rm('-rf', path.resolve(__dirname, 'dist/.*'))

shell.rm('-rf', path.resolve(__dirname, '../../../../../Vortex.Core.Host/wwwroot/*'))
shell.rm('-rf', path.resolve(__dirname, '../../../../../Vortex.Core.Host/wwwroot/.*'))

shell.rm('-rf', path.resolve(__dirname, '../../../../../Vortex.Core.Host/bin/*'))
shell.rm('-rf', path.resolve(__dirname, '../../../../../Vortex.Core.Host/bin/.*'))

console.log('Cleaned build artifacts.\n')

//build quasar
shell.exec("quasar build --clean")

//Copy quasar build to .net core, and build 
shell.cp('-rf', 'dist/spa/.', '../../../../../Vortex.Core.Host/wwwroot')
shell.exec('dotnet publish -f netcoreapp2.2 ../../../../../Vortex.Core.Host')

//Delete app settings
var dotNetBuildPath = 'Vortex.Core.Host/bin/Debug/netcoreapp2.2/publish'

shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.Development.json'))
shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.Test.json'))
shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.json'))

console.log("Build complete")