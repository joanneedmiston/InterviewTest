using System;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;
using FundsLibrary.InterviewTest.Service.Controllers;
using FundsLibrary.InterviewTest.Service.Repositories;
using Moq;
using NUnit.Framework;

namespace FundsLibrary.InterviewTest.Service.UnitTests.Controllers
{
    [TestFixture]
    public class FundManagerControllerTests
    {
        [Test]
        public async Task ShouldGet()
        {
            var mock = new Mock<IFundManagerRepository>();
            var controller = new FundManagerController(mock.Object);
            var newGuid = Guid.NewGuid();
            var fundManager = new FundManager();
            mock.Setup(m => m.GetBy(newGuid)).Returns(Task.FromResult(fundManager));

            var result = controller.Get(newGuid);

            mock.Verify();
            Assert.That(await result, Is.EqualTo(fundManager));
        }

        [Test]
        public async Task ShouldGetAll()
        {
            var mock = new Mock<IFundManagerRepository>();
            var controller = new FundManagerController(mock.Object);
            var valueFunction = new[] { new FundManager() }.AsQueryable();
            mock.Setup(m => m.GetAll()).Returns(Task.FromResult(valueFunction));

            var result = await controller.Get();

            Assert.That(result.Count(), Is.EqualTo(1));
        }


        [Test]
        public async Task ShouldGetPageDataUnsorted()
        {
            var mock = new Mock<IFundManagerRepository>();
            var controller = new FundManagerController(mock.Object);
            var pageData = new PageData<FundManager>
            {
                TotalRowCount = 4,
                PageRows = new[] { new FundManager(), new FundManager(), new FundManager() }.AsQueryable()
            };

            mock.Setup(m => m.GetPageData(3, 1, null, true)).Returns(Task.FromResult(pageData));

            var result = await controller.Get(3, 1);

            Assert.That(result.TotalRowCount, Is.EqualTo(4));
            Assert.That(result.PageRows.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task ShouldGetPageDataSorted()
        {
            var mock = new Mock<IFundManagerRepository>();
            var controller = new FundManagerController(mock.Object);
            var pageData = new PageData<FundManager>
            {
                TotalRowCount = 4,
                PageRows = new[] { new FundManager(), new FundManager(), new FundManager() }.AsQueryable()
            };

            mock.Setup(m => m.GetPageData(3, 1, "Name", false)).Returns(Task.FromResult(pageData));

            var result = await controller.Get(3, 1, "Name", false);

            Assert.That(result.TotalRowCount, Is.EqualTo(4));
            Assert.That(result.PageRows.Count(), Is.EqualTo(3));
        }

    }
}
