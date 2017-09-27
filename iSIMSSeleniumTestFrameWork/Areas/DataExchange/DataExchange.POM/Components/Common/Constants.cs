using OpenQA.Selenium;

namespace DataExchange.POM.Components.Common
{
    public static class Constants
    {
        public static By BasicDetails = By.LinkText("Basic Details");
        public static By Address = By.LinkText("Address");
        public const string EditMarkSecurable = "EditMarkSecurable";
        public const string GenericUiFeatures = "GenericUIFeatures";
        public const string NextPreviousFeature = "NextPreviousFeature";
        public const string AutoRefresh = "AutoRefresh";
        public const string CBAAutoRefresh = "CBAAutoRefresh";
        public const string CTFAutoRefresh = "CTFAutoRefresh";
        public const string CTFImportAutoRefresh = "CTFImportAutoRefresh";
        public const string CTFExportPupilSelection = "CTFExportPupilSelection";
        public const string DeniAutoRefresh = "DeniAutoRefresh";
        public const string DeniValidate = "DeniValidate";
        public const string DeniFinalise = "DeniFinalise";
        public const string Export = "Export";
        public const string LOPAutoRefresh = "LOPAutoRefresh";
        public const string LopImportAutoRefresh = "LopImportAutoRefresh";
        public const string FuturePupilAdditionalColumn = "FuturePupilAdditionalColumn";
        public const string AllocatePupilAdditionalColumn = "AllocatePupilAdditionalColumn";
        public const string AdditionalColumnSecurables = "AdditionalColumnSecurables";
        public const string LookupCodeAndDescriptionUnique = "LookupCodeAndDescriptionUnique";
        public static By PrimaryClassToFind = By.CssSelector("input[class='checkbox']");
        public static By SearchButton = By.LinkText("Search");
        public static By PalletteSearch = By.XPath("(//button[@type='submit'])[2]");
        public static By ProcessingMessage = By.CssSelector(".media-heading");
        public static By AddAllButton = By.CssSelector("button[title=\"Add All\"]");
        public static By MainScreen = By.CssSelector(".header-text");
        public static By CTFImportMainScreen = By.CssSelector(".section-header-title > span:nth-child(2)");
        public static By _DataOut { get; set; }
        public static By _ctfExportButton { get; set; }
        public const string SaveBtn = "[title=\"Save Record\"]";
        public static By DetailedScreenLoaded = By.CssSelector("form[id='editableData'] > [role='tablist'] > :nth-child(1)>div>h4>a>span");
        public const string SearchButtonToFind = "button[type='submit']";
        public const string CTFExport = "CTFExport";
    }

    public static class ReturnVersion
    {
        public const string DENI2015 = "DENI2015";
        public const string Spring2017 = "Spring2017";
        public const string Summer2017 = "Summer2017";
        public const string Autumn2017 = "Autumn2017";
    }

    public static class BugPriority
    {
        public const string P1 = "P1";
        public const string P2 = "P2";
        public const string P3 = "P3";
        public const string P4 = "P4";
    }
}
