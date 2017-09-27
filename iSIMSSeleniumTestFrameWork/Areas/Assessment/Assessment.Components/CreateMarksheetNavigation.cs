using System;
using System.Collections.Generic;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.HomePages;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace Assessment.Components
{
    public class CreateMarksheetNavigation : BaseSeleniumComponents
    {

        public void CreateMarksheetScreenNavigation(bool takeScreenPrint)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            HomePage homepage = HomePage.NavigateTo(TestDefaults.Default.TestUser,TestDefaults.Default.TestUserPassword, TestDefaults.Default.TenantId, TestDefaults.Default.SchoolName, Configuration.GetSutUrl());
            homepage.TaskMenu().WaitFor().ClickTaskMenuBar().ClickAssessmentLink();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(By.LinkText("Assessment")));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.CreateMarksheetMenu));
            IWebElement createMarksheetLink = WebContext.WebDriver.FindElement(MarksheetConstants.CreateMarksheetMenu);
            createMarksheetLink.Click();
            if (takeScreenPrint)
                WebContext.Screenshot();
            Assert.IsNotNull(createMarksheetLink);
        }

        public void CreateMarksheetScreenNavigationUserSpecific(string UserID, string Password, bool SchoolSelection, bool takeScreenPrint )
        {
            HomePage homepage = HomePage.NavigateTo(UserID, Password, TestDefaults.Default.TenantId, TestDefaults.Default.SchoolName, Configuration.GetSutUrl(), SchoolSelection);
            homepage.TaskMenu().WaitFor().ClickTaskMenuBar().ClickAssessmentLink();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(By.LinkText("Assessment")));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.CreateMarksheetMenu));
            IWebElement createMarksheetLink = WebContext.WebDriver.FindElement(MarksheetConstants.CreateMarksheetMenu);
            createMarksheetLink.Click();
            if (takeScreenPrint)
                WebContext.Screenshot();
            Assert.IsNotNull(createMarksheetLink);
        }

        public void FindTypeOfMarksheet(bool takeScreenPrint)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.CreateMarksheetButton));
            IWebElement createMarksheetTypeLink = WebContext.WebDriver.FindElement(MarksheetConstants.CreateMarksheetButton);
            createMarksheetTypeLink.Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.NewFromExisting));
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        public void CreateMarksheetWithLevels(bool takeScreenPrint)
        {
            FindTypeOfMarksheet(false);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetWithLevels));
            WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Click();
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        //Opens up the create marksheet search panel
        public void CreateMarksheetSearch(bool takeScreenPrint)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SearchMarksheetPanelButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SearchMarksheetPanelButton).Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SearchMarksheetName));
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        //Searches Marksheet by its Name
        public string SearchByName(string MarksheetNameText, bool MarksheetActive, bool takeScreenPrint)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SearchMarksheetPanelButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SearchMarksheetPanelButton).Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SearchMarksheetName));
            WebContext.WebDriver.FindElement(MarksheetConstants.SearchMarksheetName).SendKeys(MarksheetNameText);
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.ShowMoreLink));
            WebContext.WebDriver.FindElement(MarksheetConstants.ShowMoreLink).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SearchButton));
            if (MarksheetActive == false)
            {
                waiter.Until(ExpectedConditions.ElementToBeSelected(MarksheetConstants.ActiveCheckBox));
                WebContext.WebDriver.FindElement(MarksheetConstants.ActiveCheckBox).Click();
            }

            WebContext.WebDriver.FindElement(MarksheetConstants.SearchButton).Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SearchResultText, MarksheetNameText));
            ReadOnlyCollection<IWebElement> SearchResult = WebContext.WebDriver.FindElements(MarksheetConstants.SearchResultText);

            var ResultText = "";
            try
            {
                foreach (IWebElement eachresult in SearchResult)
                {
                    if (eachresult.Text == MarksheetNameText)
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(eachresult));
                        ResultText = eachresult.Text;
                    }
                }
            }
            catch 
            {
                Console.WriteLine("No Marksheet Found with Name : {0}", MarksheetNameText);
            }

            return ResultText;
         
        }

        //Marksheet Builder for create new marksheet with levels
        public void CreateMarksheetWithLevelsPage(bool takeScreenPrint)
        {
            FindTypeOfMarksheet(false);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetWithLevels));
            WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetBuilderTitle));
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        public void ValidateMarksheetOptions(bool takeScreenPrint)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.NewFromExisting));
            Assert.AreEqual(MarksheetConstants.NewFromExistingLabel, WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExisting).GetAttribute("title"));
            WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExisting).Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.NewFromExistingPalette));
            Assert.IsNotNull(WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExistingPalette));
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        /// <summary>
        /// sets the values of name and description properties
        /// </summary>
        public void setMarksheetProperties(string name, string description)
        {
            //WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetName));
            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetDescription));
            //WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetName).SendKeys(name);
            //WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetDescription).SendKeys(description);

            IWebElement MarksheetName = ElementRetriever.GetOnceLoaded(MarksheetConstants.MarksheetName);
            MarksheetName.WaitUntilState(ElementState.Displayed);
            MarksheetName.SendKeys(name);

            IWebElement MarksheetDescription = ElementRetriever.GetOnceLoaded(MarksheetConstants.MarksheetDescription);
            MarksheetDescription.WaitUntilState(ElementState.Displayed);
            MarksheetDescription.SendKeys(description);
        }

        /// <summary>
        /// opens Aspect pallette and selects an aspect
        /// </summary>
        public void SelectAspect(string dataselectableid)
        {
            System.Threading.Thread.Sleep(1000);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SelectAssessmentButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectAssessmentButton).Click();
            WaitForAndClick(BrowserDefaults.TimeOut, MarksheetConstants.AspectPalletteSearch);
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(dataselectableid));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AddSelectedAssessmentsButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.AddSelectedAssessmentsButton).Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.OkButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.OkButton).Click();
        }

        
        /// <summary>
        /// opens Aseessment period in slider control and selects the number of Assessment Period as passed in the function
        /// </summary>
        public void AddAssessmentPeriod(int NumberOfAssessmentPeriod)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AspectNextButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.AspectNextButton).Click();

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AssessmentPeriodList));
            ReadOnlyCollection<IWebElement> APElements = WebContext.WebDriver.FindElements(MarksheetConstants.AssessmentPeriodList);

           int i = 0;

            foreach (IWebElement APElement in APElements)
            {
                if (APElement.Text != "")
                {
                    APElement.Click();
                    i++;
                }

                if (i == NumberOfAssessmentPeriod)
                    break;
            }


        }

        /// <summary>
        /// opens Subject in slider control and selects Subject as passed in the function
        /// </summary>
        public void AddSubject(String SubjectName)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SelectSubjectButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectSubjectButton).Click();

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectList));
            ReadOnlyCollection<IWebElement> SubjectElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectList);

            foreach (IWebElement SubjectElement in SubjectElements)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectList));    
                
                if (SubjectElement.Text == SubjectName)
                {
                    SubjectElement.Click();
                    break;
                }
                    
            }

        }

        /// <summary>
        /// Adds mode method purpose to the selected subjects
        /// </summary>
        public void AddMode(String Mode)
        {

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            ReadOnlyCollection<IWebElement> AspectModeElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectModeList);

            foreach (IWebElement AspectModeElement in AspectModeElements)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectModeList));

                if (AspectModeElement.Text == Mode)
                {
                    AspectModeElement.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// Adds mode method purpose to the selected subjects
        /// </summary>
        public void AddMethod(String Method)
        {

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            ReadOnlyCollection<IWebElement> AspectMethodElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectMethodList);

            foreach (IWebElement AspectMethodElement in AspectMethodElements)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectMethodList));
                if (AspectMethodElement.Text == Method)
                {
                    AspectMethodElement.Click();
                    break;
                }
            }
        }
        /// <summary>
        /// Adds mode method purpose to the selected subjects
        /// </summary>
        public void AddPurpose(String Purpose)
        {

            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            ReadOnlyCollection<IWebElement> AspectPurposeElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectPurposeList);

            foreach (IWebElement AspectPurposeElement in AspectPurposeElements)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectPurposeList));

                if (AspectPurposeElement.Text == Purpose)
                {
                    AspectPurposeElement.Click();
                    break;
                }
            }
        }
        /// <summary>
        /// opens Aseessment period in slider control and selects the number of Assessment Period as passed in the function
        /// </summary>
        public void AssignYearGroups(bool takeScreenPrint)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SelectGroupsLinks));
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectGroupsLinks).Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SelectYearGroups));
            if (takeScreenPrint)
                WebContext.Screenshot();
        }
        /// <summary>
        /// opens Aspect pallette, selects an aspect and returns the selected Aspect Name
        /// </summary>
        public string SelectNReturnAspect(string dataselectableid)
        {
            //WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SelectAssessmentButton));
            WaitUntilDisplayed(MarksheetConstants.SelectAssessmentButton);
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectAssessmentButton).Click();
            WaitForAndClick(BrowserDefaults.TimeOut, MarksheetConstants.AspectPalletteSearch);
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector(dataselectableid));
            string AspectName = WebContext.WebDriver.FindElement(By.CssSelector(dataselectableid)).Text;
            WaitUntilDisplayed(MarksheetConstants.AddSelectedAssessmentsButton);
            WebContext.WebDriver.FindElement(MarksheetConstants.AddSelectedAssessmentsButton).Click();
            WaitUntilDisplayed(MarksheetConstants.OkButton);
            WebContext.WebDriver.FindElement(MarksheetConstants.OkButton).Click();
            return AspectName;
        }

        /// <summary>
        /// opens Assessment Period pallette and selects an Assessment Period
        /// </summary>
        public void SelectAssessmentPeriod(string assessmentPeriod)
        {
            System.Threading.Thread.Sleep(1000);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(d =>
            {
                try
                {
                    d.FindElement(MarksheetConstants.SelectAssessmentPeriodButton).Click();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
                );
            //waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SelectAssessmentPeriodButton));
           // WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), MarksheetConstants.SelectAssessmentPeriodButton);
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), MarksheetConstants.AssessmentPerPalletteSearch);
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), By.LinkText(assessmentPeriod));
            WaitForAndClick(TimeSpan.FromSeconds(MarksheetConstants.Timeout), MarksheetConstants.AddSelectedAssessmentsButton);
            WebContext.WebDriver.FindElement(MarksheetConstants.OkButton).Click();
        }

        /// <summary>
        /// Add columndefinition row with specific column properties
        /// </summary>
        /// <param name="gridrowIdx"></param>
        /// <param name="aspectname"></param>
        /// <param name="assessmentPeriod"></param>
        /// <param name="colHeader"></param>
        /// <param name="isHidden"></param>
        /// <param name="isreadonly"></param>
        /// <param name="displayOrder"></param>
        /// <param name="columnWidth"></param>
        public void SetColumnDefinition(int gridrowIdx, string aspectname,string assessmentPeriod,string colHeader, bool isHidden, bool isreadonly, string displayOrder, string columnWidth)
        {
            System.Threading.Thread.Sleep(1000);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(d =>
            {
                try
                {
                    d.FindElement(MarksheetConstants.AddColumnDefinitionButton).Click();
                    bool eledisplayed = false;
                    while (eledisplayed == false)
                    {
                        eledisplayed = d.FindElements(MarksheetConstants.ColDefNewRow)[gridrowIdx].Displayed;
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
                );

            WaitUntilDisplayed(MarksheetConstants.ColDefNewRow);
            var createMarksheetDataMaintenance = SeleniumHelper.Get(General.DataMaintenance);
            System.Threading.Thread.Sleep(500);
            var colDefGrid = createMarksheetDataMaintenance.GetGridRow(MarksheetConstants.ColumnDefinitionGrid, gridrowIdx);
            System.Threading.Thread.Sleep(500);
            WaitUntilDisplayed(MarksheetConstants.AssessmentValue);
            System.Threading.Thread.Sleep(1000);
            colDefGrid.ChooseSelectorOption(MarksheetConstants.AssessmentValue, aspectname);
            System.Threading.Thread.Sleep(500);
            WaitUntilDisplayed(MarksheetConstants.AssessmentPeriod);
            colDefGrid.ChooseSelectorOption(MarksheetConstants.AssessmentPeriod, assessmentPeriod);
            System.Threading.Thread.Sleep(500);
            colDefGrid.FindElement(MarksheetConstants.Header).SetText(colHeader);
            System.Threading.Thread.Sleep(500);
            colDefGrid.SetCheckBox(MarksheetConstants.Hidden, isHidden);
            System.Threading.Thread.Sleep(500);
            colDefGrid.SetCheckBox(MarksheetConstants.Readonly, isreadonly);
            System.Threading.Thread.Sleep(500);
            WaitUntilDisplayed(MarksheetConstants.DisplayOrder);
            System.Threading.Thread.Sleep(500);
            colDefGrid.FindElement(MarksheetConstants.DisplayOrder).SetText(displayOrder);
            System.Threading.Thread.Sleep(500);
            WaitUntilDisplayed(MarksheetConstants.ColumnWidth);
            colDefGrid.ChooseSelectorOption(MarksheetConstants.ColumnWidth, columnWidth);
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Selects and Return an Additional Column Name
        /// </summary>
        /// <param name="gridrowIdx">specifies the grid row index</param>
        /// <param name="columnWidth">columnWidth value</param>
        public string SelectNReturnAdditionalColumn(int gridrowIdx, string columnWidth)
        {
            System.Threading.Thread.Sleep(1000);
            SeleniumHelper.Get(General.DataMaintenance)
                .GetGridRow(MarksheetConstants.AdditionalColumnGridRow, gridrowIdx)
               .SetCheckBox(MarksheetConstants.AdditionalColumnSelect, true)
               .ChooseSelectorOption(MarksheetConstants.AdditionalColumnWidth, columnWidth);
            System.Threading.Thread.Sleep(1000);
            string AddColName = SeleniumHelper.Get(General.DataMaintenance)
                .GetGridRow(MarksheetConstants.AdditionalColumnGridRow, gridrowIdx).FindElement(MarksheetConstants.AdditionalColumnName).GetAttribute("value");
            return AddColName;
        }

        /// <summary>
        /// Selects an Additional Column
        /// </summary>
        /// <param name="gridrowIdx">specifies the grid row index</param>
        /// <param name="columnWidth">columnWidth value</param>
        public void SelectAdditionalColumn(int gridrowIdx,string columnWidth)
        {
            SeleniumHelper.Get(General.DataMaintenance)
                .GetGridRow(MarksheetConstants.AdditionalColumnGridRow, gridrowIdx)
               .SetCheckBox(MarksheetConstants.AdditionalColumnSelect,true)
               .ChooseSelectorOption(MarksheetConstants.AdditionalColumnWidth, columnWidth);
        }


        //This will select the Type of Marksheet and allow user to create a desired marksheet
        public void SelectMarksheetType(string Marksheettype)
        {

            switch (Marksheettype)
            {
                case "New from existing":
                    WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExistingPalette).Click();
                    break;
                case "Marksheet":
                    WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Click();
                    break;
                case "Programme of Study Tracking":
                    WebContext.WebDriver.FindElement(MarksheetConstants.ProgrammeOfStudyTracking).Click();
                    break;
                case "Tracking Grid":
                    WebContext.WebDriver.FindElement(MarksheetConstants.TrackingGrid).Click();
                    break;
            }
        }

        //Method to verify Save Marksheet Assertion Success Case
        public void SaveMarksheetAssertionSuccess()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            try
            {
                waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.MarksheetSaveMessageCSS));
                string actualText = WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetSaveMessageCSS).Text;
                Assert.AreEqual(MarksheetConstants.MarksheetSaveMessage, actualText);
                Console.WriteLine(actualText);
            }
            catch 
            {
                waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SaveError));
                string actualText = WebContext.WebDriver.FindElement(MarksheetConstants.SaveError).Text;
                Assert.AreEqual(MarksheetConstants.SaveErrorMessage, actualText);
                var ValidationMessageText = WebContext.WebDriver.FindElement(MarksheetConstants.ValidationError).Text;
                Console.WriteLine("The following value/values seem to be missing on UI : {0}", ValidationMessageText);
            }
            
        }

        //Method to verify Save Marksheet Assertion Failure Case
        public void SaveMarksheetAssertionFailure()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            try
            {
                waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SaveError));
                string actualText = WebContext.WebDriver.FindElement(MarksheetConstants.SaveError).Text;
                Assert.AreEqual(MarksheetConstants.SaveErrorMessage, actualText);
                var ValidationMessageText = WebContext.WebDriver.FindElement(MarksheetConstants.ValidationError).Text;
                Console.WriteLine("The following value\values seem to be missing on UI : {0}", ValidationMessageText);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        /// <summary>
        /// Select all the Year Groups
        /// </summary>
        public void checkAllYearGroups()
        {
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroups))
            {
                if (!e.Selected)
                    e.Click();
            }
        }

        /// <summary>
        /// Select all the Classes
        /// </summary>
        public void checkAllClasses()
        {
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectClasses))
            {
                if (!e.Selected)
                    e.Click();
            }
        }

        /// <summary>
        /// Clear all the Classes
        /// </summary>
        public void clearAllClasses()
        {
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectClasses))
            {
                if (e.Selected)
                    e.Click();
            }
        }

        /// <summary>
        /// Returns the list of selected year groups
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectedYearGroups()
        {
            List<string> label = new List<string>();
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroups))
            {
                if (e.Selected)
                {
                    var id = e.GetAttribute("id");
                    label.Add(SeleniumHelper.Get(General.DataMaintenance).FindElement(By.CssSelector("label[for=\"" + id + "\"")).Text);
                }
            }
            return label;
        }

        public void SetSelection(string ownrname)
        {
            System.Threading.Thread.Sleep(1000);
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            System.Threading.Thread.Sleep(500);
            var showMore = WebContext.WebDriver.FindElement(MarksheetConstants.ShowMore);
            showMore.Click();
            WaitUntilDisplayed(MarksheetConstants.OwnerValue);
            System.Threading.Thread.Sleep(1000);

            var ownerdropdown = SeleniumHelper.Get(By.Name("Owner"));
            ownerdropdown.ChooseSelectorOption(MarksheetConstants.OwnerValue, ownrname);
            //SeleniumHelper.ChooseSelectorOption(MarksheetConstants.OwnerValue, ownrname);
            System.Threading.Thread.Sleep(500);




           
           
        }

        /// <summary>
        /// Select all the GroupFilter
        /// </summary>
        public void CheckGroupFilter()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectEthnicity))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectLanguage))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSchoolIntake))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectClassesFilter))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroupsFilter))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenNeedType))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectUserDefined))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectTeachingGroup))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenStatus))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    e.Click();
            }
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                {
                    e.Click();
                    break;
                }
            }
        }
    }





    public class ColumnDefPro
    {
        public int gridrowIdx { get; set; }
        public string aspectname { get; set; }
        public string assessmentPeriod { get; set; }
        public string colHeader { get; set; }
        public bool isHidden { get; set; }
        public bool isreadonly { get; set; }
        public string displayOrder { get; set; }
        public string columnWidth { get; set; }

    }

    public class CreateMarksheetProperties 
    {
        public string userid { get; set; }

 
    }
}
