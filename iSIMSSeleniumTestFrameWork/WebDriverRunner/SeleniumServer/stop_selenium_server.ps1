
Clear-Host
$Site = 'http://localhost:4444/selenium-server/driver/?cmd=shutDownSeleniumServer'
$Request = Invoke-WebRequest -URI $Site
