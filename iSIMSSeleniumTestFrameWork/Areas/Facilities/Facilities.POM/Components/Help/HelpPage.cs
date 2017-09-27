using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
//using Staff.Selenium.Components.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM.Components.Help
{
    public class HelpPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("page"); }
        }

        private static string parentPage;

        private string guideTopicLink;

        public static HelpPage Create()
        {
            Wait.WaitForDocumentReady();
            parentPage = SeleniumHelper.GetParentTab();
            SeleniumHelper.SwitchToNewTab(parentPage);
            Wait.WaitForDocumentReady();
            bool exist = SeleniumHelper.DoesWebElementExist(By.Id("page"));
            return exist ? new HelpPage() : null;
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = ".AccordionPanelTab")]
        private IList<IWebElement> _quickGuideAreas;

        [FindsBy(How = How.CssSelector, Using = ".AccordionPanelContentActive a")]
        private IList<IWebElement> _quickGuideTopics;

        public string QuickGuideArea
        {
            set
            {
                foreach (var quickGuide in _quickGuideAreas)
                {
                    if (quickGuide.GetText().Trim().Equals(value))
                    {
                        if (!quickGuide.GetAttribute("class").Contains("AccordionPanelTabOpen"))
                        {
                            quickGuide.ClickUntilAppearElement(By.CssSelector(".AccordionPanelTabOpen"));
                            Wait.WaitForElementDisplayed(By.CssSelector(".AccordionPanelContentActive"));
                        }
                        break;
                    }
                }
                Refresh();
            }
        }

        public string QuickGuideTopic
        {
            set
            {
                foreach (var topic in _quickGuideTopics)
                {
                    if(topic.GetText().Trim().Equals(value))
                    {
                        guideTopicLink = topic.GetAttribute("href");
                        Retry.Do(topic.Click);
                        Wait.WaitLoading();
                        break;
                    }
                }
            }
        }
        
        #endregion

        #region Action

        public void SwitchToTopicTab()
        {
            string current = SeleniumHelper.GetParentTab();
            SeleniumHelper.SwitchToNewTab(current, parentPage);
        }

        public void CloseTopicTab()
        {
            SeleniumHelper.CloseTab();
            SeleniumHelper.SwitchToNewTab(parentPage);
            Refresh();
        }

        public bool IsTopicOpen()
        {
            string currentUrl = SeleniumHelper.GetCurrentUrl();
            return currentUrl.Contains(guideTopicLink);
        }

        public void ClosePage()
        {
            string current = SeleniumHelper.GetParentTab();
            SeleniumHelper.CloseTab();
            SeleniumHelper.SwitchToNewTab(current);
        }

        #endregion

    }
}
