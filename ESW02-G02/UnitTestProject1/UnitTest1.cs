using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

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
            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Identity/Account/Register");

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
                mail = "manuel" + i + "@hotmail.com";
                time += RegisterTime(mail); 
            }
            time = time / 10;
            NUnit.Framework.TestContext.WriteLine("Média do tempo decorrido: " + time);
            NUnit.Framework.Assert.LessOrEqual(time, 3);
        }

        private long LoginTime(string mail)
        {
            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Identity/Account/Login");

            Stopwatch s = Stopwatch.StartNew();
            Login(mail, "Boasbro12?");
            s.Stop();
            return s.ElapsedMilliseconds / 100;
        }

        private void Login(string mail, string pass)
        {
            IWebElement email = driver.FindElement(By.Id("Login-Email"));
            email.SendKeys(mail);
            IWebElement password = driver.FindElement(By.Id("Login-Password"));
            password.SendKeys(pass);

            IWebElement submit = driver.FindElement(By.Id("Login-Submit"));
            submit.Submit();
        }

        [Test]
        public void Login10TimeTest()
        {
            long time = 0;
            string mail = "";

            for (int i = 0; i < 10; i++)
            {
                mail = "manuel" + i + "@hotmail.com";
                time += LoginTime(mail);
            }
            time = time / 10;
            NUnit.Framework.TestContext.WriteLine("Média do tempo decorrido: " + time);
            NUnit.Framework.Assert.LessOrEqual(time, 5);
        }

        [Test]
        public void Create10EmpTest()
        {
            string mail = "";

            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Employees/Create");
            Login("admin@hotmail.com", "Qwe123!");

            for (int i = 0; i < 10; i++)
            {
                mail = "manuel" + i + "@hotmail.com";
                CreateEmpAux(mail);
            }

            NUnit.Framework.Assert.Pass();

        }

        private void CreateEmpAux(string mail)
        {
            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Employees/Create");

            IWebElement user = driver.FindElement(By.Id("AccountID-Sel"));
            user.SendKeys(mail);

            IWebElement type = driver.FindElement(By.Id("user-type"));
            type.SendKeys("Funcionario");

            IWebElement info = driver.FindElement(By.Id("aditional-info"));
            info.SendKeys("info");

            IWebElement btn = driver.FindElement(By.Id("Employee-Create-submit"));
            btn.Submit();
        }

        [Test]
        public void Edit5EmpTest()
        {
            string mail = "";

            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Employees");
            Login("admin@hotmail.com", "Qwe123!");

            for (int i = 1; i < 6; i++)
            {
                mail = "teste" + i + "@hotmail.com";
                EditEmpAux(mail);
            }

            NUnit.Framework.Assert.Pass();
        }

        private void EditEmpAux(string mail)
        {
            driver.Navigate().GoToUrl("https://projeto-esw.azurewebsites.net/Employees");

            IWebElement link = driver.FindElement(By.Id(mail + " edit"));
            link.Click();

            IWebElement info = driver.FindElement(By.Id("edit-info"));
            info.Clear();
            info.SendKeys("updated");

            IWebElement btn = driver.FindElement(By.Id("edit-submit"));
            btn.Submit();
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}

