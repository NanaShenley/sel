java -Dwebdriver.ie.driver=C:\WIP\iSIMSSeleniumTestFrameWork\packages\Selenium.WebDriver.IEDriver.3.6.0\driver\IEDriverServer.exe -Dwebdriver.chrome.driver=C:\WIP\iSIMSSeleniumTestFrameWork\packages\Selenium.WebDriver.ChromeDriver.2.32.0.0\driver\chromedriver.exe -Dwebdriver.gecko.driver=C:\WIP\iSIMSSeleniumTestFrameWork\packages\Selenium.WebDriver.GeckoDriver.Win64.0.19.0\driver\geckodriver.exe -jar selenium-server-standalone-3.5.3.jar -port 5555 -role node -debug -hub http://localhost:4444/grid/register -browser "browserName=firefox,maxInstances=20,platform=WINDOWS,seleniumProtocol=WebDriver" -browser "browserName=internet explorer,version=11,platform=WINDOWS,maxInstances=20" -browser "browserName=chrome,version=ANY,maxInstances=20,platform=WINDOWS"
