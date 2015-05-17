using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using FundsLibrary.InterviewTest.Web.Controllers;
using FundsLibrary.InterviewTest.Web.Models;
using FundsLibrary.InterviewTest.Web.Repositories;
using Moq;
using NUnit.Framework;
using FundsLibrary.InterviewTest.Common;
using FundsLibrary.InterviewTest.Web.ViewModels;

namespace FundsLibrary.InterviewTest.Web.UnitTests.Controllers
{
    public class FundManagerControllerTests
    {
        [Test]
        public async void ShouldGetIndexPage()
        {
            var mock = new Mock<IFundManagerModelRepository>();
            var fundManagerModels = new PageData<FundManagerModel>
            {
                PageRows = new[] { new FundManagerModel(), new FundManagerModel(), new FundManagerModel() },
                TotalRowCount = 7
            };
            mock.Setup(m => m.GetPageData(3, 1, null, true)).Returns(Task.FromResult(fundManagerModels));
            var controller = new FundManagerController(mock.Object);

            var result = await controller.Index(1);

            Assert.That(result, Is.TypeOf<ViewResult>());
            mock.Verify();

            var viewModel = (FundManagerPagedDataViewModel)(((ViewResult)result).Model);
            Assert.That(viewModel.PageCount, Is.EqualTo(3));
            Assert.That(viewModel.PageNumber, Is.EqualTo(1));
            Assert.That(viewModel.PageRows.Count(), Is.EqualTo(3));
        }

        [Test]
        public async void ShouldGetDetailsPage()
        {
            var guid = Guid.NewGuid();
            var mock = new Mock<IFundManagerModelRepository>();
            var fundManagerModel = new FundManagerModel();
            mock.Setup(m => m.Get(guid)).Returns(Task.FromResult(fundManagerModel));
            var controller = new FundManagerController(mock.Object);

            var result = await controller.Details(guid.ToString());

            Assert.That(result, Is.TypeOf<ViewResult>());
            mock.Verify();
            Assert.That(((ViewResult)result).Model, Is.EqualTo(fundManagerModel));
        }


    }
}
