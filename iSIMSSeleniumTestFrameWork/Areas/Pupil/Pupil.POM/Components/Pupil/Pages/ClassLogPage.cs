using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SeSugar.Automation;
using WebDriverRunner.webdriver;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil.Pages
{
    public class ClassLogPage : BaseComponent
    {
        public ClassLogPage()
        {
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForDocumentReady();
        }

        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='clog-classname-dropdown-description']")]
        private IWebElement _className;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio'] [data-automation-id='Button_DropdownRadio_Description']")]
        private IWebElement _defaultClassSpan { get; set; }

        public string ClassName()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='clog-classname-dropdown-description']"));
            return _className.GetAttribute("title");
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='gallery_tile_list_item']")]
        private IList<IWebElement> listOfGalleryItem { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[data-ajax-url $= 'General']")]
        private IWebElement _generalNoteMenuItem;

        private const string _pupilXpath = "//*/ul[@data-automation-id='gallery_main_list_panel']/li";

        private By _firstPupilSelector = SimsBy.Xpath(_pupilXpath + "[1]");

        private By _firstSelectedPupilSelector = SimsBy.Xpath(_pupilXpath + "[1 and contains(@class, 'selected')]");

        public string SelectedLearnerId
        {
            get
            {
                By selectedLearnerHidden = SimsBy.Id("hidselectedLearnersdata");

                var element = ElementRetriever.FindElementSafe(WebContext.WebDriver, selectedLearnerHidden);

                return element.GetValue();
            }
        }

        public string DefaultClass()
        {
            return _defaultClassSpan.Text;
        }

        public bool HasPupilsLoaded()
        {
            return listOfGalleryItem.Any();
        }

        public List<string> SelectFristTwoPupils()
        {
            IWebElement galleryContainer = GetGalleryItems();

            ReadOnlyCollection<IWebElement> galleryItems = galleryContainer.FindElements(By.CssSelector("[data-automation-id='gallery_tile_list_item'"));

            List<string> pupilnames = new List<string>();

            int counter = 0;
            foreach (var galleryItem in galleryItems)
            {
                if (counter >= 2)
                {
                    break;
                }
                counter++;
                galleryItem.Click();

                string forname;
                string surname;

                forname = galleryItem.GetAttribute("data-forname");
                surname = galleryItem.GetAttribute("data-surname");

                pupilnames.Add(String.Format("{0}, {1}", forname, surname));

            }

            return pupilnames;
        }


        public void SelectAddNoteItem()
        {
            IWebElement dropdownContainer = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='clog-toolbar']"));
            IWebElement dropdownItem = dropdownContainer.FindElement(By.CssSelector("[data-automation-id='Button_Dropdown']"));
            dropdownItem.Click();
            IWebElement noteItem = dropdownContainer.FindElement(By.CssSelector("[data-automation-id='clog-gen-contextlink']"));
            noteItem.Click();
        }


        public void SelectPupil()
        {
            SeleniumHelper.WaitUntilElementIsDisplayed(_pupilXpath + "[1]");

            ElementRetriever.FindElementSafe(WebContext.WebDriver, _firstPupilSelector).Click();

            Wait.WaitUntilDisplayed(_firstSelectedPupilSelector);
        }

        public QuickAddAchievementDialog OpenAchievementPopup()
        {
            By achievementLinkSelector = SimsBy.Xpath("//*/a[@data-class-log-conduct-event='achievementlink']");

            ElementRetriever.FindElementSafe(WebContext.WebDriver, achievementLinkSelector).Click();
            AutomationSugar.WaitForAjaxCompletion();

            // Arbitrary but assume that if the title is displayed then the dialog has loaded?
            Wait.WaitForElementDisplayed(SimsBy.AutomationId("record_achievement_popup_header_title"));

            return new QuickAddAchievementDialog();
        }

        public QuickAddBehaviourDialog OpenBehaviourPopup()
        {
            By behaviourLinkSelector = SimsBy.Xpath("//*/a[@data-class-log-conduct-event='behaviourlink']");

            ElementRetriever.FindElementSafe(WebContext.WebDriver, behaviourLinkSelector).Click();
            AutomationSugar.WaitForAjaxCompletion();

            // Arbitrary but assume that if the title is displayed then the dialog has loaded?
            Wait.WaitForElementDisplayed(SimsBy.AutomationId("record_behaviour_popup_header_title"));

            return new QuickAddBehaviourDialog();
        }

        #region private methods 

        private static IWebElement GetGalleryItems()
        {
            return WebContext.WebDriver.FindElement(By.CssSelector(".gallery-list"));
        }

        #endregion

        public NoteDialog SelelectGeneralNoteType()
        {
            //_generalNoteMenuItem.ClickByJS();
            //Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new NoteDialog();
        }

        public ClogPupilLogTimeLine TimeLine
        {

            get { return new ClogPupilLogTimeLine(); }
        }
    }


    public class ClogPupilLogTimeLine
    {
        private IWebElement _timeLineElement;
        private IWebElement _timeLine;

        public ClogPupilLogTimeLine()
        {
            _timeLineElement = WebContext.WebDriver.FindElement(By.CssSelector("[data-section-id='timeline-container']"));
            //_timeLineElement = _webElement;
        }

        public Note this[string noteName]
        {
            get { return GetNote(noteName); }
        }

        public Note GetNote(string noteName)
        {
            var _notes = _timeLineElement.FindElements(SimsBy.CssSelector(".event"));
            foreach (var note in _notes)
            {
                var name = note.FindElement(SimsBy.AutomationId("log-event-heading")).GetText();
                if (name.Trim().Contains(noteName))
                {
                    return new Note(note);
                }
            }

            return null;
        }

        public List<Note> GetNotes()
        {
            List<Note> lstNote = new List<Note>();
            var _notes = _timeLineElement.FindElements(SimsBy.CssSelector(".event"));
            foreach (var noteElement in _notes)
            {
                var note = new Note(noteElement);
                lstNote.Add(note);
            }
            return lstNote;
        }

        public void Delete()
        {
            _timeLineElement.FindElement(SimsBy.AutomationId("delete_button")).ClickByJS();
            SeleniumHelper.FindElement(SimsBy.AutomationId("Yes_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));

        }
    }
}
