using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.Staff
{
    public class ManageClassPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get
            {
                return SimsBy.AutomationId("manage_primary_class_detail");
            }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Staff Details']")]
        private IWebElement _sectionStaffDetailLink;

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _classFullName;

        public GridComponent<ClassTeacherRow> ClassTeacherTable
        {
            get
            {
                GridComponent<ClassTeacherRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ClassTeacherRow>(By.CssSelector("[data-maintenance-container='PrimaryClassTeachers']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<StaffRow> StaffTable
        {
            get
            {
                GridComponent<StaffRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffRow>(By.CssSelector("[data-maintenance-container='PrimaryClassStaffs']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ClassTeacherRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[id $= '_Staff_dropdownImitator']")]
            private IWebElement _teacherDropdown;

            public string ClassTeachcher
            {
                set
                {
                    _teacherDropdown.EnterForDropDown(value);
                }
                get
                {
                    return _teacherDropdown.GetValue();
                }
            }

            public bool IsFormedStaffIsNotExist(string staffName)
            {
                _teacherDropdown.Click();
                List<IWebElement> list = new List<IWebElement>(SeleniumHelper.FindElements(SimsBy.CssSelector("#select2-drop ul li")));
                bool result = list.Count(x => x.GetText().Trim().Equals(staffName)) == 0;
                SeleniumHelper.FindElement(SimsBy.CssSelector("#select2-drop input.select2-input")).SendKeys(Keys.Tab);
                return result;
            }
        }

        public class StaffRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[id $= '_Staff_dropdownImitator']")]
            private IWebElement _staffDropdown;

            public string Staff
            {
                set
                {
                    _staffDropdown.EnterForDropDown(value);
                }
                get
                {
                    return _staffDropdown.GetValue();
                }
            }

            public bool IsFormedStaffIsNotExist(string staffName)
            {
                _staffDropdown.Click();
                List<IWebElement> list = new List<IWebElement>(SeleniumHelper.FindElements(SimsBy.CssSelector("#select2-drop ul li")));
                bool result = list.Count(x => x.GetText().Trim().Equals(staffName)) == 0;
                SeleniumHelper.FindElement(SimsBy.CssSelector("#select2-drop input.select2-input")).SendKeys(Keys.Tab);
                return result;
            }
        }



        public string ClassFullName
        {
            get
            {
                if (SeleniumHelper.DoesWebElementExist(_classFullName))
                {
                    return _classFullName.GetValue();
                }
                return null;
            }
        }
        #endregion
               


        #region Page action

        public void SelectSectionStaffDetail()
        {         
             _sectionStaffDetailLink.Click();              
        }

        public bool IsClassDisplaySuccess(string className)
        {
            if (ClassFullName != null)
            {
                return ClassFullName.Contains(className);
            }
            return false;
        }

        #endregion

    }
}
