using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductManagement;
using ProductManagement.Controllers;

namespace ProductManagement.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            ProductController controller = new ProductController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create()
        {
            ProductController controller = new ProductController();

            ViewResult result = controller.Create() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
