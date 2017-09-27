using System;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace SharedComponents.CRUD
{
	public static class ControlsHelper
	{
		public static void UpdateValue(string css, object value)
		{
			if (css.Contains("dropdownImitator"))
			{
				SeleniumHelperObsolete.FindAndSetValueForSelector(css, value.ToString());
			}
			else
			{
				var x = BaseSeleniumComponents.WaitForAndGet(new TimeSpan(0, 0, 0, 5), By.CssSelector(css));

				if (x.Displayed)
				{
					SeleniumHelperObsolete.FindAndSetValue(css, value.ToString());
				}
				else
				{
					if (x.GetAttribute("data-tristate-checkbox-value") != null)
					{
						var c =
							WebContext.WebDriver.FindElement(
								By.CssSelector(string.Format("#{0}", x.GetAttribute("name"))));

						c.Click();
					}
				}
			}
		}

		public static void ClickCheckBox(string css, object value)
		{

		}
	}
}