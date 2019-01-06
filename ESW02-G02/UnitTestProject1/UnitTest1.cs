using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
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

        
        private long RegisterTime(string mail)
        {
            driver.Navigate().GoToUrl("https://quinta-miao-02.azurewebsites.net/Identity/Account/Register");

            // Find the email form field
            IWebElement name = driver.FindElement(By.Id("Registar-name"));
            name.SendKeys("Manuel");
            IWebElement date = driver.FindElement(By.Id("Registar-date"));
            date.SendKeys("05/01/1997");
            IWebElement type = driver.FindElement(By.Id("Registar-user-type"));
            type.SendKeys("Funciario");
            IWebElement email = driver.FindElement(By.Id("Registar-email"));
            email.SendKeys(mail);
            IWebElement password = driver.FindElement(By.Id("Registar-password"));
            password.SendKeys("Boasbro12?");
            IWebElement confirm = driver.FindElement(By.Id("Registar-confirm"));
            confirm.SendKeys("Boasbro12?");

            IWebElement submit = driver.FindElement(By.Id("Registar-submit"));
            Stopwatch s = Stopwatch.StartNew();
            submit.Submit();
            s.Stop();
            return s.ElapsedMilliseconds / 100; 
        }

        [Test]
        public void Register10TimeTest()
        {
            long time = 0;
            string mail = "";

            for (int i = 0; i < 10; i++)
            {
                mail = "Jose" + i + "@hotmail.com";
                time += RegisterTime(mail); 
            }
            time = time / 10;
            NUnit.Framework.TestContext.WriteLine("Média do tempo decorrido: " + time);
            NUnit.Framework.Assert.LessOrEqual(time, 3);
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}

