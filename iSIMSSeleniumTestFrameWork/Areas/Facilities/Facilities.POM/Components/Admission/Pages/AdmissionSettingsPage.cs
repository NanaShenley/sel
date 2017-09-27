using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POM.Components.Admission
{
    public class AdmissionSettingsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "LastAdmissionNumber")]
        private IWebElement _adminssionNumberTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public string LastAdmissionNumber
        {
            set { _adminssionNumberTextbox.SetText(value); }
            get { return _adminssionNumberTextbox.GetValue(); }
        }

        private static int _beforeAdmission;

        public int BeforeAdmissionNumber
        {
            get { return _beforeAdmission; }
        }

        public int AdmissionNumber
        {
            get
            {
                string currentAdmission = LastAdmissionNumber;
                //Split number and text
                var match = Regex.Match(currentAdmission, "(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
                string text = match.Groups["Alpha"].Value;
                string number = match.Groups["Numeric"].Value;
                return int.Parse(number);
            }
        }

        #endregion

        #region Action

        public void Save()
        {
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsSuccessMessageDisplay()
        {
            return _successMessage.IsElementDisplayed();
        }

        public string GetNextAvailableAdmissionNumber()
        {
            string currentAdmission = LastAdmissionNumber;

            //Split number and text
            var match = Regex.Match(currentAdmission, "(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
            string text = match.Groups["Alpha"].Value;
            string number = match.Groups["Numeric"].Value;

            //Try increment the number
            int num = int.Parse(number);
            num++;
            if (num.ToString().Length > number.Length)
            {
                //Length of new number longer than current number
                //Try create new text
                char[] textChars = text.ToCharArray();
                for (int i = textChars.Length - 1; i >= 0; i--)
                {
                    int newChar = textChars[i] + 1;
                    if (newChar >= 'A' && newChar <= 'Z')
                    {
                        textChars[i] = (char)newChar;
                        text = new string(textChars);
                        break;
                    }

                    if (i == 0)
                    {
                        text += "A";
                    }
                }

                //Reset number
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 6 - text.Length - 1; i++)
                {
                    builder.Append(0);
                }
                number = builder.ToString();
            }
            else
            {
                number = num.ToString();
            }

            string availableAdmission = String.Format("{0}{1}", text, number);
            while (availableAdmission.Length < 6)
            {
                availableAdmission = String.Format("{0}{1}", 0, availableAdmission);
            }
            return availableAdmission;
        }

        public void RecordAdmissionNumber()
        {
            _beforeAdmission = AdmissionNumber;
        }

        #endregion
    }
}
