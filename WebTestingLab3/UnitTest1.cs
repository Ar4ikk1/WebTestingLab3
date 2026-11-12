using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace WebTesting;

public class Tests
{
    private EdgeDriver driver;
    private const string url = "https://the-internet.herokuapp.com/add_remove_elements/";
    
    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver();
        
        driver.Manage().Window.Maximize();
    }

    [Test]
    public void AddButtonTask1()
    {
        driver.Navigate().GoToUrl(url);
        
        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/button")).Click();

        var deleteButton = driver.FindElement(By.XPath("//*[@id=\"elements\"]/button"));
        
        Assert.That(deleteButton, Is.Not.Null);
    }
    
    [Test]
    public void DeleteButtonTask1()
    {
        driver.Navigate().GoToUrl(url);
        driver.FindElement(By.XPath("//*[@id=\"content\"]/div/button")).Click();
        driver.FindElement(By.XPath("//*[@id=\"elements\"]/button")).Click();
        bool isRemoved = driver.FindElements(By.XPath("//*[@id=\"elements\"]/button")).Count == 0;
        
        Assert.That(isRemoved, Is.True);
    }
    
    
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        
        driver.Dispose();
    }
}