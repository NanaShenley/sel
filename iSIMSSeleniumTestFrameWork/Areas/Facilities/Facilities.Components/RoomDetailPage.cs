using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Facilities.Components
{
     

    public class RoomDetailPage :BaseSeleniumComponents
    {
#pragma warning disable 0649
     public RoomDetailPage()
        {       
            // wait for the page to be fully loaded
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            waiter.Until<Boolean>(d =>
            {
               IWebElement el = WebContext.WebDriver.FindElement(By.Id("html"));
               string classes = el.GetAttribute("class");
                if (classes.Contains("nprogress-busy")){
                    return false;
                }
                return true;
            });
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

          private IWebElement ClickonRoomButton()
          {
              return WebContext.WebDriver.FindElement(By.CssSelector("[title='Create the Record']") );
          }


          public void ClickRoomButton()
          {
              IWebElement Button = ClickonRoomButton();
              Button.Click(); //click create button
              waitForTelNotoAppear(); //wait for short name control
          }

          public void waitForTelNotoAppear()
          {
              WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(4));
              wait1.Until<Boolean>( d =>
                  {
                      IWebElement TelNo = WebContext.WebDriver.FindElement(By.Name("TelephoneNumber"));
                      if (TelNo.Displayed)
                      {
                          return true;
                      }
                      return false;
                  }
              );

          }
    
          public void EnterShortName(string name)

          {

              var shortname = WebContext.WebDriver.FindElement(By.CssSelector("#editableData [name='ShortName']"));
              shortname.SendKeys(name);
          }

          public void EnterName(string name)
          {

              var longName = WebContext.WebDriver.FindElement(By.CssSelector("#editableData [name='LongName']"));

              longName.SendKeys(name);
          }
   
        public void EnterRoomOtherDetails()
          {
              //CODE TO EXPAND SITE DROPDOWN AND SELECT THE SITE.
              var ExpStDrp = WebContext.WebDriver.FindElements(By.ClassName("select2-arrow"));
              ExpStDrp[1].Click();
              WebContext.Screenshot();
              var SelectSite = WebContext.WebDriver.FindElements(By.ClassName("select2-result-label"));
              SelectSite[0].Click();
              WebContext.Screenshot();
        
              //CODE TO EXPAND BUILDING DROPDOWN AND SELECT THE BUILDING.
              waitforappearBuildingName();
              var ExpbldngDrp = WebContext.WebDriver.FindElements(By.ClassName("select2-arrow"));

              ExpbldngDrp[3].Click();
              WebContext.Screenshot();
              var SelectBldng = WebContext.WebDriver.FindElements(By.ClassName("select2-result-label"));
              SelectBldng[0].Click();
              WebContext.Screenshot();
            //ENTER TELEPHONE NUMBER
              var enterTelNo = WebContext.WebDriver.FindElement(By.Name("TelephoneNumber"));
              enterTelNo.SendKeys("9527725936");
            //Enter Area
              var enterArea = WebContext.WebDriver.FindElement(By.Name("Area"));
              enterArea.SendKeys("10000000");
              //Enter Area
              var enterMaxgrpsize = WebContext.WebDriver.FindElement(By.Name("MaximumGroupSize"));
              enterMaxgrpsize.SendKeys("10000");

            //CLICK ON SELECT MAIN USER FOR THE ROOM.
              var selectMainUser = WebContext.WebDriver.FindElement(By.CssSelector("button[title = 'Select User']"));
              selectMainUser.Click();
             // WebDriverWait wait2 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(5));
              waitForSurname();
            //ASSOCIATE STAFF WITH THE ROOM USER(BY DEFAULT STAFF IS SELECTED)
              var clickSearchButton = WebContext.WebDriver.FindElements(By.CssSelector("button[type='submit']"));
              clickSearchButton[1].Click();
              Thread.Sleep(2000);
              waitforSearchResult();
            //SELECT SEARCHED STAFF RECORD
              var selectStaff = WebContext.WebDriver.FindElement(By.CssSelector("a.search-result.h1-result"));
              selectStaff.Click();
              WebContext.Screenshot();
            //CLICK ON OK BUTTON.
              var clickOKBtn = WebContext.WebDriver.FindElement(By.XPath("//div[3]/button"));
              clickOKBtn.Click();
            //WAIT TO CLEAR BUTTON TO ENABLE
              waitforClearBtntoEnaable();

            //CLICK ON SAVE BUTTON.
              var clickSavebutton = WebContext.WebDriver.FindElement(By.CssSelector("[title='Save Record']"));
              clickSavebutton.Click();
              waitforSavemessagetoAppear();
              WebContext.Screenshot();
          }


        //METHODE TO HANDLE WAIT TO LOAD
        public void waitForSurname()//WAIT TO LOAD THE POP-UP PAGE(FORENEAME TO APPEAR)
        {
            WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(5));
            wait1.Until<Boolean>(d =>
            {
                IWebElement foreName = WebContext.WebDriver.FindElement(By.Name("Surname"));
                if (foreName.Displayed)
                {
                    return true;
                }
                return false;
            }
            );

        }

        public void waitforappearBuildingName()
        {
            WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            wait1.Until<Boolean>(d =>
            {
                IWebElement BldngName = WebContext.WebDriver.FindElement(By.CssSelector("input[name='Building.dropdownImitator']"));
                if (BldngName.Enabled)
                {
                    return true;
                }
                return false;
            }
            );

        }

        public void waitforSearchResult()
        {
            WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(5));
            wait1.Until<Boolean>(d =>
            {
                IWebElement foreName = WebContext.WebDriver.FindElement(By.Name("Surname"));
                if (foreName.Displayed)
                {
                    return true;
                }
                return false;
            }
            );

        }

        public void waitforClearBtntoEnaable()
        {
            WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            wait1.Until<Boolean>(d =>
            {
                IWebElement clrBtntoEnable = WebContext.WebDriver.FindElement(By.CssSelector("button[title = 'Clear']"));
                if (clrBtntoEnable.Enabled)
                {
                    return true;
                }
                return false;
            }
            );
        }
        public void waitforSavemessagetoAppear()
        {
            WebDriverWait wait1 = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));
            wait1.Until<Boolean>(d =>
            {
                IWebElement saveMessage = WebContext.WebDriver.FindElement(By.CssSelector("[class='inline-alert-title']"));
                if (saveMessage.Displayed)
                {
                    return true;
                }
                return false;
            }
            );
        }

    }
}