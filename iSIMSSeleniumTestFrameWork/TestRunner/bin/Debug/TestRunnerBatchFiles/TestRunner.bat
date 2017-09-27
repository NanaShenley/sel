@ECHO OFF
REM ****************************************************************************************************
REM Selenium Test Runner
REM ****************************************************************************************************
IF "%~1"=="" GOTO ParamError
IF "%~2"=="" GOTO ParamError
IF "%~3"=="" GOTO ParamError
IF "%~4"=="" GOTO ParamError

SET TimingLog=%~1
SET DLLPaths=%~2
SET Reports=%~3
SET MaxThreads=%~4

ECHO TimingLog .%TimingLog%
ECHO DLLPaths .%DLLPaths%
ECHO Reports .%Reports%
ECHO MaxThreads .%MaxThreads%

REM ****************************************************************************************************
REM Assessment CNR Tests
REM ****************************************************************************************************
@echo Assessment.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Assessment\Tests\Assessment.Tests\bin\Debug\Assessment.CNR.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Assessment.CNR.CreateMarksheet.Tests"
@echo Assessment.Tests End Time %time% >> %~1\time.log

@echo Assessment.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Assessment\Tests\Assessment.CNR.OtherScreens.Tests\bin\Debug\Assessment.CNR.OtherScreens.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Assessment.CNR.OtherScreens.Tests"
@echo Assessment.Tests End Time %time% >> %~1\time.log

@echo Assessment.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Assessment\Tests\Assessment.CNR.POSMarksheet.Tests\bin\Debug\Assessment.CNR.POSMarksheet.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Assessment.CNR.POSMarksheet.Tests"
@echo Assessment.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Assessment DEA Tests
REM ****************************************************************************************************
@echo Assessment.DEA.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Assessment.DEA.Tests\bin\Debug\Assessment.DEA.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Assessment.DEA.Tests"
@echo Assessment.DEA.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Admissions Tests
REM ****************************************************************************************************
@echo Admissions.SchoolIntake.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Admissions\Tests\Admissions.SchoolIntake.Tests\bin\Debug\Admissions.SchoolIntake.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Admissions.SchoolIntake.Tests"
@echo Admissions.SchoolIntake.Tests End Time %time% >> %~1\time.log

@echo Admissions.Application.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Admissions\Tests\Admissions.Application.Tests\bin\Debug\Admissions.Application.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Admissions.Application.Tests"
@echo Admissions.Application.Tests End Time %time% >> %~1\time.log

@echo Admissions.BulkUpdate.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Admissions\Tests\Admissions.BulkUpdate.Tests\bin\Debug\Admissions.SchoolIntake.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Admissions.SchoolIntake.Tests"
@echo Admissions.BulkUpdate.Tests End Time %time% >> %~1\time.log

@echo Admissions.Enquiries.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Admissions\Tests\Admissions.Enquiries.Tests\bin\Debug\Admissions.Enquiries.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Admissions.Enquiries.Tests"
@echo Admissions.Enquiries.Tests End Time %time% >> %~1\time.log

@echo Admissions.Policy.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Admissions\Tests\Admissions.Policy.Tests\bin\Debug\Admissions.Policy.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Admissions.Policy.Tests"
@echo Admissions.Policy.Tests End Time %time% >> %~1\time.log


REM ****************************************************************************************************
REM Attendance Tests
REM ****************************************************************************************************
@echo Attendance.AttendancePattern.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.AttendancePattern.Tests\bin\Debug\Attendance.AttendancePattern.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.AttendancePattern"
@echo Attendance.AttendancePattern.Tests End Time %time% >> %~1\time.log

@echo Attendance.EditMarks.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.EditMarks.Tests\bin\Debug\Attendance.EditMarks.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.EditMarks.Tests"
@echo Attendance.EditMarks.Tests End Time %time% >> %~1\time.log

@echo Attendance.ExceptionalCircumstances.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.ExceptionalCircumstances.Tests\bin\Debug\Attendance.ExceptionalCircumstances.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.ExceptionalCircumstances.Tests"
@echo Attendance.ExceptionalCircumstances.Tests End Time %time% >> %~1\time.log

@echo Attendance.ApplyMarkOverDateRange.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.ApplyMarkOverDateRange.Tests\bin\Debug\Attendance.ApplyMarkOverDateRange.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.ApplyMarkOverDateRange.Tests"
@echo Attendance.ApplyMarkOverDateRange.Tests End Time %time% >> %~1\time.log

