using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using POM.Base;
using POM.Components.HomePages;
using SeSugar.Automation;
using TestSettings;
using WebDriverRunner.webdriver;
using Environment = SeSugar.Environment;

namespace POM.Helper
{
    public static class SeleniumHelper //ElementAccessor
    {


        [DllImport("kernel32.dll")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        // ReSharper disable once InconsistentNaming
        public struct SYSTEMTIME
        {
            public ushort WYear;
            public ushort WMonth;
            public ushort WDayOfWeek;
            public ushort WDay;
            public ushort WHour;
            public ushort WMinute;
            public ushort WSecond;
            public ushort WMilliseconds;
        }

        public static SYSTEMTIME GetSystemTime()
        {
            // Call the native GetSystemTime method 
            // with the defined structure.
            SYSTEMTIME stime = new SYSTEMTIME();
            GetSystemTime(ref stime);
            return stime;
        }

        public static void SetSystemTimeFromNow(SYSTEMTIME systime)
        {
            // Call the native GetSystemTime method 
            // with the defined structure.            
            SetSystemTime(ref systime);
        }

        public static void SetSystemTimeFromNow(int day = 0)
        {
            // Call the native GetSystemTime method 
            // with the defined structure.
            SYSTEMTIME systime = new SYSTEMTIME();
            GetSystemTime(ref systime);

            // Set the system clock ahead one hour.
            systime.WHour = (ushort) (systime.WDay + day);
            SetSystemTime(ref systime);
        }



        public static void AddDateFromNowByCommandLine(int day = 0)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            startInfo.FileName = "cmd.exe";
            //string date
            //startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";            
            var dateFuture = DateTime.Now.AddDays(day);

            string commandLine = "date " + dateFuture.ToString(sysFormat);
            startInfo.Arguments = "/C " + commandLine;
            process.StartInfo = startInfo;
            process.Start();
        }


        public const string AutomationIdAttributeFormat = "[data-automation-id='{0}']";

        public static string AutomationId(string value)
        {
            return String.Format(AutomationIdAttributeFormat, value);
        }

        public static By SelectByDataAutomationId(string idToFind)
        {
            return By.CssSelector(AutomationId(idToFind));
        }

        public static IWebElement ScrollToElement(this IWebElement element, By elementToScrollTo,
            string containerWhichNeedsToScrollCss)
        {
            var scrollTo = element.FindElement(elementToScrollTo);
            var h = scrollTo.Location.Y + scrollTo.Size.Height;

            WebContext.WebDriver.Url = string.Format("javascript:$('{0}').scrollTop({1},0);",
                containerWhichNeedsToScrollCss, h);
            return element;
        }

        public static IWebElement GoToAccordionPanel(this IWebElement element, string title)
        {
            var accordion = element.FindElementsSafe(By.CssSelector(".accordion-default [data-accordian-panel]"));

            foreach (var accordionSection in accordion)
            {
                var header = accordionSection.FindChild(By.CssSelector(".panel-heading"));
                if (header.Text.Trim().Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    var h = header.Location.Y + header.Size.Height;
                    WebContext.WebDriver.Url =
                        "javascript:$('#screen > div > div.layout-col.main.pane > div > div > div > div.form-body > div > div > div > div.layout-col.form.pane > div > div.form-body > div').scrollTop(" +
                        h + ",0);";

                    header.Click();

                    break;
                }
            }

            return element;
        }

