using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Pupil.Components.Common;
using SeSugar.Automation;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Pupil.Components
{
    public class PupilRecordNavigation : BaseSeleniumComponents
    {
        const string PupilRecordQuickLink = "quicklinks_top_level_pupil_submenu_pupilrecords";

        /// <summary>
        /// Navigates to PupilRecords page from Pupil Menu.
        /// </summary>
        public void NavigateToPupilRecordMenuPage()
        {
            AutomationSugar.NavigateMenu("Tasks", "Pupils", "Pupil Records");
        }

        public void NavigateToPupilRecord_SearchPupil()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID(PupilRecordQuickLink));
            Thread.Sleep(2000);
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.PupilSearchPanel.SearchButton);
        }

        public void NavigateToPupilRecord_PupilDetailPage(string studentId)
        {
            NavigateToPupilRecord_SearchPupil();
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.ReadPupilDetail, TestSettings.TestDefaults.Default.Path, studentId);
            WaitUntillAjaxRequestCompleted();
        }

        /// <summary>
        /// Load the Pupil Records
        /// </summary>
        public void NavigateToPupilRecords()
        {
            SeleniumHelper.FindAndClick(SeleniumHelper.SelectByDataAutomationID(PupilRecordQuickLink));
        }

        /// <summary>
        /// Set the Pupil search crieria panel options
        /// </summary>
        public void SetPupilSearchCriteria()
        {
            // Clear current
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.PupilSearchPanel.PupilRecordCurrentCheckBox);

            // Tick Leavers
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.PupilSearchPanel.PupilRecordLeaverCheckBox);

            // Search
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.PupilSearchPanel.SearchButton);
        }

        /// <summary>
        /// Load specified student (all types eg Current, Future, Leaver)
        /// </summary>
        /// <param name="studentId"></param>
        public void SelectSpecificPupil(string studentId)
        {
            // Get the specified student
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.ReadPupilDetail, TestSettings.TestDefaults.Default.Path, studentId);
        }

        /// <summary>
        /// Pupil Contextual action 'Record as deceased'
        /// </summary>
        public void SelectRecordAsDeceased(string studentId)
        {
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.ContextualActions.RecordAsDeceased, TestSettings.TestDefaults.Default.Path, studentId);
        }

        /// <summary>
        /// Populate the date for Deceased Pupil
        /// </summary>
        /// <param name="deceasedDate"></param>
        public void NavigateAndSetDeceasedDate(string deceasedDate)
        {
            var elementToFind = SeleniumHelper.Get(PupilElements.PupilRecord.PupilDeceased.DateOfDeath);
            elementToFind.SetText(deceasedDate);
        }

        /// <summary>
        /// Update the pupil Middle Name
        /// </summary>
        /// <param name="middleName"></param>
        public void NavigateAndSetMiddleName(string middleName)
        {
            var elementToFind = SeleniumHelper.Get(PupilElements.PupilRecord.LegalMiddleName);
            elementToFind.SetText(middleName);
        }

        /// <summary>
        /// Click the SAVE button on the Deceased Pupil panel
        /// </summary>
        public void SaveRecord()
        {
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.SaveRecord);
        }

        /// <summary>
        /// Click the SAVE button on the Deceased Pupil panel
        /// </summary>
        public void SaveDeceasedDate()
        {
            SeleniumHelper.FindAndClick(PupilElements.PupilRecord.SaveRecord);
        }

        /// <summary>
        /// Find the container for validation warnings
        /// </summary>
        /// <returns></returns>
        public bool FindSaveDeceasedValidationWarning()
        {
            var errorFound = SeleniumHelper.Get(PupilElements.PupilRecord.PupilDeceased.SaveDeceasedDateValidationWarning);
            return errorFound != null;
        }

        /// <summary>
        /// Find the container for the success message
        /// </summary>
        /// <returns></returns>
        public bool FindSaveSuccessMessage()
        {
            var content = SeleniumHelper.Get(PupilElements.PupilRecord.PupilDeceased.SaveSuccessMessage);
            return content != null;
        }

        /// <summary>
        /// Testing method to cause Selenium to wait (showing the browser) for default timeout
        /// </summary>
        public void ForceDefaultWaitState()
        {
            var dummy = SeleniumHelper.Get(By.CssSelector("NoSuchElement"));
        }

        /// <summary>
        /// Navigate to Pupil SEN DETAILS contextual action
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public SENDetails NavigateToPupilRecord_ContextualActions_SENDetail(string studentId)
        {
            NavigateToPupilRecord_PupilDetailPage(studentId);

            By by = By.CssSelector(string.Format(PupilElements.PupilRecord.ContextualActions.SENDetail, TestSettings.TestDefaults.Default.Path, studentId));
            WaitForElement(by);
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.FindAndClick(by);
            return new SENDetails();
        }

        public void NavigateAndSetQuickNote(string s)
        {
            var elementToFind = SeleniumHelper.Get(PupilElements.PupilRecord.QuickNote);
            elementToFind.SetText(s);
        }
    }
}