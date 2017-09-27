using OpenQA.Selenium;

using SharedComponents.Helpers;

namespace Pupil.Components.Common
{
	public struct ClassLogElements
	{
		public struct Detail
		{
            public const string IndicatorDropDownList = ".layout-gallery-details .navbar-nav li:nth-child(6) > a";
            public const string ShowIndicator = ".layout-gallery-details .navbar-nav li:nth-child(6) ul > li:last-child input";
		}
	}
}
