using FundsLibrary.InterviewTest.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundsLibrary.InterviewTest.Web.UnitTests.Controllers
{
    public class ErrorControllerTest
    {
        [Test]
        public async void ShouldGetEntityNotFoundPage()
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            var response = new Mock<HttpResponseBase>();
            mockHttpContext.SetupGet(x => x.Response).Returns(response.Object);

            var controller = new ErrorController()
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            var result = controller.EntityNotFound("test entity");

            response.VerifySet(x => x.StatusCode = 404);

            Assert.That(result, Is.TypeOf<ViewResult>());
            Assert.That(((ViewResult)result).Model, Is.EqualTo("test entity"));
        }
    }
}