        public static IWebElement ValueChangeTriggerAction(this IWebElement element,
            Func<IWebElement, IWebElement> action, string updateSectionId)
        {
            var updateSection = element.FindChild(By.CssSelector("[data-section-id=\"" + updateSectionId + "\"]"));

            var sender = GetWaiterForAriaBusyState(updateSection, updateSectionId, "false");
            var waiter = GetWaiterForAriaBusyState(updateSection, updateSectionId, "true");

            sender.Start();
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" started.", updateSectionId));
            action.Invoke(element);
            sender.Join(BrowserDefaults.TimeOut);
            waiter.Start();
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" in progress.", updateSectionId));
            waiter.Join(BrowserDefaults.TimeOut);
            Debug.WriteLine(string.Format("ValueChange for \"{0}\" complete.", updateSectionId));

            var topLevelSelector = element.GetTopLevelSelector();
            var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);
            element = topLevelElement;
            element.SetTopLevelSelector(topLevelSelector);

            return element;
        }

        private static Thread GetWaiterForAriaBusyState(IWebElement updateSection, string updateSectionId,
            string busyStateToWaitFor)
        {
            return new Thread(() =>
            {
                {
                    var busy = updateSection.GetAttribute("aria-busy");
                    Debug.WriteLine("\"{0}\" aria-busy=\"{1}\".", updateSectionId, busyStateToWaitFor);
                    while (busy == busyStateToWaitFor)
                    {
                        busy = updateSection.GetAttribute("aria-busy");
                        Debug.WriteLine("\"{0}\" aria-busy=\"{1}\".", updateSectionId, busyStateToWaitFor);
                    }
                }
            });
        }

        private static readonly ConditionalWeakTable<IWebElement, By> TopLevelSelectors =
            new ConditionalWeakTable<IWebElement, By>();

        private static By GetTopLevelSelector(this IWebElement element)
        {
            By topLevelSelector;
            if (!TopLevelSelectors.TryGetValue(element, out topLevelSelector))
                throw new Exception("Cannot identify top level element.");
            return topLevelSelector;
        }

        private static void SetTopLevelSelector(this IWebElement element, By topLevelSelector)
        {
            TopLevelSelectors.Add(element, topLevelSelector);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get value of selected item on a combobox
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static String GetSelectedComboboxItemText(this IWebElement element)
        {
            SelectElement select = new SelectElement(element);
            var text = @select.SelectedOption.Text;
            return text;
        }

        /// <summary>
        /// Au: Luong.Mai
        /// Des: Scroll to element use action
        /// </summary>
        /// <param name="element"></param>
        public static void ScrollToByAction(this IWebElement element)
        {
            Actions actions = new Actions(WebContext.WebDriver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Select an option in a combobox control by text
        /// </summary>
        /// <param name="element"></param>
        /// <param name="text"></param>
        public static void SelectByText(this IWebElement element, String text)
        {
            SelectElement select = new SelectElement(element);
            select.SelectByText(text);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set "checked" for an checkbox
        /// </summary>
        /// <param name="element"></param>
        /// <param name="ischeck"></param>
        public static void Set(this IWebElement element, Boolean ischeck)
        {
            bool check = IsChecked(element);
            if ((ischeck && !check) || (!ischeck && check))
            {
                element.ClickByJS();
            }

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a checkbox is checked
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsChecked(this IWebElement element)
        {
            return element.Selected;

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Clicking on an element use JavaScript
        /// </summary>
        /// <param name="element"></param>
        // ReSharper disable once InconsistentNaming
        public static void ClickByJS(this IWebElement element)
        {
            ((IJavaScriptExecutor) WebContext.WebDriver).ExecuteScript("arguments[0].click();", element);
        }

        /// <summary/>
        /// Au: Huy Vo
        /// Des: GetText method uses to get text from element
        /// <param name="element">Element contais text</param>
        /// <returns>Return string value</returns>
        public static String GetText(this IWebElement element)
        {
            return element.Text;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check an element is displayed.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsElementDisplayed(this IWebElement element)
        {
            Boolean isDisplayed = false;
            if (IsElementExists(element))
            {
                try
                {
                    isDisplayed = element.Displayed;
                }
                catch (Exception e)
                {
                    Console.WriteLine("isElementDisplayed:" + e.Message);
                }
            }
            return isDisplayed;
        }

        public static Boolean IsExist(this IWebElement element)
        {
            try
            {
                if (element.Displayed)
                {
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Clear content of a TextBox element or customize controls use TextBox
        /// such as: Combobox, TextArea
        /// </summary>
        /// <param name="element"></param>
        public static void ClearText(this IWebElement element)
        {
            try
            {
                element.SendKeys(Keys.Control + "a");
                element.SendKeys(Keys.Delete);
            }
            catch (Exception e)
            {
                Console.WriteLine("Have an exception when executing 'Send Keys' :" + e);
            }
        }

        public static void SetText(this IWebElement element, string value)
        {
            try
            {
                element.Clear();
                element.SendKeys(value);
            }
            catch (StaleElementReferenceException serx)
            {
                Console.WriteLine("Can't sendkeys '" + value + "' to element - StaleElementReferenceException:" + serx);
            }
            catch (Exception)
            {
                Console.WriteLine("Can't sendkeys '" + value + "' to element " + element.TagName);
            }
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static void SetText(this IWebElement element, string value, By thingToWaitFor = null)
        {

            if (thingToWaitFor == null)
                throw new ArgumentNullException(nameof(thingToWaitFor));

            {
                WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, Environment.Settings.ElementRetrievalTimeout);
                wait.Until(ExpectedConditions.ElementToBeClickable(thingToWaitFor));
            }

            //Clear out the field using JS
            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript("arguments[0].value='';", element);
            element.SendKeys(value);
            element.SendKeys(Keys.Tab);
            AutomationSugar.WaitForAjaxCompletion();
        }

        public static void SetDateIntoField(this IWebElement element, string value)
        {
            try
            {
                element.ClearText();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                element.SetDateTime(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't sendkeys to element" + ex);
            }
        }

        /// <summary>
        /// Au: Luong.Mai
        /// Des: Get value of web element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this IWebElement element)
        {
            return element.GetAttribute("value");
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Enter a string into TextBox, TextArea... controls
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void EnterForDropDown(this IWebElement element, string value)
        {
            try
            {
                element.SendKeys(Keys.Enter);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                //IWebElement inputElement = FindElement(SimsBy.CssSelector("#select2-drop input.select2-input"));
                var inputElement =
                    ElementRetriever.GetOnceLoaded(SimsBy.CssSelector("#select2-drop input.select2-input"));
                inputElement.WaitUntilState(ElementState.Displayed);
                inputElement.SendKeys(value);
                Thread.Sleep(2000);
                inputElement.SendKeys(Keys.Enter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Have an exception happens when enter date for drop down: " + ex);
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Enter a string into TextBox, TextArea... controls
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void EnterForDropDownByClick(this IWebElement element, string value)
        {
            try
            {
                element.SendKeys(Keys.Enter);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                FindElement(SimsBy.CssSelector("#select2-drop input.select2-input"));

                var listItems = FindElements(SimsBy.CssSelector("[id^='select2-result-label']"));
                foreach (var item in listItems)
                {
                    if (item.GetText().Equals(value))
                    {
                        Retry.Do(item.Click);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Have an exception happens when enter date for drop down: " + ex);
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get a attribute of an element by att's name
        /// </summary>
        /// <param name="element"></param>
        /// <param name="att"></param>
        /// <returns></returns>
        public static String GetAttribute(this IWebElement element, String att)
        {
            try
            {
                return element.GetAttribute(att);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void SetAttribute(this IWebElement element, string att, string value)
        {
            ((IJavaScriptExecutor) WebContext.WebDriver).ExecuteScript(
                "arguments[0].setAttribute('" + att + "', '" + value + "');", element);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a element is exists with default time out.
        /// Return false if it isn't existed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false/throws a exception</returns>
        public static Boolean IsElementExists(this IWebElement element)
        {
            try
            {
                if (element != null)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("isElementExists: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a element is exists with default time out.
        /// Return false if it isn't existed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false/throws a exception</returns>
        public static Boolean IsElementExists(By element)
        {
            try
            {
                if (WebContext.WebDriver.FindElement(element) != null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///  <summary/>
        /// Des: Double Click method uses to click on an element
        /// <returns>Void</returns>
        public static void DoubleClick(this IWebElement e)
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.DoubleClick(e);
            action.Perform();
        }

        public static void ClickByAction(this IWebElement e)
        {
            Actions action = new Actions(WebContext.WebDriver);
            action.Click(e);
            action.Perform();
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check an element is collapsed
        /// </summary>
        /// <param name="element"></param>
        /// <returns>true/false</returns>
        public static Boolean IsCollapsed(this IWebElement element)
        {
            return element.GetAttribute("class").Contains("collapsed");
        }

        /// <summary>
        /// Copy a text from a WebElement
        /// </summary>
        /// <param name="elementToCopy"></param>
        public static void CopyText(this IWebElement elementToCopy)
        {
            elementToCopy.SendKeys(Keys.Control + "a");
            elementToCopy.SendKeys(Keys.Control + "c");
        }

        /// <summary>
        /// paste a text from a WebElement
        /// </summary>
        /// <param name="elementToPaste"></param>
        public static void PasteText(this IWebElement elementToPaste)
        {
            elementToPaste.ClearText();
            elementToPaste.SendKeys(Keys.Control + "v");
        }


        /// <summary>
        /// Au: Logigear
        /// Des: Get a collection web element with time out is inputed
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<IWebElement> FindElements(By element)
        {
            Wait.WaitForElement(element);
            return WebContext.WebDriver.FindElements(element);
        }

        /// <summary>
        /// Au: Logigear:
        /// Des: Finding an web element with time out is inputed
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static IWebElement FindElement(By element)
        {
            Wait.WaitForControl(element);
            return WebContext.WebDriver.FindElement(element);
        }

        public static IWebElement FindElementWithOutTimeout(By element)
        {
            return WebContext.WebDriver.FindElement(element);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Sleep a thread in a moment time (second)
        /// </summary>
        /// <param name="second"></param>
        public static void Sleep(int second)
        {
            try
            {
                Thread.Sleep(second*1000);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Finding a text in a page
        /// </summary>
        /// <param name="text"></param>
        /// <returns>true/false</returns>
        public static bool FindTextInPage(String text)
        {
            bool isFound = false;
            if (WebContext.WebDriver.PageSource.Contains(text))
            {
                isFound = true;
            }

            return isFound;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Get the parent tab of opened browser
        /// </summary>
        /// <returns></returns>
        public static string GetParentTab()
        {
            return WebContext.WebDriver.CurrentWindowHandle;
        }

        /// <summary>
        /// Au: Huy Vo
        /// Des: SwitchToNewTab method uses to switch to new tab that is opening on Browsers
        /// </summary>
        /// <param name="parentTab">parentTab: Current tab that being focused</param>
        public static void SwitchToNewTab(params string[] parentTab)
        {
            IList<string> parentTabs = parentTab.ToList();
            string newTab = string.Empty;
            ReadOnlyCollection<string> windowHandles = WebContext.WebDriver.WindowHandles;
            foreach (string handle in windowHandles)
            {
                if (!parentTabs.Contains(handle))
                {
                    newTab = handle;
                    break;
                }
            }
            if (!newTab.Equals(string.Empty))
            {
                WebContext.WebDriver.SwitchTo().Window(newTab);
            }
        }

        /// <summary>
        /// Au: Huy Vo
        /// Des: CloseTab uses to close current tab on IE, Chrome, Firefox
        /// </summary>
        public static void CloseTab()
        {
            WebContext.WebDriver.Close();
        }

        /// <summary>
        /// Author: Huy Vo
        /// Description: Switch to parent tab such as root tab on Browsers
        /// </summary>
        /// <param name="parentTab">parentTab: The first tab is opened on Browser</param>
        public static void SwitchToRootTab(string parentTab)
        {
            WebContext.WebDriver.SwitchTo().Window(parentTab);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Comparing two lists string together
        /// </summary>
        /// <param name="listRowElement1"></param>
        /// <param name="listRowElement2"></param>
        /// <returns>true/false</returns>
        public static bool DoesListItemEqual(List<string> listRowElement1, List<string> listRowElement2)
        {
            if (listRowElement1.Count != listRowElement2.Count)
                return false;

            for (int i = 0; i < listRowElement1.Count; i++)
                if (!listRowElement1[i].Equals(listRowElement2[i]))
                {
                    return false;
                }
            return true;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Convert a collect IWebElement to list string
        /// </summary>
        /// <param name="webElementCollection"></param>
        /// <param name="listTextWebElement"></param>
        /// <param name="isGetAttribute"></param>
        /// <param name="attribute"></param>
        public static void ConvertToListItem(List<IWebElement> webElementCollection, ref List<string> listTextWebElement,
            bool isGetAttribute = false, string attribute = "value")
        {
            listTextWebElement.Clear();
            foreach (var item in webElementCollection)
            {
                if (!isGetAttribute)
                {
                    listTextWebElement.Add(item.Text.Trim());
                }
                else
                {
                    listTextWebElement.Add(item.GetAttribute(attribute).Trim());
                }
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a web element existed
        /// </summary>
        /// <returns></returns>
        public static bool DoesWebElementExist(By element)
        {
            bool isExist;
            try
            {
                isExist = FindElement(element) != null;
            }
            catch (Exception)
            {
                isExist = false;
            }

            return isExist;
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Check a web element existed
        /// </summary>
        /// <returns></returns>
        public static bool DoesWebElementExist(IWebElement element)
        {
            bool isExisted = true;
            try
            {
                // ReSharper disable once UnusedVariable
                bool isDisplayed = element.Displayed;
            }
            catch (NoSuchElementException)
            {
                isExisted = false;
            }

            return isExisted;
        }

        /// <summary>
        /// Au: Logigear
        /// Des : Switch to window with name of windows. Name of windows will be contain at Automation.WebContext.WebDriver.WindowHandles
        /// </summary>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToWindow(string windowHandle)
        {
            try
            {
                WebContext.WebDriver.SwitchTo().Window(windowHandle);
            }
            catch (NoSuchWindowException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Au : Logigear
        /// Des : Switch to new window opened and save current window before switch
        /// </summary>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToNewWindow(out string currentWindow)
        {
            try
            {
                //Store the current window handle
                currentWindow = WebContext.WebDriver.CurrentWindowHandle;

                //Switch to new window opened
                foreach (string winHandle in WebContext.WebDriver.WindowHandles)
                {
                    SwitchToWindow(winHandle);
                }

            }
            catch (NoSuchWindowException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Execute a specifice javascript command.
        /// </summary>
        /// <param name="commandScript"></param>
        /// <returns></returns>
        public static string ExecuteJavascript(string commandScript)
        {
            try
            {
                IWebDriver webDriver = WebContext.WebDriver;
                IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

                var result = js?.ExecuteScript(commandScript);
                if (result != null)
                    return result.ToString();
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Execute a specifice javascript command with element
        /// </summary>
        /// <param name="commandScript"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ExecuteJavascript(string commandScript, By element)
        {
            try
            {
                IWebDriver webDriver = WebContext.WebDriver;
                IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

                var result = js?.ExecuteScript(commandScript, Get(element));
                if (result != null)
                    return result.ToString();
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Au : Logigear
        /// Des : Switch to new window opened
        /// </summary>
        /// <exception>Throws NoSuchWindowException</exception>
        public static void SwitchToNewWindow()
        {
            try
            {
                //Switch to new window opened
                foreach (string winHandle in WebContext.WebDriver.WindowHandles)
                {
                    SwitchToWindow(winHandle);
                }
            }
            catch (NoSuchWindowException e)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw e;
            }
        }

        public static string GetCurrentWindow()
        {
            try
            {
                return WebContext.WebDriver.CurrentWindowHandle;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static void SelectDropDownListItem(By elementListItem, string value)
        {
            ReadOnlyCollection<IWebElement> listItme = FindElements(elementListItem);
            foreach (IWebElement item in listItme)
            {
                if (item.Text.Equals(value))
                {

                    item.Click();
                    return;
                }
            }

        }

        public static void SelectElemetAtIndex(By elementListItem, int index)
        {
            ReadOnlyCollection<IWebElement> listItem = FindElements(elementListItem);

            if (listItem.Any())
            {

                listItem[index].Click();
            }

        }

        public static void SendKeyToElement(By element, string p)
        {
            try
            {
                if (p.Equals("enter"))
                {
                    Get(element).SendKeys(Keys.Enter);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="value">valute</param>
        /// <param name="formatTime"></param>
        /// <param name="timeOnly"></param>
        public static void SetDateTime(this IWebElement element, string value, bool formatTime = false,
            bool timeOnly = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");
            if (!String.IsNullOrEmpty(value))
            {
                if (!formatTime)
                {
                    element.SetText(
                        DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture).ToString(elementFormat));
                }
                else
                {
                    if (timeOnly)
                    {
                        element.SetText(
                            DateTime.ParseExact(value, "h:mm tt", CultureInfo.InvariantCulture).ToString(elementFormat));
                    }
                    else
                    {
                        element.SetText(
                            DateTime.ParseExact(value, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture)
                                .ToString(elementFormat));
                    }
                }
            }
            else
            {
                element.SetText(String.Empty);
            }

        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="value">valute</param>
        /// <param name="formatTime"></param>
        // ReSharper disable once InconsistentNaming
        public static void SetDateTimeByJS(this IWebElement element, string value, bool formatTime = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");

            ((IJavaScriptExecutor) WebContext.WebDriver).ExecuteScript("arguments[0].value='';", element);

            if (!formatTime)
            {
                element.SetText(
                    DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture).ToString(elementFormat));
            }
            else
            {
                element.SetText(
                    DateTime.ParseExact(value, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString(elementFormat));
            }

            element.SendKeys(Keys.Tab);
        }

        /// <summary>
        /// Sets date time whilst requerying for elements for each operation to avoid
        /// stale element reference exceptions
        /// </summary>
        /// <param name="by"></param>
        /// <param name="value"></param>
        /// <param name="formatTime"></param>
        // ReSharper disable once InconsistentNaming
        public static void SetDateTimeByJS(By by, string value, bool formatTime = false)
        {
            // Ensure element has focus (tabbing call at the end of this method doesn't seem to work properly)
            WebContext.WebDriver.FindElementSafe(by).Click();

            string elementFormat = Regex.Replace(WebContext.WebDriver.FindElementSafe(by).GetAttribute("data-date-validator-format"), "a|A", "tt");

            WebContext.WebDriver.FindElementSafe(by).ClearText();

            if (!formatTime)
            {
                WebContext.WebDriver.FindElementSafe(by).SetText(
                    DateTime.ParseExact(value, "d/M/yyyy", CultureInfo.InvariantCulture).ToString(elementFormat));
            }
            else
            {
                WebContext.WebDriver.FindElementSafe(by).SetText(
                    DateTime.ParseExact(value, "d/M/yyyy h:mm tt", CultureInfo.InvariantCulture).ToString(elementFormat));
            }

            // Note: I don't think this works!
            WebContext.WebDriver.FindElementSafe(by).SendKeys(Keys.Tab);
        }

        public static string GetDateTime(By by, bool formatTime = false, bool timeOnly = false)
        {
            string elementFormat = Regex.Replace(FindElement(by).GetAttribute("data-date-validator-format"), "a|A", "tt");

            string value = FindElement(by).GetAttribute("value");

            if (String.IsNullOrWhiteSpace(value))
            {
                value = FindElement(by).GetText();
            }

            return Format(value, elementFormat, formatTime, timeOnly);
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Set Date Time for Date Time textbox
        /// </summary>
        /// <param name="element">Date Time textbox</param>
        /// <param name="formatTime">contains time or not</param>
        /// <param name="timeOnly"></param>
        public static string GetDateTime(this IWebElement element, bool formatTime = false, bool timeOnly = false)
        {
            string elementFormat = Regex.Replace(element.GetAttribute("data-date-validator-format"), "a|A", "tt");
            string value = element.GetAttribute("value");
            if (String.IsNullOrEmpty(value))
            {
                value = element.GetText();
            }
            return Format(value, elementFormat, formatTime, timeOnly);
        }

        public static string GetCurrentUrl()
        {
            return WebContext.WebDriver.Url;
        }

        public static string GenerateRandomString(int length)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[random.Next(chars.Length)]);

            return builder.ToString();
        }

        public static string GenerateRandomNumberUsingDateTime(int length = 0)
        {
            int randomNumber = Math.Abs((int)DateTime.Now.Ticks);
            if (length == 0)
            {
                return randomNumber.ToString();
            }
            string time = randomNumber.ToString();
            string rand = time.Substring(0, length);
            return rand;
        }

        public static int GenerateRandomNumber(int max)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(max);
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Des : Generate random number, return a string.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomNumberInString(int length)
        {
            var chars = "0123456789";
            var randomString = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[random.Next(chars.Length)]);
            }
            return randomString.ToString();
        }

        /// <summary>
        /// Get a random date in a range
        /// </summary>
        static readonly Random Rnd = new Random();
        public static DateTime GetRandomDate(DateTime from, DateTime to)
        {
            var range = to - from;

            var randTimeSpan = new TimeSpan((long)(Rnd.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public static void ClickLink(string cssSelector)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));

            var script = "try { " +
                "var link = document.querySelector(\"" + cssSelector + "\"); " +
                "link.click(); " +
            "} catch(ex) { " +
                "console.log('ToggleCheckboxForLabel: failed with - ' + ex); " +
            "} ";

            ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(script);
        }

        public static void WaitUntilElementIsDisplayed(string xpathSelector)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathSelector)));
        }

        #region Fluent
        /// <summary>
        /// Gets an element from the DOM.
        /// </summary>
        /// <param name="selector">A selector describing which element to retrieve. Scoped to the document root.</param>
        /// <returns></returns>
        public static IWebElement Get(By selector)
        {
            IWebElement element = ElementRetriever.GetOnceLoaded(selector);
            element.SetTopLevelSelector(selector);
            return element;
        }

        /// <summary>
        /// Gets an element from unique element automation id
        /// </summary>
        /// <param name="id">unique element automation id</param>
        /// <returns></returns>
        public static IWebElement Get(string id)
        {
            var cssFormat = AutomationId(id);
            return Get(By.CssSelector(cssFormat));
        }

        /// <summary>
        /// An element (such as a div) is considered available and displayed even with a zero height or width. This method corrects that.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IWebElement GetVisible(By selector)
        {
            var elementToGet = ElementRetriever.GetOnceLoaded(selector);
            if (elementToGet.Displayed
                && elementToGet.Size.Height > 0
                && elementToGet.Size.Width > 0)
            {
                return elementToGet;
            }

            // Element not truly visible
            return null;
        }

        /// <summary>
        /// Clicks an element.
        /// </summary>
        /// <param name="element">The element containing the element to click.</param>
        /// <param name="selector">A selector describing which element to click. Scoped by the element this method is called upon.</param>
        /// <returns></returns>
        public static IWebElement Click(this IWebElement element, By selector)
        {
            var elementToClick = element.FindChild(selector);
            elementToClick.WaitUntilState(ElementState.Displayed);

            elementToClick.Click();
            return element;
        }

        /// <summary>
        /// Gets an element contained within another.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing which element to retrieve. Scoped by the element this method is called upon.</param>
        /// <returns></returns>
        public static IWebElement FindChild(this IWebElement element, By selector)
        {
            element.WaitUntilState(ElementState.Displayed);
            return ElementRetriever.GetOnceLoaded(element, selector);
        }

        /// <summary>
        /// Clicks on one element, waits for another to load. Typically used when areas of the DOM are being replaced. For example, saving and reloading a record.
        /// </summary>
        /// <param name="elementToClick">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="elementToWaitFor">A selector describing which element to wait until it's loaded. Scoped to the document root.</param>
        /// <returns>The element you've been waiting for.</returns>
        public static IWebElement ClickAndWaitFor(By elementToClick, By elementToWaitFor)
        {
            var toClick = Get(elementToClick);
            Retry.Do(toClick.Click);

            var toWaitUpon = Get(elementToWaitFor);
            toWaitUpon.WaitUntilState(ElementState.Displayed);

            return ElementRetriever.GetOnceLoaded(elementToWaitFor);
        }

        /// <summary>
        /// Clicks on one element, waits for another to load. Typically used when areas of the DOM are being replaced. For example, saving and reloading a record.
        /// Using Javascript
        /// </summary>
        /// <param name="elementToClick">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="elementToWaitFor">A selector describing which element to wait until it's loaded. Scoped to the document root.</param>
        /// <returns>The element you've been waiting for.</returns>
        // ReSharper disable once InconsistentNaming
        public static IWebElement ClickAndWaitForByJS(By elementToClick, By elementToWaitFor)
        {
            var toClick = Get(elementToClick);
            toClick.ClickByJS();

            var toWaitUpon = Get(elementToWaitFor);
            toWaitUpon.WaitUntilState(ElementState.Displayed);

            return ElementRetriever.GetOnceLoaded(elementToWaitFor);
        }

        public static IWebElement WaitForUpdateSection(this IWebElement element, By elementInUpdateSection)
        {
            var topLevelSelector = element.GetTopLevelSelector();
            var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);
            var updatedElement = topLevelElement.FindElementSafe(elementInUpdateSection);
            updatedElement.WaitUntilState(ElementState.Displayed);

            element = topLevelElement;
            element.SetTopLevelSelector(topLevelSelector);
            return element;
        }

        public static IWebElement WaitForUpdateSection(this IWebElement element, string updateSectionId)
        {
            //var topLevelSelector = element.GetTopLevelSelector();
            //var topLevelElement = ElementRetriever.GetOnceLoaded(topLevelSelector);

            var updateSection = element.FindChild(By.CssSelector("[data-section-id=\"" + updateSectionId + "\"]"));
            for (int i = 0; i < 1000; i++)
            {
                var busy = updateSection.GetAttribute("aria-busy");

                Debug.WriteLine("aria-busy: {0} {1}", updateSectionId, busy);
                Thread.Sleep(1);
            }
            //var updatedElement = topLevelElement.FindElementSafe(elementInUpdateSection);
            //updatedElement.WaitUntilState(ElementState.Displayed);

            //element = topLevelElement;
            //element.SetTopLevelSelector(topLevelSelector);
            return element;
        }

        public static IWebElement ClickAndWaitFor(this IWebElement elementToClick, By elementToWaitFor)
        {
            elementToClick.Click();
            var toWaitUpon = Get(elementToWaitFor);
            toWaitUpon.WaitUntilState(ElementState.Displayed);
            return ElementRetriever.GetOnceLoaded(elementToWaitFor);

        }



        /// <summary>
        /// Gets the specified grid row from a grid.
        /// </summary>
        /// <param name="element">The element containing the grid.</param>
        /// <param name="gridSelector">A selector describing which grid the row to retrieve belongs to. Scoped by the element this method is called upon.</param>
        /// <param name="rowIndex">The zero-based index of the row to retrieve.</param>
        /// <returns></returns>
        public static IWebElement GetGridRow(this IWebElement element, By gridSelector, int rowIndex)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> els = wait.Until(x => x
                .FindElement(gridSelector)
                .WaitUntilState(ElementState.Displayed)
                .FindElements(By.CssSelector(gridRowCss)));

            return els[rowIndex];
        }

        /// <summary>
        /// Gets the specified grid row from a grid.
        /// </summary>
        /// <param name="element">The element containing the grid.</param>
        /// <param name="rowIndex">The zero-based index of the row to retrieve.</param>
        /// <returns></returns>
        public static IWebElement GetGridRow(this IWebElement element, int rowIndex)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            // ReSharper disable once UnusedVariable
            var webDriverWait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> els = element.WaitUntilState(ElementState.Displayed).FindElements(By.CssSelector(gridRowCss));

            return els[rowIndex];
        }

        public static ReadOnlyCollection<IWebElement> GetGridRows(this IWebElement element, By gridSelector)
        {
            const string gridRowCss = "tbody tr[data-role=\"gridRow\"]";

            element.WaitUntilState(ElementState.Displayed);
            var wait = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(15));
            ReadOnlyCollection<IWebElement> rowCollection = wait.Until(x => x
                .FindElement(gridSelector)
                .WaitUntilState(ElementState.Displayed)
                .FindElements(By.CssSelector(gridRowCss)));

            return rowCollection;
        }

        /// <summary>
        /// Selects an option from a "Selector" control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the top-level element of the Selector control. Scoped by the element this method is called upon.</param>
        /// <param name="option">The literal value of the option to select (case sensitive).</param>
        /// <returns></returns>
        public static IWebElement ChooseSelectorOption(this IWebElement element, By selector, string option)
        {
            var selectorElement = element.FindChild(selector);
            selectorElement.WaitUntilState(ElementState.Displayed);
            selectorElement.Click();

            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";

            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);
            selectorSearchElement.Click();
            selectorSearchElement.Clear();
            selectorSearchElement.SendKeys(option);
            selectorSearchElement.SendKeys(Keys.Tab);
            return element;
        }

        public static IWebElement ChooseSelectorOption(this IWebElement element, string option)
        {
            element.WaitUntilState(ElementState.Displayed);
            Retry.Do(element.Click);
            const string selectorSearchBox = "[id*=s2id_autogen][id$=_search]";
            var selectorSearchElement = ElementRetriever.GetOnceLoaded(By.CssSelector(selectorSearchBox));
            selectorSearchElement.WaitUntilState(ElementState.Displayed);
            selectorSearchElement.Click();
            selectorSearchElement.Clear();
            selectorSearchElement.SendKeys(option);
            selectorSearchElement.SendKeys(Keys.Tab);
            return element;
        }
        /// <summary>
        /// Selects an option from a "MultiSelector" control.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of all checkboxes within a MultiSelector. Typically the "Name" attribute. Scoped by the element this method is called upon.</param>
        /// <param name="option">The literal value of the option to select (case sensitive).</param>
        /// <returns></returns>
        public static IWebElement ChooseMultiSelectorOption(this IWebElement element, By selector, string option)
        {
            element.WaitUntilState(ElementState.Displayed);

            foreach (var e in element.FindElementsSafe(selector))
            {
                var id = e.GetAttribute("id");
                var label = element.FindElementSafe(By.CssSelector("label[for=\"" + id + "\""));

                if (label.Text == option)
                {
                    label.Click();
                    break;
                }
            }
            return element;
        }

        /// <summary>
        /// Waits for element to be clickable and then clicks on it
        /// </summary>
        /// <param name="selector">element selector</param>
        public static void WaitForElementClickableThenClick(By selector)
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementToBeClickable(selector));
            FindAndClick(selector);
        }

        /// <summary>
        /// Sets the value of a Checkbox.
        /// </summary>
        /// <param name="element">The containing element.</param>
        /// <param name="selector">A selector describing the retrieval of the Checkbox. Scoped by the element this method is called upon.</param>
        /// <param name="checked">Checked or not.</param>
        /// <returns></returns>
        public static IWebElement SetCheckBox(this IWebElement element, By selector, bool @checked)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                    checkBoxElement.Click();
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                    checkBoxElement.Click();
            }

            return element;
        }

        public static IWebElement SetCheckBox(this IWebElement checkBoxElement, bool @checked)
        {
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                    checkBoxElement.Click();
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                    checkBoxElement.Click();
            }

            return checkBoxElement;
        }

        //tristate click
        public static IWebElement SetTriStateCheckBox(this IWebElement element, By selector, bool @checked)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            if (@checked)
            {
                //Assuming NULL means unchecked.
                if (checkedValue == null)
                {
                    checkBoxElement.Click();

                }
            }
            else
            {
                //Assuming !NULL means checked.
                if (checkedValue != null)
                {
                    checkBoxElement.Click();

                }
            }

            return element;
        }


        /// <summary>
        /// Sets the value of a Text control.
        /// </summary>
        /// <param name="element">
        ///     The containing element.
        ///     The containing element.
        /// </param>
        /// <param name="selector">
        ///     A selector describing the retrieval of the Text control. Scoped by the element this method is called upon.
        ///     A selector describing the retrieval of the Checkbox. Scoped by the element this method is called upon.
        /// </param>
        /// <returns></returns>
        /// <summary>
        /// Checks the value of the checkbox and returns TRUE if checkbox is checked else returns FALSE.
        /// </summary>
        /// <returns>TRUE if checkbox is checked else returns FALSE.</returns>
        public static bool IsCheckboxChecked(this IWebElement element, By selector)
        {
            var checkBoxElement = element.FindChild(selector);
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            //Assuming !NULL means checked.
            return checkedValue != null;
        }

        public static bool IsCheckboxChecked(this IWebElement checkBoxElement)
        {
            checkBoxElement.WaitUntilState(ElementState.Displayed);

            var checkedValue = checkBoxElement.GetAttribute("checked");

            //Assuming !NULL means checked.
            return checkedValue != null;
        }

        ///// <summary>
        ///// Returns the value of the element
        ///// </summary>
        ///// <param name="element">The containing element.</param>
        ///// <returns></returns>
        //public static string GetValue(this IWebElement element)
        //{
        //    return element.GetAttribute("value") ?? String.Empty;
        //}

        public static void Type(this IWebElement element, string value, int timeWait = 500)
        {
            try
            {
                element.ClearText();
                foreach (char c in value)
                {
                    element.SendKeys(c.ToString());
                    Thread.Sleep(timeWait);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't sendkeys to element" + ex);
            }
        }

        #endregion

        #region General
        /// <summary>
        /// Creates an instance of a Selenium Component
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public static TComponent InitialiseComponent<TComponent>() where TComponent : new()
        {
            //do some other stuff if you want

            return new TComponent();
        }
        #endregion

        #region Navigation

        /// <summary>
        /// Navigates to the page via the side menu
        /// </summary>
        /// <param name="topLevel">e.g.: Tasks or Lookups</param>
        /// <param name="category">e.g.: Staff</param>
        /// <param name="item">e.g: Service Terms</param>
        public static void NavigateMenu(string topLevel, string category, string item)
        {
            FindAndClick(By.CssSelector("[title=\"Task Menu\"]"));
            var taskMenu = Get(By.CssSelector("#task-menu"));
            var level1 = taskMenu.FindElementsSafe(By.CssSelector(".shell-task-menu-section"));
            foreach (var l1Item in level1)
            {
                var l1Header = l1Item.FindElementSafe(By.CssSelector(".shell-task-menu-section-header"));
                if (l1Header.Text.Trim().Equals(topLevel, StringComparison.OrdinalIgnoreCase))
                {
                    var level2 = l1Item.FindElementsSafe(By.CssSelector(".panel-group.accordion-sidemenu .panel-default.panel"));
                    foreach (var l2Item in level2)
                    {
                        var l2Header = l2Item.FindElementSafe(By.CssSelector(".accordion-toggle"));
                        if (l2Header.Text.Trim().Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            if (l2Header.GetAttribute("aria-expanded").Trim().ToLower().Equals("false"))
                            {
                                l2Header.ClickByJS();
                                Thread.Sleep(1000);
                            }
                            var level3 = l2Item.FindElementsSafe(By.CssSelector(".shell-task-menu-item-title")); //changed based on the menu item Title
                            foreach (var l3Item in level3)
                            {
                                if (l3Item.Text.Trim().Equals(item))
                                {
                                    l3Item.ClickByJS();
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }

        /// <summary>
        /// Navigate to menu item by enter menu item into search textbox
        /// </summary>
        /// <param name="itemMenu"></param>
        /// <param name="wait"></param>
        public static void NavigateBySearch(string itemMenu, bool wait = false)
        {
            WaitForElementClickableThenClick(By.CssSelector(AutomationId("task_menu")));
	        FindElementWithOutTimeout(SimsBy.Id("task-menu-search"));
            var searchTextbox = Get(SimsBy.Id("task-menu-search"));
            if (wait)
            {
                searchTextbox.Type(itemMenu);
            }
            else
            {
                searchTextbox.Clear();
                searchTextbox.SendKeys(itemMenu);
            }

	        Thread.Sleep(1000);
            IWebElement taskResult = null;
            IReadOnlyCollection<IWebElement> taskResults = FindElements(SimsBy.CssSelector("[data-automation-id^='Task_Menu_Search_Result_title']"));

            foreach (var item in taskResults)
            {
                if (item.Text.Trim().ToLower().Equals(itemMenu.ToLower()))
                {
                    taskResult = item;
                    break;
                }
            }

            if (taskResult == null)
            {
                //Re-type slower to find menu item once more time
                searchTextbox.Type(itemMenu, 1000);
                taskResults = FindElements(SimsBy.CssSelector("[data-automation-id^='Task_Menu_Search_Result_title']"));

                foreach (var item in taskResults)
                {
                    if (item.Text.Trim().ToLower().Equals(itemMenu.ToLower()))
                    {
                        taskResult = item;
                        break;
                    }
                }

                if (taskResult == null)
                {
                    throw new NoSuchElementException("Menu for '" + itemMenu + "' does not exist");
                }
            }

            taskResult.Click();
            Wait.WaitForAjaxReady(By.Id("nprogress"));
        }


        public static T NavigateViaAction<T>(string actionName) where T : BaseComponent, new()
        {
            if (DoesWebElementExist(SimsBy.CssSelector("[title='Actions'] a")))
            {
                if (FindElement(SimsBy.CssSelector("[title='Actions'] a")).Displayed)
                {
                    Get(SimsBy.CssSelector("[title='Actions'] a")).ClickByJS();
                    Wait.WaitUntilDisplayed(By.CssSelector("[data-section-id='contextual-actions']"));
                }
            }

            Wait.WaitForAndClick(BrowserDefaults.ElementTimeOut, SimsBy.CssSelector(String.Format("[data-loading-text='{0}']", actionName)));
            Wait.WaitForAjaxReady(By.Id("nprogress"));

            var pageDetail = default(T);

            Retry.Do(() =>
            {
                pageDetail = new T();
            });

            pageDetail.Refresh();
            return pageDetail;
        }

        /// <summary>
        /// Navigates to the page via the Quick Link
        /// </summary>
        /// <param name="item">e.g: Pupil Records</param>
        public static void NavigateQuickLink(string item)
        {
            IReadOnlyCollection<IWebElement> quickLinks = FindElements(SimsBy.CssSelector("#quick-links a"));
            foreach (var quickLink in quickLinks)
            {
                if (quickLink.GetText().Trim().Equals(item))
                {
                    quickLink.ClickByJS();
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    break;
                }
            }
        }

        /// <summary>
        /// Close tab
        /// </summary>
        /// <param name="tabName">name of tab which want to close</param>
        public static void CloseTab(string tabName)
        {
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    IWebElement closeButton = tab.FindElement(By.CssSelector(".tab-close"));
                    Retry.Do(closeButton.Click);
                    Wait.WaitForElementReady(By.Id("nprogress"));
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    Sleep(2);
                    break;
                }
            }
        }

        /// <summary>
        /// Select tab
        /// </summary>
        /// <param name="tabName">name of tab which want to close</param>
        public static void SelectTab(string tabName)
        {
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    Retry.Do(tab.Click);
                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    break;
                }
            }
        }

        /// <summary>
        /// Check the tab is displayed.
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public static bool IsTabDisplay(string tabName)
        {
            Sleep(5);
            IReadOnlyCollection<IWebElement> tabs = FindElements(By.CssSelector(".page-tabs [role='tab']"));
            foreach (var tab in tabs)
            {
                if (tab.GetAttribute("data-tab-name").Trim().Equals(tabName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Open About SIMS popup
        /// </summary>
        public static bool OpenAboutPopup()
        {
            FindAndClick(By.CssSelector(".navbar-brand-header button"));
            return DoesWebElementExist(SimsBy.AutomationId("popover-custom-id"));
        }


        /// <summary>
        /// Open Help Page
        /// </summary>
        public static void OpenHelpPage()
        {
            var helpLink = FindElement(By.CssSelector("[title='Help']"));
            helpLink.Click();
        }

        #endregion

        #region Login

        /// <summary>
        /// The types of iSIMS user.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public enum iSIMSUserType
        {
            TestUser,
            PersonnelOfficer,
            SchoolAdministrator,
            // ReSharper disable once InconsistentNaming
            SENCoordinator,
            AdmissionsOfficer,
            HeadTeacher,
            SystemManger,
            AssessmentCoordinator,
            CurricularManager,
            SeniorManagementTeam,
            ClassTeacher,
            ReturnsManager,
            TestUserWaterEdgePrimary

        }

        /// <summary>
        /// Log in as a given user. SchoolAdmin as default.
        /// </summary>
        /// <param name="userType">The user type to login as.</param>
        /// <param name="schoolSelection">show  school selection?</param>
        /// <param name="enabledFeatures">The FeatureBee features to enable.</param>
        public static void Login(iSIMSUserType userType = iSIMSUserType.SchoolAdministrator, bool schoolSelection = true, params string[] enabledFeatures)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            switch (userType)
            {
                case iSIMSUserType.TestUser:
                case iSIMSUserType.TestUserWaterEdgePrimary:
                    Login(
                        TestDefaults.Default.TestUser,
                        TestDefaults.Default.TestUserPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        schoolSelection,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SchoolAdministrator:
                   Login(
                        TestDefaults.Default.SchoolAdministratorUser,
                        TestDefaults.Default.SchoolAdministratorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SENCoordinator:
                    Login(
                        TestDefaults.Default.SENCoordinator,
                        TestDefaults.Default.SENCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.AdmissionsOfficer:
                    Login(
                        TestDefaults.Default.AdmissionsOfficer,
                        TestDefaults.Default.AdmissionsOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.HeadTeacher:
                    Login(
                        TestDefaults.Default.HeadTeacher,
                        TestDefaults.Default.HeadTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SystemManger:
                    Login(
                        TestDefaults.Default.SystemManger,
                        TestDefaults.Default.SystemMangerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.AssessmentCoordinator:
                    Login(
                        TestDefaults.Default.AssessmentCoordinator,
                        TestDefaults.Default.AssessmentCoordinatorPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.CurricularManager:
                    Login(
                        TestDefaults.Default.CurricularManager,
                        TestDefaults.Default.CurricularManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.SeniorManagementTeam:
                    Login(
                        TestDefaults.Default.SeniorManagementTeam,
                        TestDefaults.Default.SeniorManagementTeamPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.ClassTeacher:
                    Login(
                        TestDefaults.Default.ClassTeacher,
                        TestDefaults.Default.ClassTeacherPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.ReturnsManager:
                    Login(
                        TestDefaults.Default.ReturnsManager,
                        TestDefaults.Default.ReturnsManagerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                case iSIMSUserType.PersonnelOfficer:
                    Login(
                        TestDefaults.Default.PersonnelOfficer,
                        TestDefaults.Default.PersonnelOfficerPassword,
                        TestDefaults.Default.TenantId,
                        TestDefaults.Default.SchoolName,
                        Configuration.GetSutUrl(),
                        false,
                        enabledFeatures);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("userType");
            }
        }

        /// <summary>
        /// Log in to iSIMS.
        /// </summary>
        public static void Login(string user, string password, int tenantId, string schoolName, string url, bool schoolSelection = true, params string[] enabledFeatures)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            // ReSharper disable once UnusedVariable
            HomePage homePage = HomePage.NavigateTo(user, password, tenantId, schoolName, url, schoolSelection, enabledFeatures);
            /*
            if (schoolSelection)
                homePage.MenuBar().WaitFor();
             */
            Wait.WaitForDocumentReady();
        }

        public static void Logout()
        {
            HomePage.SignOut();
        }
        #endregion

        #region Seacrh Criteria

        /// <summary>
        /// Function to click on a checkbox within a checkbox group in a Search Criteria
        /// eg in form[data-section-id='searchCriteria'
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="checkBoxGroupText"></param>
        /// <param name="checkBoxText"></param>
        public static void FindCheckBoxAndClick(this IWebElement searchCriteria, string checkBoxGroupText, List<string> checkBoxText)
        {
            IReadOnlyCollection<IWebElement> getCheckBoxElements = searchCriteria.FindElements(By.CssSelector(".checkbox-tree.wrap"));
            foreach (var checkBoxElement in getCheckBoxElements)
            {
                var groupHeader = checkBoxElement.FindElement(By.CssSelector("a[data-parent]"));
                var groupHeaderId = groupHeader.GetAttribute("data-parent");

                if (groupHeader.Text == checkBoxGroupText)
                {
                    var checkboxContainer =
                        checkBoxElement.FindElements(By.CssSelector("label[data-checkbox-container='" + groupHeaderId + "']"));

                    foreach (var checkbox in checkboxContainer.Where(cb => checkBoxText.Any(cb.Text.Contains)))
                    {
                        checkbox.Click();
                    }
                }
            }
        }
        #endregion

        #region Find and Click
        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="elementAccessor">A selector describing which element to click. Scoped to the document root.</param>
        public static void FindAndClick(By elementAccessor)
        {
            //TODO Use ElementRetrieval helper methods.
            Wait.WaitForAndClick(BrowserDefaults.TimeOut, elementAccessor);
        }

        /// <summary>
        /// Find an element and click it.
        /// </summary>
        /// <param name="elementAccessor">A selector describing which element to click. Scoped to the document root.</param>
        /// <param name="timeout">The longest time to wait for the action to be completed.</param>
        public static void FindAndClick(By elementAccessor, TimeSpan timeout)
        {
            //TODO Use ElementRetrieval helper methods.
            Wait.WaitForAndClick(timeout, elementAccessor);
        }

        /// <summary>
        /// Find and element with a specific part-name concatenated to main selector
        /// </summary>
        /// <param name="elementAccessor"></param>
        /// <param name="cssElements"></param>
        public static void FindAndClick(string elementAccessor, params object[] cssElements)
        {
            var link = By.CssSelector(string.Format(elementAccessor, cssElements));
            FindAndClick(link);
        }
        #endregion

        #region Other

        // ReSharper disable once InconsistentNaming
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Au: Logigear
        /// Des: Get a current time
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static string Format(string value)
        {
            return Format(value, false);
        }

        public static string Format(string value, bool formatTime)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var dateTime = DateTime.Parse(value);
            return dateTime.ToString(formatTime ? "d/M/yyyy h:mm tt" : "d/M/yyyy");
        }

        public static string Format(string value, string valueFormat, bool formatTime = false, bool timeOnly = false)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var dateTime = DateTime.ParseExact(value, valueFormat, CultureInfo.InvariantCulture);
                if (formatTime)
                {
                    if (timeOnly)
                    {
                        return dateTime.ToString("h:mm tt");
                    }
                    return dateTime.ToString("d/M/yyyy h:mm tt");
                }
                return dateTime.ToString("d/M/yyyy");
            }
            return string.Empty;
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Get First day of week (use for create data provider)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime date)
        {
            int range = DayOfWeek.Monday - date.DayOfWeek;
            return date.AddDays(range);
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Get Last day of week (use for create data provider)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(DateTime date)
        {
            int range = date.DayOfWeek - DayOfWeek.Sunday;
            return date.AddDays(7 - range);
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the last day of current month
        /// </summary>
        /// <returns></returns>
        public static string GetLastDayOfMonth()
        {
            DateTime now = DateTime.Now;
            int lastDayOfMonth = DateTime.DaysInMonth(now.Year, now.Month);
            return new DateTime(now.Year, now.Month, lastDayOfMonth).ToString("d/M/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the day before today
        /// </summary>
        /// <returns></returns>
        public static string GetDayBeforeToday()
        {
            return DateTime.Today.AddDays(-1).ToString("d/M/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get today
        /// </summary>
        /// <returns></returns>
        public static string GetToday()
        {
            DateTime now = DateTime.Now;
            return now.ToString("d/M/yyyy");
        }

        /// <summary>
        /// Au: Hieu Pham
        /// Get the day of week in the next week or current week
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <param name="isCurrentWeek"></param>
        /// <returns></returns>
        public static string GetDateFromDayOfWeek(int dayOfWeek, bool isCurrentWeek = false)
        {
            DateTime today = DateTime.Today;
            int daysUntilDestinationDay = (dayOfWeek - (int)today.DayOfWeek) % 7;
            if (isCurrentWeek)
            {
                return today.AddDays(daysUntilDestinationDay).ToString("d/M/yyyy");
            }
            return today.AddDays(daysUntilDestinationDay + 7).ToString("d/M/yyyy");
        }

        /// <summary>
        /// Au : Hieu Pham
        /// Get the day after or before current day in a number of days. Format input date required d/M/yyyy
        /// </summary>
        /// <param name="currentDate"></param> Current Date 
        /// <param name="numberOfDay"></param> Negative if before, positive if after.
        /// <returns></returns>
        public static string GetDayAfter(string currentDate, int numberOfDay)
        {
            DateTime currentDateTime = DateTime.ParseExact(currentDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            return currentDateTime.AddDays(numberOfDay).ToString("d/M/yyyy");
        }

        /// <summary>
        /// Author: Huy Vo
        /// Des: Get years and months from birthday to now
        /// </summary>
        /// <param name="dob"></param>
        public static string GetMonthsAndYears(this string dob)
        {
            string pattern = "d/M/yyyy";
            DateTime birthday = DateTime.ParseExact(dob, pattern, CultureInfo.InvariantCulture);
            DateTime dt = DateTime.Today;

            int days = dt.Day - birthday.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
            }

            int months = dt.Month - birthday.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - birthday.Year;

            return $"{years} Years {months} Month(s)";

        }

        /// <summary>
        /// Au : Hieu Pham
        /// Des : Return browser name in test runtime
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserName()
        {
            IWebDriver driver = WebContext.WebDriver;
            ICapabilities capabilities = ((RemoteWebDriver)driver).Capabilities;
            return capabilities.BrowserName;
        }

        /// <summary>
        /// Get academic name of date
        /// </summary>
        /// <param name="dateOfAcademic">Date in academic year which you want to get</param>
        /// <returns>Name of academic year</returns>
        public static string GetAcademicYear(DateTime dateOfAcademic)
        {
            if (dateOfAcademic == default(DateTime))
            {
                dateOfAcademic = DateTime.Now;
            }
            DateTime startAcademicDay = new DateTime(dateOfAcademic.Year, 8, 1);
            string academic;
            if (dateOfAcademic.Date.CompareTo(startAcademicDay) >= 0)
            {
                academic = String.Format("Academic Year {0}/{1}", dateOfAcademic.Year, dateOfAcademic.Year + 1);
            }
            else
            {
                academic = String.Format("Academic Year {0}/{1}", dateOfAcademic.Year - 1, dateOfAcademic.Year);
            }
            return academic;
        }

        /// <summary>
        /// Get start date of academic year
        /// </summary>
        /// <param name="dateOfAcademic">Date in academic year which you want to get</param>
        /// <returns>Start date of academic year</returns>
        public static DateTime GetStartDateAcademicYear(DateTime dateOfAcademic)
        {
            if (dateOfAcademic == default(DateTime))
            {
                dateOfAcademic = DateTime.Now;
            }
            DateTime startAcademicDayOfYear = new DateTime(dateOfAcademic.Year, 8, 1);
            DateTime startAcademicDay;
            if (dateOfAcademic.Date.CompareTo(startAcademicDayOfYear) >= 0)
            {
                startAcademicDay = startAcademicDayOfYear;
            }
            else
            {
                startAcademicDay = new DateTime(dateOfAcademic.Year - 1, 8, 1);
            }
            return startAcademicDay;
        }

        /// <summary>
        /// Get finish date of academic year
        /// </summary>
        /// <param name="dateOfAcademic">Date in academic year which you want to get</param>
        /// <returns>Finish date of academic year</returns>
        public static DateTime GetFinishDateAcademicYear(DateTime dateOfAcademic)
        {
            if (dateOfAcademic == default(DateTime))
            {
                dateOfAcademic = DateTime.Now;
            }
            DateTime finishAcademicDayOfYear = new DateTime(dateOfAcademic.Year, 7, 31);
            DateTime finishAcademicDay;
            if (dateOfAcademic.Date.CompareTo(finishAcademicDayOfYear) >= 0)
            {
                finishAcademicDay = finishAcademicDayOfYear;
            }
            else
            {
                finishAcademicDay = new DateTime(dateOfAcademic.Year - 1, 7, 31);
            }
            return finishAcademicDay;
        }

        #endregion

        #region Webix Functions

        /// <summary>
        /// Au: Logigear
        /// Des: Scroll Down the specific cell in table grid until it is visible
        /// Only use with custom table (Webx DataTable) in Capita
        /// </summary>
        /// <returns></returns>
        public static bool ScrollDownUntilName(IWebElement tableElement, By element, string name, bool containsMultipleWebix = false)
        {
            try
            {
                bool doesRepeatElementList = false;
                //List<IWebElement> ListWebElementFirst = tableElement.FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell")).ToList();
                List<IWebElement> listWebElementFirst = tableElement.FindElements(element).ToList();
                List<string> webElementsFirst = new List<string>();
                ConvertToListItem(listWebElementFirst, ref webElementsFirst);

                List<IWebElement> ListWebElementSecond;
                List<string> webElementsSecond = new List<string>();
                webElementsSecond.AddRange(webElementsFirst);

                bool isElementPresent = webElementsSecond.Contains(name);

                int currentPosition = 100;
                int stepScroll = 0;

                //string scripCommand = "$$(document.getElementsByClassName(\"webix_ss_vscroll webix_vscroll_y\")[0]).scrollTo({0},{1})";
                IWebElement scrollElement = null;
                string scripCommand;
                if (containsMultipleWebix)
                {
                    scripCommand = "$$(arguments[0]).scrollTo({0},{1})";
                    scrollElement = tableElement.FindElement(By.CssSelector(".webix_ss_vscroll.webix_vscroll_y"));
                }
                else
                {
                    scripCommand = "$$(document.getElementsByClassName(\"webix_ss_vscroll webix_vscroll_y\")[0]).scrollTo({0},{1})";
                }

                while (isElementPresent == false && doesRepeatElementList == false)
                {
                    if (containsMultipleWebix)
                    {
                        ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(String.Format(scripCommand, stepScroll, currentPosition), scrollElement);
                    }
                    else
                    {
                        ExecuteJavascript(String.Format(scripCommand, stepScroll, currentPosition));
                    }

                    currentPosition += 100;
                    //stepScroll += 100;

                    webElementsFirst.Clear();
                    webElementsFirst.AddRange(webElementsSecond);

                    ListWebElementSecond = FindElements(By.CssSelector(".webix_ss_left .read-only .webix_cell")).ToList();
                    ConvertToListItem(ListWebElementSecond, ref webElementsSecond);
                    doesRepeatElementList = DoesListItemEqual(webElementsFirst, webElementsSecond);
                    isElementPresent = webElementsSecond.Contains(name);
                }
                return isElementPresent;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Au: Logigear
        /// Des: Scroll Right the specific cell in table grid until it is visible
        /// Only use with custom table (Webx DataTable) in Capita
        /// </summary>
        /// <returns></returns>
        public static bool ScrollRightUntilExist(IWebElement tableElement, By element, bool containMultipleWebix = false)
        {
            bool isElementPresent = IsElementExists(element);
            int currentPosition = 0;
            int stepScroll = 100;
            //string scripCommand = "$$(document.getElementsByClassName(\"webix_ss_hscroll webix_vscroll_x\")[0]).scrollTo({0},{1})";

            string scripCommand;
            IWebElement scrollElement = null;
            if (containMultipleWebix)
            {
                scripCommand = "$$(arguments[0]).scrollTo({0},{1})";
                scrollElement = tableElement.FindElement(By.CssSelector(".webix_ss_hscroll.webix_vscroll_x"));
            }
            else
            {
                scripCommand = "$$(document.getElementsByClassName(\"webix_ss_hscroll webix_vscroll_x\")[0]).scrollTo({0},{1})";
            }

            while (isElementPresent == false)
            {
                if (containMultipleWebix)
                {
                    ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(String.Format(scripCommand, stepScroll, currentPosition), scrollElement);
                }
                else
                {
                    ExecuteJavascript(String.Format(scripCommand, stepScroll, currentPosition));
                }
                //currentPosition += 100;
                stepScroll += 100;

                isElementPresent = IsElementExists(element);
            }
            return true;
        }

        public static void ScrollToTopLeftTable(IWebElement tableElement)
        {

        }
        #endregion

        #region Footer Home Page
        public static void ClickFooterTab(string tabName)
        {
            switch (tabName.ToLower())
            {
                case "home page":
                    FindElement(By.CssSelector("[data-automation-id='home_page_tab']>a")).ClickByJS();

                    Wait.WaitForAjaxReady(By.Id("nprogress"));
                    Wait.WaitForDocumentReady();
                    break;
            }
        }
        #endregion

        #region Permission Checks

        public static bool HasMenuPermission(string menuDataAutomationId, string[] featureList = null, iSIMSUserType userType = iSIMSUserType.TestUser, bool enableSelection=true)
        {
            if (string.IsNullOrEmpty(menuDataAutomationId))
                return false;

            if (featureList == null)
                featureList = new[] { string.Empty };

            Login(userType, enabledFeatures: featureList, schoolSelection: enableSelection);
            var menuItem = WebContext.WebDriver.FindElements(By.CssSelector($"[data-automation-id='{menuDataAutomationId}']"));
            return menuItem != null && menuItem.Count == 1;
        }

        #endregion
    }
}
