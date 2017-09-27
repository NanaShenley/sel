using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;
using POM.Helper;
using OpenQA.Selenium.Interactions;
namespace MessageSetting.Components
{
    public class MessageSettingScreen
    {
        #region Page Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='message_settings_header_title']") ]
        private  IWebElement settingScreen;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private  IWebElement saveButton;

        [FindsBy(How = How.Name, Using = "EmailSetupUserCredentials.IsEmailActive")]
        private  IWebElement emailCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[title='Test Email Connection']")]
        private  IWebElement testEmailConnectionBtn;

        [FindsBy(How = How.Name, Using = "SMSSetupUserCredentials.IsSMSActive")]
        private  IWebElement smsCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[title='Test SMS Connection']")]
        private  IWebElement testSMSConnectionBtn;

        [FindsBy(How = How.Name, Using = "EmailSetupUserCredentials.EmailAccountID")]
        private IWebElement emailAddressField;

        [FindsBy(How = How.Name, Using = "EmailSetupUserCredentials.EmailPassword")]
        private IWebElement emailPwdField;

        [FindsBy(How = How.Name, Using = "TestEmail")]
        private IWebElement testEmailAddrField;

        [FindsBy(How = How.Name, Using = "TestSMS")]
        private IWebElement testSMSField;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='send_test_email_button']")]
        private IWebElement sendTestEmailBtn;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='send_test_sms_button']")]
        private IWebElement sendSMSBtn;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement cancelBtn;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement statusError;

        [FindsBy(How = How.Name, Using = "SMSSetupUserCredentials.ClientID")]
        private IWebElement smsSubIDField;

        [FindsBy(How = How.Name, Using = "SMSSetupUserCredentials.HashKey")]
        private IWebElement authCodeField;
        #endregion

        public MessageSettingScreen()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        public bool isPageDisplayed()
        {
            return settingScreen.Displayed;
        }
        public bool isSaveDisplayed()
        {
            return saveButton.Displayed;
        }
        public bool isEmailCheckBoxDisplayed()
        {
            return emailCheckBox.Displayed;
        }
        public bool isTestEmailConnectionBtnDisplayed()
        {
            return testEmailConnectionBtn.Displayed;
        }
        public bool isSMSCheckBoxDisplayed()
        {
            return smsCheckBox.Displayed;
        }
        public void saveButtonClick()
        {
            saveButton.Click();
        }
        public void clickEmailCheckBox()
        {
            if (emailCheckBox.IsChecked())
            {
                emailCheckBox.Click();
                POM.Helper.SeleniumHelper.Sleep(1);
                emailCheckBox.Click();
            }
            else
                emailCheckBox.Click();
            POM.Helper.SeleniumHelper.Sleep(3);       
        }
        public void clickSMSCheckBox()
        {
            if (smsCheckBox.IsChecked())
            {
                smsCheckBox.Click();
                POM.Helper.SeleniumHelper.Sleep(1);
                smsCheckBox.Click();
            }
            else
                smsCheckBox.Click();
            POM.Helper.SeleniumHelper.Sleep(3);

        }
        public bool configEmail(String serviceProvider, String emailID, String pwd)
        {
            try
            {
                Actions action = new Actions(WebContext.WebDriver);
                action.SendKeys(Keys.Tab).Perform();

                POM.Helper.SeleniumHelper.Sleep(1);
                action.SendKeys(Keys.Enter).Perform();
                POM.Helper.SeleniumHelper.Sleep(1);
                action.SendKeys(serviceProvider).Perform();
                POM.Helper.SeleniumHelper.Sleep(1);
                action.SendKeys(Keys.Enter).Perform();

                emailAddressField.ClearText();
                emailAddressField.SendKeys(emailID);

                emailPwdField.ClearText();
                emailPwdField.SendKeys(pwd);

                return true;

            }
            catch(Exception e)
            {
                return false;
            }

        }
        public bool testEmailConnection(string emailID)
        {
            try
            {
                if (emailCheckBox.IsChecked())
                {
                    //Follow with test email connection flow. 
                    testEmailConnectionBtn.Click();
                    POM.Helper.SeleniumHelper.Sleep(2);
                    POM.Helper.Wait.WaitForElement(By.CssSelector("[data-automation-id='send_test_email_button']"));
                    testEmailAddrField.SendKeys(emailID);

                    sendTestEmailBtn.Click();
                    POM.Helper.Wait.WaitLoading();

                    if (statusError.Displayed)
                        return false;
                    else
                        return true;
                }
                else
                    return false;               
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool configSMS(string subID, string authCode)
        {
            try
            {
                if (smsCheckBox.IsChecked() == false)
                {
                    clickSMSCheckBox();
                }
                else
                {
                    smsSubIDField.ClearText();
                    smsSubIDField.SendKeys(subID);
                    authCodeField.ClearText();
                    authCodeField.SendKeys(authCode);
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }            
        }
        public bool testSMSConnection(string number)
        {
            try
            {
                if(smsCheckBox.IsChecked())
                {
                    testSMSConnectionBtn.Click();
                    POM.Helper.SeleniumHelper.Sleep(2);

                    POM.Helper.Wait.WaitForElement(By.CssSelector("[data-automation-id='send_test_sms_button']"));
                    testSMSField.SendKeys(number);

                    sendSMSBtn.Click();
                    POM.Helper.Wait.WaitLoading();

                    if (statusError.Displayed)
                        return false;
                    else
                        return true;
                }
                else
                    return false;

            }            
            catch(Exception e)
            {
                return false;
            }    
        }
    }
}
