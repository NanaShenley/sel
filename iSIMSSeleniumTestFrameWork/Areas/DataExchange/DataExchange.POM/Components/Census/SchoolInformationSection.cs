using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.Census
{
    public class SchoolInformationSection
    {
        
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_School Information']")]
            private IWebElement _schoolInfoSection;

            [FindsBy(How = How.Name, Using = "SchoolSection.SchoolName")]
            private IWebElement _schoolName;

            [FindsBy(How = How.Name, Using = "SchoolSection.SchoolEstab")]
            private IWebElement _establishmentNumber;

         
            public SchoolInformationSection()
            {
                PageFactory.InitElements(WebContext.WebDriver, this);
            }
            public IWebElement SchoolInformation
            {
                get
                {
                    return _schoolInfoSection;
                }
            }
            public IWebElement SchoolName
            {
                get
                {
                    return _schoolName;
                }
            }
            public IWebElement EstablishmentNumber
        {
                get
                {
                    return _establishmentNumber;
                }
            }
         

            /// <summary>
            /// Check if Section Exist
            /// </summary>
            /// <returns></returns>
            public bool IsSectionExist()
            {
                return SchoolInformation.IsElementDisplayed();
            }

            /// <summary>
            /// Open On section
            /// </summary>
            public void OpenSection()
            {
                if (SchoolInformation.IsElementDisplayed())
                {
                    SchoolInformation.Click();
                }
            }

            /// <summary>
            /// This method returns true if all required Parameters exits and has default values
            /// </summary>
            /// <returns></returns>
            public bool HasRecords()
            {
                if (!(string.IsNullOrEmpty(SchoolName.GetValue())) &&
                    !(string.IsNullOrEmpty(EstablishmentNumber.GetValue())))
                {
                    return true;
                }
                else
                    return false;

            }
        }
    }
