using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IWebDriver driver = new ChromeDriver();

            // Open website
            driver.Get("https://quintamiaoesw02-g02.azurewebsites.net/");

            // Find the link to registration form
            IWebElement link = driver.FindElement(By.Id("Registar"));

            // Click the link
            link.Click();

            // Find the email form field
            IWebElement name = driver.FindElement(By.Id("Registar-name"));
            name.SendKeys("Manuel");
            IWebElement address = driver.FindElement(By.Id("Registar-address"));
            address.SendKeys("Rua");
            IWebElement date = driver.FindElement(By.Id("Registar-date"));
            address.SendKeys("5/1/2008 8:30:52 AM");
            IWebElement type = driver.FindElement(By.Id("Registar-user-type"));
            address.SendKeys("Funciario");
            IWebElement email = driver.FindElement(By.Id("Registar-email"));
            address.SendKeys("manuel@hotmail.com");
            IWebElement password = driver.FindElement(By.Id("Registar-password"));
            address.SendKeys("123");
            IWebElement confirm = driver.FindElement(By.Id("Registar-confirm"));
            address.SendKeys("123");

            IWebElement submit = driver.FindElement(By.Id("Registar-submit"));
            submit.Submit();
/*
            // Check the sign up succeeded by checking that the randomized
            // email appears in the website's header bar.
            (new OpenQA.Selenium.Support.UI.WebDriverWait(driver, 10)).until(new ExpectedCondition<Boolean>()
            {
            public Boolean apply(WebDriver d)
            {
                WebElement header = d.findElement(By.id("header-login"));
                return header.getText().contains(randomEmail);
            }
        });
        */
        //Close the browser
        driver.quit();
    }
}
    }
}
