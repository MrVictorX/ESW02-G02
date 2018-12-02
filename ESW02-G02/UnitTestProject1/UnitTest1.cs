using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        IWebDriver driver;

        [SetUp]
        public void StartBrowser()
        {
            driver = new ChromeDriver(@"C:\Users\victo\Documents\LEI\3ºAno\1ºSem\ESW\ESW02-G02\ESW02-G02\UnitTestProject1");
        }

        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("https://localhost:44384//Identity/Account/Register");

            // Find the email form field
            IWebElement name = driver.FindElement(By.Id("Registar-name"));
            name.SendKeys("Manuel");
            IWebElement address = driver.FindElement(By.Id("Registar-address"));
            address.SendKeys("Rua");
            IWebElement date = driver.FindElement(By.Id("Registar-date"));
            date.SendKeys("05/01/1997");
            IWebElement type = driver.FindElement(By.Id("Registar-user-type"));
            type.SendKeys("Funciario");
            IWebElement email = driver.FindElement(By.Id("Registar-email"));
            email.SendKeys("manuel@hotmail.com");
            IWebElement password = driver.FindElement(By.Id("Registar-password"));
            password.SendKeys("Boasbro12?");
            IWebElement confirm = driver.FindElement(By.Id("Registar-confirm"));
            confirm.SendKeys("Boasbro12?");

            IWebElement submit = driver.FindElement(By.Id("Registar-submit"));
            submit.Submit();
            Stopwatch s = Stopwatch.StartNew();

            string currentURL = driver.Url;

            if (currentURL.Equals("https://localhost:44384/"))
            {
                s.Stop();
                System.Console.WriteLine("Tempo decorrido: " + (s.ElapsedMilliseconds / 100));
                NUnit.Framework.Assert.Less(s.ElapsedMilliseconds, 3000);
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}