@echo Attendance.EarlyYearSessionPattern.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.EarlyYearSessionPattern.Tests\bin\Debug\Attendance.EarlyYearSessionPattern.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.EarlyYearSessionPattern.Tests"
@echo Attendance.EarlyYearSessionPattern.Tests End Time %time% >> %~1\time.log

@echo Attendance.DealWithSpecificMarks.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Attendance\Tests\Attendance.DealWithSpecificMarks.Tests\bin\Debug\Attendance.DealWithSpecificMarks.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Attendance.DealWithSpecificMarks.Tests"
@echo Attendance.DealWithSpecificMarks.Tests End Time %time% >> %~1\time.log


REM ****************************************************************************************************
REM Communication Tests
REM ****************************************************************************************************

@echo ManageAgents.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\ManageAgents.Test\bin\Debug\ManageAgents.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\ManageAgents.Test"
@echo ManageAgents.Test End Time %time% >> %~1\time.log
@echo Communication_OldScreens.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Communication\Communication_OldScreens.Tests\bin\Debug\Communication_OldScreens.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\Communication_OldScreens.Tests"
@echo Communication_OldScreens.Tests End Time %time% >> %~1\time.log
@echo AddressBook.CurrentPupil.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\AddressBook.Test\bin\Debug\AddressBook.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\AddressBook.Test"
@echo AddressBook.CurrentPupil.Test End Time %time% >> %~1\time.log
@echo AddressBook.CurrentPupil.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\AddressBook.CurrentPupil.Test\bin\Debug\AddressBook.CurrentPupil.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\AddressBook.CurrentPupil.Test"
@echo AddressBook.CurrentPupil.Test End Time %time% >> %~1\time.log
@echo AddressBook.CurrentStaff.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\AddressBook.CurrentStaff.Test\bin\Debug\AddressBook.CurrentStaff.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\AddressBook.CurrentStaff.Test"
@echo AddressBook.CurrentStaff.Test End Time %time% >> %~1\time.log
@echo AddressBook.PupilContact.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\AddressBook.PupilContact.Test\bin\Debug\AddressBook.PupilContact.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\AddressBook.PupilContact.Test"
@echo AddressBook.PupilContact.Test End Time %time% >> %~1\time.log
@echo ParentalReporting.Template.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Communications\ParentalReporting.Template.Test\bin\Debug\ParentalReporting.Template.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\ParentalReporting.Template.Test"
@echo ParentalReporting.Template.Test End Time %time% >> %~1\time.log
@echo MessageSetting.Test Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="C:\WIP\Teams\Team1\Dev\iSIMSSeleniumTestFrameWork\Areas\Communications\Tests\MessageSetting.Test\bin\Debug\MessageSetting.Test.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\MessageSetting.Test"
@echo MessageSetting.Test End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Statutory Return Tests
REM ****************************************************************************************************
@echo DataExchange.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\DataExchange\Tests\DataExchange.Tests\bin\Debug\DataExchange.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\DataExchange.Tests"
@echo DataExchange.Tests End Time %time% >> %~1\time.log
REM @echo Census.Tests Start Time %time% >> %~1\time.log
REM "%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\DataExchange\Tests\Census.Tests\bin\Debug\Census.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Census.Tests"
REM @echo Census.Tests End Time %time% >> %~1\time.log
REM @echo DENI.Tests Start Time %time% >> %~1\time.log
REM "%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\DataExchange\Tests\DENI.Tests\bin\Debug\DENI.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\DENI.Tests"
REM @echo DENI.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Facilities Tests
REM ****************************************************************************************************
@echo Facilities.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Facilities\Tests\Facilities.Tests\bin\Debug\Facilities.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Facilities.Tests"
@echo Facilities.Tests End Time %time% >> %~1\time.log
@echo Calendar.Tests Start Time %time% >> %~1\time.log
"%~2\TestRunner\TestRunner.exe" --dll="%~2\Calendar.Tests\Calendar.Tests.dll" --hub=http://localhost:4444/wd/hub --reporter="%~2\HtmlReport\HtmlReport.dll"  --reporter="%~2\SonarXMLReport\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Calendar.Tests"
@echo Calendar.Tests End Time %time% >> %~1\time.log
@echo Facilities.ManageKPI.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Facilities\Tests\Facilities.ManageKPI.Tests\bin\Debug\Facilities.ManageKPI.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Facilities.ManageKPI.Tests"
@echo Facilities.ManageKPI.Tests End Time %time% >> %~1\time.log
@echo SchoolSiteandBuildingTests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Facilities\Tests\SchoolSiteandBuildingTests\bin\Debug\SchoolSiteandBuildingTests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\SchoolSiteandBuildingTests.Tests"
@echo SchoolSiteandBuildingTests End Time %time% >> %~1\time.log
@echo Faclities.LogigearTests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Facilities\Tests\Faclities.LogigearTests\bin\Debug\Faclities.LogigearTests.dll" --hub=http://bedcsscmssl01:4444/wd/hub --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Faclities.LogigearTests.Tests"
@echo Faclities.LogigearTests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Pupil Tests
REM ****************************************************************************************************
@echo Pupil.BulkUpdate.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.BulkUpdate.Tests\bin\Debug\Pupil.BulkUpdate.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.BulkUpdate.Tests"
@echo Pupil.BulkUpdate.Tests End Time %time% >> %~1\time.log

