param($installPath, $toolsPath, $package, $project)

if (Get-Module | ?{ $_.Name -eq 'GeneratorModule' })
{
    Remove-Module GeneratorModule
}

Import-Module (Join-Path $toolsPath GeneratorModule.psd1)