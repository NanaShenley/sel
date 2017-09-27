using OpenQA.Selenium;

namespace DataExchange.POM.Components.Common
{
    public struct DeniConstants
    {        
        public static By DetailedScreenFinaliseLoaded = By.CssSelector("form[id='editableData'] > [role='tablist'] > :nth-child(2)>div>h4>a>span");
        public static By DetailedScreenCBALoaded = By.CssSelector("Form[Id='editableData']> div > div:nth-child(2)>div>h4>a>span");
       
        public static By ProjectedPupilNumbers = By.CssSelector("form[id='editableData'] > [role='tablist']>div:nth-child(2)>div>h4>a>span:nth-child(1)");
    
        public static By SearchCriteria =By.CssSelector(".search-criteria-form");
        public static By ReturnVersionSelector = By.CssSelector(".search-criteria-form>div:nth-child(1)>div>div:nth-child(3)>div>div>input");

        public static By ValidateButton = By.CssSelector("a[title='Re-collect data and Validate']");
        public static By FinaliseButton = By.CssSelector("a[title='Finalise Return']");
        public static By SignOffButton = By.CssSelector("button[title='Sign Off Return']");
    }
}
