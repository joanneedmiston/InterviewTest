using System;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;
using FundsLibrary.InterviewTest.Service.Repositories;
using NUnit.Framework;

namespace FundsLibrary.InterviewTest.Service.UnitTests.Repositories
{
    public class FundManagerMemoryDbTests
    {
        [Test]
        public async Task ShouldGetItems()
        {
            var repo = new FundManagerMemoryDb();

            var result = await repo.GetAll();

            //assert we have some data
            Assert.That(result.Count(), Is.GreaterThan(3));
        }

        [Test]
        public async Task ShouldGetById()
        {
            var repo = new FundManagerMemoryDb();
            var firstItem = (await repo.GetAll()).First();

            var result = await repo.GetBy(firstItem.Id);
            Assert.That(result, Is.EqualTo(firstItem));
        }

        [Test]
        public async Task ShouldAddItem()
        {
            var repo = new FundManagerMemoryDb();
            var beforeCount = (await repo.GetAll()).Count();
            var fundManager = new FundManager()
            {
                Name = "TestFundManager",
                Biography = "test bio",
                Location = Location.NewYork,
                ManagedSince = DateTime.Now.AddYears(-1)
            };

            repo.Create(fundManager);

            Assert.That(fundManager.Id, Is.Not.EqualTo(Guid.Empty));
            var afterCount = (await repo.GetAll()).Count();
            Assert.That(afterCount, Is.EqualTo(beforeCount + 1));
        }

        [Test]
        public async Task ShouldUpdateItem()
        {
            var repo = new FundManagerMemoryDb();
            var firstItem = (await repo.GetAll()).First();

            firstItem.Name = "NewName";
            repo.Update(firstItem.Id, firstItem);

            Assert.That((await repo.GetBy(firstItem.Id)).Name, Is.EqualTo("NewName"));
        }

        [Test]
        public async Task ShouldRemoveItem()
        {
            var repo = new FundManagerMemoryDb();
            var fundManagers = (await repo.GetAll());
            var beforeCount = fundManagers.Count();
            var firstItem = fundManagers.First();

            repo.Delete(firstItem.Id);

            var afterCount = (await repo.GetAll()).Count();
            Assert.That(afterCount, Is.EqualTo(beforeCount - 1));
        }

        [Test]
        public async Task ShouldGetFirstPageDataUnsorted()
        {
            var repo = new FundManagerMemoryDb();

            var testFundManagers = (await repo.GetAll()).ToList();
            var allRowsCount = testFundManagers.Count();
            var pageSize = allRowsCount - 1;

            var result = await repo.GetPageData(pageSize, 1);

            Assert.That(result.TotalRowCount, Is.EqualTo(allRowsCount));
            Assert.That(result.PageRows.Count(), Is.EqualTo(pageSize));
            Assert.That(result.PageRows.First().Id, Is.EqualTo(testFundManagers.First().Id));
            Assert.That(result.PageRows.Last().Id, Is.EqualTo(testFundManagers[pageSize - 1].Id));

        }

        [Test]
        public async Task ShouldGetLastPageDataUnsorted()
        {
            var repo = new FundManagerMemoryDb();

            var testFundManagers = (await repo.GetAll()).ToList();
            var allRowsCount = testFundManagers.Count();
            var pageSize = allRowsCount - 1;

            var result = await repo.GetPageData(pageSize, 2);

            Assert.That(result.TotalRowCount, Is.EqualTo(allRowsCount));
            Assert.That(result.PageRows.Count(), Is.EqualTo(1));
            Assert.That(result.PageRows.First().Id, Is.EqualTo(testFundManagers.Last().Id));

        }

        [Test]
        public async Task ShouldGetFirstPageDataSortedAscending()
        {
            var repo = new FundManagerMemoryDb();

            var testFundManagers = (await repo.GetAll()).OrderBy(f => f.Name).ToList();
            var allRowsCount = testFundManagers.Count();
            var pageSize = allRowsCount - 1;

            var result = await repo.GetPageData(pageSize, 1, "Name");

            Assert.That(result.TotalRowCount, Is.EqualTo(allRowsCount));
            Assert.That(result.PageRows.Count(), Is.EqualTo(pageSize));
            Assert.That(result.PageRows.First().Id, Is.EqualTo(testFundManagers.First().Id));
            Assert.That(result.PageRows.Last().Id, Is.EqualTo(testFundManagers[pageSize - 1].Id));

        }

        [Test]
        public async Task ShouldGetFirstPageDataSortedDescending()
        {
            var repo = new FundManagerMemoryDb();

            var testFundManagers = (await repo.GetAll()).OrderByDescending(f => f.ManagedSince).ToList();
            var allRowsCount = testFundManagers.Count();
            var pageSize = allRowsCount - 1;

            var result = await repo.GetPageData(pageSize, 1, "ManagedSince", false);

            Assert.That(result.TotalRowCount, Is.EqualTo(allRowsCount));
            Assert.That(result.PageRows.Count(), Is.EqualTo(pageSize));
            Assert.That(result.PageRows.First().Id, Is.EqualTo(testFundManagers.First().Id));
            Assert.That(result.PageRows.Last().Id, Is.EqualTo(testFundManagers[pageSize - 1].Id));

        }

    }
}
