using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace InstagramBot
{
    class ActionClass
    {
        private string userName;
        private string password;
        private IWebDriver chromeDriver;

        public ActionClass(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
            this.chromeDriver = new ChromeDriver();
            chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        }

        public void Initiate()
        {
            chromeDriver.Navigate().GoToUrl("https://instagram.com");
            chromeDriver.FindElement(By.XPath("//input[@name='username']")).SendKeys(userName);
            chromeDriver.FindElement(By.XPath("//input[@name='password']")).SendKeys(password);
            chromeDriver.FindElement(By.XPath("//button[@type='submit']")).Click();
            chromeDriver.FindElement(By.XPath("//button[contains(text(), 'Not Now')]")).Click();
            chromeDriver.FindElement(By.XPath("//button[contains(text(), 'Not Now')]")).Click();
        }

        #region Public Methods

        public List<string> GetNoneFollowers()
        {
            chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/nav/div[2]/div/div/div[1]/a")).Click();
            chromeDriver.FindElement(By.XPath($"//a[contains(@href,'/{userName}')]")).Click();
            chromeDriver.FindElement(By.XPath($"//a[contains(@href,'/following')]")).Click();
            List<string> followingList = GetNames();
            chromeDriver.FindElement(By.XPath($"//a[contains(@href,'/followers')]")).Click();
            List<string> followersList = GetNames();
            Thread.Sleep(500);
            List<string> noneFollowers = followingList.Except(followersList).ToList();
            return noneFollowers;
        }

        public void Logout()
        {
            chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/nav/div[2]/div/div/div[1]/a")).Click();
            chromeDriver.FindElement(By.XPath($"//a[contains(@href,'/{userName}')]")).Click();
            chromeDriver.FindElement(By.XPath("/html/body/div[1]/section/main/div/header/section/div[1]/div/button")).Click();
            chromeDriver.FindElement(By.XPath("//button[contains(text(), 'Log Out')]")).Click();
        }

        #endregion

        #region Private Helper Methods

        private List<string> GetNames()
        {
            var scrollBox = chromeDriver.FindElement(By.XPath("/html/body/div[4]/div/div/div[2]"));
            ScrollToBottom(scrollBox);

            var links = scrollBox.FindElements(By.TagName("a"));
            var listOfNames = new List<string>();
            foreach(var entry in links)
            {
                if (!entry.Text.Equals(null) && !entry.Text.Equals(""))
                {
                    listOfNames.Add(entry.Text.ToString());
                }
            }
            Thread.Sleep(500);
            chromeDriver.FindElement(By.XPath("/html/body/div[4]/div/div/div[1]/div/div[2]/button")).Click();
            return listOfNames;
        }

        private void ScrollToBottom(IWebElement scrollBox)
        {
            long height = 0;
            do
            {
                IJavaScriptExecutor jsExec = (IJavaScriptExecutor)chromeDriver;
                var newHeight = (long)jsExec.ExecuteScript("arguments[0].scrollTo(0, arguments[0].scrollHeight); return arguments[0].scrollHeight;", scrollBox);

                if (newHeight == height)
                    break;
                else
                {
                    height = newHeight;
                    Thread.Sleep(500);
                }
            } while (true);
        }

        #endregion
    }
}
