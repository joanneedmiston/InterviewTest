using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;
using FundsLibrary.InterviewTest.Web.Models;
using FundsLibrary.InterviewTest.Web.Models.Mappers;
using FundsLibrary.InterviewTest.Web.Repositories;
using Moq;
using NUnit.Framework;

namespace FundsLibrary.InterviewTest.Web.UnitTests.Repositories
{
    public class FundManagerModelRepositoryTests
    {
        [Test]
        public async Task ShouldGetAll()
        {
            var mockServiceClient = new Mock<IHttpClientWrapper>();
            var mockToFundManagerModelMapper = new Mock<IMapper<FundManager, FundManagerModel>>();
            var fundManagers = new[] { new FundManager() }.AsEnumerable();
            mockServiceClient
                .Setup(m => m.GetAndReadFromContentGetAsync<IEnumerable<FundManager>>("api/FundManager/"))
                .Returns(Task.FromResult(fundManagers));
            mockToFundManagerModelMapper
                .Setup(m => m.Map(It.IsAny<FundManager>()))
                .Returns(new FundManagerModel());
            var repository = new FundManagerModelRepository(
                mockServiceClient.Object,
                mockToFundManagerModelMapper.Object);

            var result = await repository.GetAll();

            mockToFundManagerModelMapper.Verify();
            mockServiceClient.Verify();
            Assert.That(result.Count(), Is.EqualTo(1));
        }

        [Test]
        public async void ShouldGet()
        {
            var mockServiceClient = new Mock<IHttpClientWrapper>();
            var mockToFundManagerModelMapper = new Mock<IMapper<FundManager, FundManagerModel>>();
            var fundManager = new FundManager();
            var guid = Guid.NewGuid();
            mockServiceClient
                .Setup(m => m.GetAndReadFromContentGetAsync<FundManager>("api/FundManager/" + guid))
                .Returns(Task.FromResult(fundManager));
            var fundManagerModel = new FundManagerModel();
            mockToFundManagerModelMapper
                .Setup(m => m.Map(It.IsAny<FundManager>()))
                .Returns(fundManagerModel);
            var repository = new FundManagerModelRepository(
                mockServiceClient.Object,
                mockToFundManagerModelMapper.Object);

            var result = await repository.Get(guid);

            mockToFundManagerModelMapper.Verify();
            mockServiceClient.Verify();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(fundManagerModel));
        }

        [Test]
        public async void ShouldGetNull()
        {
            var mockServiceClient = new Mock<IHttpClientWrapper>();
            var mockToFundManagerModelMapper = new Mock<IMapper<FundManager, FundManagerModel>>();
            var guid = Guid.NewGuid();
            FundManager fundManager = null;
            mockServiceClient
                .Setup(m => m.GetAndReadFromContentGetAsync<FundManager>("api/FundManager/" + guid))
                .Returns(Task.FromResult(fundManager));


            var repository = new FundManagerModelRepository(
                mockServiceClient.Object,
                mockToFundManagerModelMapper.Object);

            var result = await repository.Get(guid);

            mockToFundManagerModelMapper.Verify(s => s.Map(It.IsAny<FundManager>()), Times.Never());
            mockServiceClient.Verify();
            Assert.That(result, Is.Null);
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public async Task ShouldGetPageDataUnsorted()
        {
            var mockServiceClient = new Mock<IHttpClientWrapper>();
            var mockToFundManagerModelMapper = new Mock<IMapper<FundManager, FundManagerModel>>();
            var fundManagerPageData = new PageData<FundManager>
            {
                TotalRowCount = 4,
                PageRows = new[] { new FundManager(), new FundManager(), new FundManager() }.AsQueryable()
            };
            mockServiceClient
                .Setup(m => m.GetAndReadFromContentGetAsync<PageData<FundManager>>("api/FundManager/?pageSize=3&pageNo=1"))
                .Returns(Task.FromResult(fundManagerPageData));
            mockToFundManagerModelMapper
                .Setup(m => m.Map(It.IsAny<FundManager>()))
                .Returns(new FundManagerModel());
            var repository = new FundManagerModelRepository(
                mockServiceClient.Object,
                mockToFundManagerModelMapper.Object);

            var result = await repository.GetPageData(3, 1);

            mockToFundManagerModelMapper.Verify();
            mockServiceClient.Verify();
            Assert.That(result.PageRows.Count(), Is.EqualTo(3));
            Assert.That(result.TotalRowCount, Is.EqualTo(4));
        }

        [Test]
        public async Task ShouldGetPageDataSorted()
        {
            var mockServiceClient = new Mock<IHttpClientWrapper>();
            var mockToFundManagerModelMapper = new Mock<IMapper<FundManager, FundManagerModel>>();
            var fundManagerPageData = new PageData<FundManager>
            {
                TotalRowCount = 4,
                PageRows = new[] { new FundManager(), new FundManager(), new FundManager() }.AsQueryable()
            };
            mockServiceClient
                .Setup(m => m.GetAndReadFromContentGetAsync<PageData<FundManager>>("api/FundManager/?pageSize=3&pageNo=1&orderByField=Name&ascending=True"))
                .Returns(Task.FromResult(fundManagerPageData));
            mockToFundManagerModelMapper
                .Setup(m => m.Map(It.IsAny<FundManager>()))
                .Returns(new FundManagerModel());
            var repository = new FundManagerModelRepository(
                mockServiceClient.Object,
                mockToFundManagerModelMapper.Object);

            var result = await repository.GetPageData(3, 1, "Name");

            mockToFundManagerModelMapper.Verify();
            mockServiceClient.Verify();
            Assert.That(result.PageRows.Count(), Is.EqualTo(3));
            Assert.That(result.TotalRowCount, Is.EqualTo(4));
        }

    }
}
