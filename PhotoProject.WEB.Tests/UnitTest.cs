using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoProject.WEB.Controllers;
using System.Web.Mvc;

namespace PhotoProject.WEB.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void HomeTest()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("", result.ViewName);
        }
    }
}
