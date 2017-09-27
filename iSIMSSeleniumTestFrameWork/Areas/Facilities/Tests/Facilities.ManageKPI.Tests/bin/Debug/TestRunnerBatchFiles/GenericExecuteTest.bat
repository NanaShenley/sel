@ECHO OFF
REM ****************************************************************************************************
REM Selenium Test Runner
REM ****************************************************************************************************
IF "%~1"=="" GOTO ParamError
IF "%~2"=="" GOTO ParamError
IF "%~3"=="" GOTO ParamError
IF "%~4"=="" GOTO ParamError
IF "%~5"=="" GOTO ParamError

SET TestRunner=%~1
SET TimingLog=%~2
SET DLLPaths=%~3
SET Reports=%~4
SET MaxThreads=%~5

ECHO.%TestRunner%
ECHO.%TimingLog%
ECHO.%DLLPaths%
ECHO.%Reports%
ECHO.%MaxThreads%

REM Example Use
REM Call "CutDownTestRunner.bat" "D:\SeleniumTimeLogs" "D:\SeleniumBuild" "D:\SeleniumReports" "4"

Call "%TestRunner%" "%TimingLog%" "%DLLPaths%" "%Reports%" "%MaxThreads%"

GOTO End

:ParamError
CALL:WriteLine "Parameter Error - Check and Run again.."
SET ERRORLEVEL=1
GOTO End

:End
CALL:WriteLine "Completed Deployment:- %~n0"
EXIT /b %ERRORLEVEL%


