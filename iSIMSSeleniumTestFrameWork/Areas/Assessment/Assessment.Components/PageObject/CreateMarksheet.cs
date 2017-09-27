using Assessment.Components.Common;
using Assessment.Components.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using System;
using System.Linq;
using System.Threading;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class CreateMarksheet : MarksheetType
    {
        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] input[name='MarksheetTemplateName']")]
        public IWebElement MarksheetTemplateName;

        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] textarea[name='Description']")]
        public IWebElement MarksheetTemplateDescription;

        [FindsBy(How = How.CssSelector, Using = "div[id='marksheetcreation-wrapper'] textarea[name='keyWords']")]
        public IWebElement MarksheetTemplateKeyWords;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next_add_columns']")]
        public IWebElement Next_Add_Columns_Button;

        //private static By Pupil_Information_Path = By.CssSelector("button[data-automation-id='Pupil_Information']");
        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Pupil_Information']")]
        private IWebElement Pupil_Information;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='Predefined_Button']")]
        private IWebElement Predefined;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ad_hoc']")]
        private IWebElement AdHoc;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='pupil_details_done']")]
        private IWebElement Pupil_Details_Done;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='preview_marksheet']")]
        private IWebElement Marksheet_Preview;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='close_button']")]
        private IWebElement Marksheet_Preview_Ok;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='savetemplate']")]
        private IWebElement Marksheet_Save_Button;

        
        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='template_marksheet_filter']")]
        private IWebElement Create_Marksheet;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='back-button-save']")]
        private IWebElement goback_Marksheet;

        [FindsBy(How = How.CssSelector, Using = "a[data-cancel-button]")]
        private IWebElement CancelTemplateButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Button_Dropdown']")]
        private IWebElement CreateMarksheetButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='marksheet_with_level_modify']")]
        private IWebElement modifyMarksheet;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='marksheet_with_level_copy']")]
        private IWebElement copyMarksheet;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        public CreateMarksheet()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public CreateMarksheet FillTemplateDetails(string TemplateName, string Description,string Keywords="")
        {
            MarksheetTemplateName.WaitUntilState(ElementState.Displayed);
            MarksheetTemplateName.Clear();
            MarksheetTemplateName.SendKeys(TemplateName);
            MarksheetTemplateDescription.Clear();
            MarksheetTemplateDescription.SendKeys(Description);

            MarksheetTemplateKeyWords.Clear();
            MarksheetTemplateKeyWords.SendKeys(Keywords);

            waiter.Until(ExpectedConditions.ElementToBeClickable(Next_Add_Columns_Button));
            Next_Add_Columns_Button.Click();
            return this;
        }


        public CreateMarksheet NextButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Next_Add_Columns_Button));
            Next_Add_Columns_Button.Click();
            return this;
        }

        public CreateMarksheet PupilInformationClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Pupil_Information));
            Pupil_Information.Click();
            return this;
        }

        public CreateMarksheet PupilDetailsDoneClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Pupil_Details_Done));
            Pupil_Details_Done.Click();
            return this;
        }

        public String getMarksheetTemplateName()
        {
            MarksheetTemplateName.WaitUntilState(ElementState.Displayed);
            String ExistingMarksheetTemplateName = MarksheetTemplateName.GetValue();
            return ExistingMarksheetTemplateName;
        }

        public String getMarksheetTemplateDescription()
        {
            MarksheetTemplateDescription.WaitUntilState(ElementState.Displayed);
            String ExistingMarksheetTemplateDescription = MarksheetTemplateDescription.GetValue();
            return ExistingMarksheetTemplateDescription;
        }

        public String getMarksheetTemplateKeywords()
        {
            MarksheetTemplateKeyWords.WaitUntilState(ElementState.Displayed);
            String ExistingMarksheetTemplateKeywords = MarksheetTemplateKeyWords.GetValue();
            return ExistingMarksheetTemplateKeywords;
        }

        public bool IsCreateMarksheetButtonVisibleAfterSavingTemplate()
        {
            Create_Marksheet.WaitUntilState(ElementState.Displayed);
            return Create_Marksheet.Displayed;
        }

        public CreateMarksheet AddPupilInformation()
        {
            Pupil_Information.Click();
            PupilDetails pupilDetails = new PupilDetails();
            pupilDetails.SelectPupilDetailColumnCheckBox("Gender");
            pupilDetails.SelectPupilDetailColumnCheckBox("Class");
            pupilDetails.SelectPupilDetailColumnCheckBox("Term of Birth");
            waiter.Until(ExpectedConditions.ElementToBeClickable(Pupil_Details_Done));
            Pupil_Details_Done.Click();
            return this;
        }

        public CreateMarksheet AddPredefinedAssessment()
        {
            Predefined.Click();
            PredefinedAssessment predefinedAspects = new PredefinedAssessment();
            predefinedAspects.SerachPredefinedAspects();
            WaitUntillAjaxRequestCompleted();
            predefinedAspects.SelectPredefinedAspects();
            WaitUntillAjaxRequestCompleted();
            predefinedAspects.SelectHowOftenYouAccess();
            predefinedAspects.ClickDone();
            return this;
        }

        //public CreateMarksheet AddPredefinedStatutoryAssessment()
        //{
        //    Predefined.Click();
        //    PredefinedAssessment predefinedAspects = new PredefinedAssessment();
        //    predefinedAspects.SerachPredefinedStatutoryAspects();
        //    WaitUntillAjaxRequestCompleted();
        //    predefinedAspects.SelectPredefinedAspects();
        //    predefinedAspects.ClickDone();
        //    return this;
        //}

        public CreateMarksheet AddAdHoc()
        {
            AdHoc.Click();
            AdHocColumn adHocColumn = new AdHocColumn();
            AddRandomAdHocColumns(adHocColumn);
            adHocColumn.AddColumn_Done();
            return this;
        }

        public CreateMarksheet AddAdHocWithAssessment()
        {
            AdHoc.Click();
            AdHocColumn adHocColumn = new AdHocColumn();
            AddRandomAdHocColumns(adHocColumn);
            adHocColumn.AdHoc_Column_LinkPeriod_Click();

            PredefinedAssessment predefinedAssessment = new PredefinedAssessment();
            predefinedAssessment.SelectHowOftenYouAccess();
            predefinedAssessment.ClickDone();

            return this;
        }

        private void AddRandomAdHocColumns(AdHocColumn adHocColumn)
        {
            adHocColumn.AddColumn(RandomString(5), ColumnType.Text);
            adHocColumn.AddColumn(RandomString(5), ColumnType.Numeric);
            adHocColumn.AddColumn(RandomString(5), ColumnType.Text);
            adHocColumn.AddColumn(RandomString(5), ColumnType.Numeric);
        }

        public CreateMarksheet Preview()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Marksheet_Preview));
            Thread.Sleep(1000);
            Marksheet_Preview.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(Marksheet_Preview_Ok));
            Marksheet_Preview_Ok.Click();
            return this;
        }

        public CreateMarksheet SaveTemplate()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Marksheet_Save_Button));
            Marksheet_Save_Button.Click();
            return this;
        }

        public AssignTemplateGroupAndFilter Marksheetcreate()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(Create_Marksheet));
            Create_Marksheet.Click();
            return new AssignTemplateGroupAndFilter();
        }

        public CreateMarksheet goBack()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(goback_Marksheet));
            goback_Marksheet.Click();
            return this;
        }

        public CreateMarksheet editColumn()
        {
            Thread.Sleep(1000);
            IWebElement columnlist = WebContext.WebDriver.FindElements(By.ClassName("fa-pencil")).FirstOrDefault();
            columnlist.Click();
            PredefinedAssessment obj = new PredefinedAssessment();
            obj.SelectHowOftenYouAccess();
            obj.ClickDone();
            return this;
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public CreateMarksheet CancelTemplate()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CancelTemplateButton));
            CancelTemplateButton.Click();
            return new CreateMarksheet();
        }

        public CreateMarksheet CreateTemplate()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateMarksheetButton));
            CreateMarksheetButton.Click();
            return new CreateMarksheet();
        }

        public EditMarksheetTemplate ClickModifyExistingButton()
        {
            modifyMarksheet.Click();
            return new EditMarksheetTemplate();
        }

        public EditMarksheetTemplate ClickCopyFromExistingButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(copyMarksheet));
            copyMarksheet.Click();
            return new EditMarksheetTemplate();
        }
    }
}