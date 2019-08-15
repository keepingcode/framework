var
  shell = require('shelljs'),
  path = require('path')

const { execSync } = require('child_process')

//clean build
shell.rm('-rf', path.resolve(__dirname, 'dist/*'))
shell.rm('-rf', path.resolve(__dirname, 'dist/.*'))

shell.rm('-rf', path.resolve(__dirname, '../../../../../Fontes/Highway.Host/wwwroot/*'))
shell.rm('-rf', path.resolve(__dirname, '../../../../../Fontes/Highway.Host/wwwroot/.*'))

shell.rm('-rf', path.resolve(__dirname, '../../../../../Fontes/Highway.Host/bin/*'))
shell.rm('-rf', path.resolve(__dirname, '../../../../../Fontes/Highway.Host/bin/.*'))

console.log('Cleaned build artifacts.\n')

//build quasar
shell.exec("quasar build --clean")

//Copy quasar build to .net core, and build 
shell.cp('-rf', 'dist/spa/.', '../../../../../Fontes/Highway.Host/wwwroot')
shell.exec('dotnet publish -f netcoreapp2.2 ../../../../../Fontes/Highway.Host')

//Delete app settings
var dotNetBuildPath = 'Fontes/Highway.Host/bin/Debug/netcoreapp2.2/publish'

shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.Development.json'))
shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.Test.json'))
shell.rm('-f', path.join(dotNetBuildPath, 'appsettings.json'))

console.log("Build complete")