using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;


namespace WebTesting;

public class Tests
{
    private EdgeDriver driver;
    private const string urlAddRemoveElements = "https://the-internet.herokuapp.com/add_remove_elements/";
    private const string urlCheckboxes = "https://the-internet.herokuapp.com/checkboxes";
    private const string urlDropdownList = "https://the-internet.herokuapp.com/dropdown";
    private const string urlInputs = "https://the-internet.herokuapp.com/inputs";
    private const string urlStatusCodes = "https://the-internet.herokuapp.com/status_codes";
    private const string urlDragAndDrop = "https://the-internet.herokuapp.com/drag_and_drop";
    private const string urlShiftingContent = "https://the-internet.herokuapp.com/shifting_content";
    private const string urlGeolocation = "https://the-internet.herokuapp.com/geolocation";
    private const string urlJavaScriptErrors = "https://the-internet.herokuapp.com/javascript_error";
    private const string urlExitIntent = "https://the-internet.herokuapp.com/exit_intent";
    
    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver();
        
        driver.Manage().Window.Maximize();
    }

    [Test]
    public void AddButtonTask1()
    {
        driver.Navigate().GoToUrl(urlAddRemoveElements);
        
        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/button")).Click();

        var deleteButton = driver.FindElement(By.XPath("//*[@id=\"elements\"]/button"));
        
        Assert.That(deleteButton.Displayed, Is.True);
    }
    
    [Test]
    public void DeleteButtonTask1()
    {
        driver.Navigate().GoToUrl(urlAddRemoveElements);
        
        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/button")).Click(); //Кнопка Add
        driver.FindElement(By.XPath("//*[@id=\"elements\"]/button")).Click(); //Кнопка Delete
        bool isRemoved = driver.FindElements(By.XPath("//*[@id=\"elements\"]/button")).Count == 0;
        
        Assert.That(isRemoved, Is.True);
    }

    [Test]
    public void CheckboxTask2()
    {
        driver.Navigate().GoToUrl(urlCheckboxes);

        var checkbox = driver.FindElement(By.XPath("//*[@id=\"checkboxes\"]/input[1]"));

        if (!checkbox.Selected)
        {
            checkbox.Click();
        }
        Assert.That(checkbox.Selected, Is.True);
    }

    [Test]
    public void DropdownListTask3()
    {
        driver.Navigate().GoToUrl(urlDropdownList);

        var dropdownList = new SelectElement(driver.FindElement(By.Id("dropdown")));
        dropdownList.SelectByText("Option 2");
        Assert.That(dropdownList.SelectedOption.Text, Is.EqualTo("Option 2"));
    }

    [Test]
    public void InputTextBoxTask4()
    {
        driver.Navigate().GoToUrl(urlInputs);

        var textBox = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/div/div/input"));
        textBox.Clear();
        textBox.SendKeys("123");
        Assert.That(textBox.GetAttribute("value"), Is.EqualTo("123"));
    }

    [Test]
    public void StatusCodesMultipleWinTask5()
    {
        driver.Navigate().GoToUrl(urlStatusCodes);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        
        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/p[1]/a")).Click();

        wait.Until(d => d.FindElement(By.XPath("//*[@id=\"http-status-codes\"]/h1")).Displayed);
        
        var title = driver.FindElement(By.XPath("//*[@id=\"http-status-codes\"]/h1"));
        
        Assert.That(title.Text, Is.EqualTo("Hypertext Transfer Protocol (HTTP) Status Code Registry"));
    }

    [Test]
    public void DragAndDropTask6()
    {
        driver.Navigate().GoToUrl(urlDragAndDrop);

        var blockA = driver.FindElement(By.Id("column-a"));
        var blockB = driver.FindElement(By.Id("column-b"));

        var actions = new Actions(driver);
        actions.ClickAndHold(blockA);
        actions.MoveToElement(blockB);
        actions.Release();
        actions.Perform();
        
        Assert.That(blockA.Text, Is.EqualTo("B"));
        Assert.That(blockB.Text, Is.EqualTo("A"));
    }

    [Test]
    public void ShiftingContentTask7()
    {
        driver.Navigate().GoToUrl(urlShiftingContent);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/a[2]")).Click();

        wait.Until(d => d.FindElement(By.XPath("//*[@id=\"content\"]/div/h3")));

        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/p[5]/a")).Click();
        var newImage = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/img"));
        
        Assert.That(newImage.Displayed, Is.True);
    }

    [Test]
    public void GeolocationTask8()
    {
        driver.Navigate().GoToUrl(urlGeolocation);

        var title = driver.FindElement(By.XPath("//*[@id=\"content\"]/div/h3"));
        
        Assert.That(title.Text, Is.EqualTo("Geolocation"));
    }

    [Test]
    public void JavaScriptErrorsTask9()
    {
        driver.Navigate().GoToUrl(urlJavaScriptErrors);
        
        var errors = driver.Manage().Logs.GetLog(LogType.Browser).Where(e => e.Level == LogLevel.Severe).ToList();
        
        Assert.That(errors, Is.Not.Empty);
    }

    [Test]
    public void ExitIntentTask10()
    {
        driver.Navigate().GoToUrl(urlExitIntent);
        
        Thread.Sleep(1500);
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "['mouseleave','mouseout'].forEach(t => document.documentElement" +
            ".dispatchEvent(new MouseEvent(t, {bubbles:true, clientY:-1})));");
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElement(By.XPath("//*[@id=\"ouibounce-modal\"]/div[2]")).Displayed);

        var window = driver.FindElement(By.XPath("//*[@id=\"ouibounce-modal\"]/div[2]/div[1]/h3"));
        
        Assert.That(window.Text, Is.EqualTo("THIS IS A MODAL WINDOW"));
        
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        
        driver.Dispose();
    }
}