@echo Pupil.ClassLog.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.ClassLog.Tests\bin\Debug\Pupil.ClassLog.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.ClassLog.Tests"
@echo Pupil.ClassLog.Tests End Time %time% >> %~1\time.log

@echo Pupil.PupilContact.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.PupilContact.Tests\bin\Debug\Pupil.PupilContacts.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.PupilContacts.Tests"
@echo Pupil.PupilContact.Tests End Time %time% >> %~1\time.log

@echo Pupil.PupilLog.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.PupilLog.Tests\bin\Debug\Pupil.PupilLog.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.PupilLog.Tests"
@echo Pupil.PupilLog.Tests End Time %time% >> %~1\time.log

@echo Pupil.PupilSENRecord.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.PupilSENRecord.Tests\bin\Debug\Pupil.PupilSENRecord.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.PupilSENRecord.Tests"
@echo Pupil.PupilSENRecord.Tests End Time %time% >> %~1\time.log

@echo Pupil.PupilSENRecord.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.PupilRecords.Tests\bin\Debug\Pupil.PupilRecords.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.PupilRecords.Tests"
@echo Pupil.PupilSENRecord.Tests End Time %time% >> %~1\time.log

@echo Pupil.PupilPremium.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.PupilPremium.Tests\bin\Debug\Pupil.PupilPremium.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"    --maxThreads=%~4  --output="%~3\Pupil.PupilPremium.Tests"
@echo Pupil.PupilPremium.Tests End Time %time% >> %~1\time.log

REM @echo Pupil.Smoke.Tests Start Time %time% >> %~1\time.log
REM "%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Pupil\Tests\Pupil.Smoke.Tests\bin\Debug\Pupil.Smoke.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\Pupil.Smoke.Tests"
REM @echo Pupil.Smoke.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Shared Services Tests
REM ****************************************************************************************************
@echo SharedServices.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\SharedServices\Tests\SharedServices.Tests\bin\Debug\SharedServices.Tests.dll" --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"   --maxThreads=%~4  --output="%~3\SharedServices.Tests"
@echo SharedServices.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Staff Tests
REM ****************************************************************************************************
REM @echo Staff.StaffRecord.Tests Start Time %time% >> %~1\time.log
REM "%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.StaffRecord.Tests\bin\Debug\Staff.StaffRecord.Tests.dll " --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Staff.StaffRecord.Tests"
REM @echo Staff.StaffRecord.Tests End Time %time% >> %~1\time.log
@echo Staff.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Staff\Tests\Staff.Tests\bin\Debug\Staff.Tests.dll " --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Staff.Tests"
@echo Staff.Tests End Time %time% >> %~1\time.log

REM ****************************************************************************************************
REM Migration Tests
REM ****************************************************************************************************
@echo Migration.Tests Start Time %time% >> %~1\time.log
"%~2\iSIMSSeleniumTestFrameWork\TestRunner\bin\Debug\TestRunner.exe" --dll="%~2\iSIMSSeleniumTestFrameWork\Areas\Migration\Tests\Migration.Tests\bin\Debug\Migration.Tests.dll " --hub=http://bedcsscmssl01:4444/wd/hub  --reporter="%~2\iSIMSSeleniumTestFrameWork\HtmlReport\bin\Debug\HtmlReport.dll"  --reporter="%~2\iSIMSSeleniumTestFrameWork\SonarXMLReport\bin\Debug\SonarXMLReport.dll"  --maxThreads=%~4  --output="%~3\Migration.Tests"
@echo Migration.Tests End Time %time% >> %~1\time.log

GOTO End

:ParamError
CALL:WriteLine "Parameter Error - Check and Run again.."
SET ERRORLEVEL=1
GOTO End

:End
CALL:WriteLine "Completed Deployment:- %~n0"
EXIT /b %ERRORLEVEL%
