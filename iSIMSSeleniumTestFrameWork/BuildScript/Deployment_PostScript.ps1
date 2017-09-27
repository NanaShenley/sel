Write-Output "Deployment post build script"

# Don't continue if anything goes wrong
# $ErrorActionPreference = "Stop"

# Get the script folder
$scriptRoot = Split-Path -Parent -Path $MyInvocation.MyCommand.Definition

# Setup folder locations
# This is our current drop location - it will be build number dependent
$CurrentBuildDropLocation = "$Env:TF_BUILD_DROPLOCATION"
# This is a constant, predictable drop location. This allows us to use the same script to deploy the latest build always, and is not build number dependent.
$DeploySourceDropLocation = Join-Path "$CurrentBuildDropLocation" "..\Latest"
$DeploySourceDropLocationRoboCopyLog = Join-Path "$DeploySourceDropLocation" "\RoboCopyLog.txt"

#****************************************************************************************************
#Copy SeleniumBuild to the SeleniumCurrentBuildDropLocation and to the SeleniumDeploySourceDropLocation
#****************************************************************************************************
#
#if(Test-Path '$DeploySourceDropLocation\SeleniumBuild') 
#{
#    Remove-Item '$DeploySourceDropLocation\SeleniumBuild' -force -recurse
#}
#    $checkError
#
#    robocopy "`"$CurrentBuildDropLocation`"","`"$DeploySourceDropLocation\SeleniumBuild`"", "/MIR", "/V", "/NP", "/X", "/LOG:$DeploySourceDropLocationRoboCopyLog", "/R:15", "/W:15"
#
#****************************************************************************************************
#Zipping Selenium Build Folder & Copying To Latest Drop Location
#****************************************************************************************************
#$Source = "$DeploySourceDropLocation\SeleniumBuild"
#$Destination = "$DeploySourceDropLocation\SeleniumBuild.zip"
#If(Test-path $Destination) {Remove-item $Destination}
#Add-Type -As System.IO.Compression.FileSystem
#[IO.Compression.ZipFile]::CreateFromDirectory( $Source, $Destination, "Optimal", $true )

# Final error check and completion message
if ($error.Count -eq 0)
{
    Write-Output "Completed Deployment post builds script with no errors" 
}
else
{
    Write-Error "Completed Deployment post builds script with $($error.Count) errors: $($error[0])"
    Exit 1
}

Exit 0
