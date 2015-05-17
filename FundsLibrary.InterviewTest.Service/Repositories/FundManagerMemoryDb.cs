using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;

namespace FundsLibrary.InterviewTest.Service.Repositories
{
    /*
	 * This class is the simplest possible implementation of the fund manager repository interface.
	 * A real world implementation would wrap SQL Server / Couchbase / ElasticSearch /etc.
	 */

    public class FundManagerMemoryDb : IFundManagerRepository
    {
        private static readonly ConcurrentDictionary<Guid, FundManager> _fundManagers = new ConcurrentDictionary<Guid, FundManager>();

        static FundManagerMemoryDb()
        {
            //fake data
            var items = new[]
            {
                new FundManager { Id = Guid.Parse("83eed26a-d1e3-4e78-8d3d-c4c9b63eb548"), Name = "A fund manager", Biography = "some bio goes here", Location = Location.Amsterdam, ManagedSince = DateTime.Now.AddYears(-3) },
                new FundManager { Id = Guid.Parse("11a12c9f-417f-4225-ba33-d05ce638b457"), Name = "Another fund manager", Biography = "some bio goes here", Location = Location.Frankfurt, ManagedSince = DateTime.Now.AddYears(-1) },
                new FundManager { Id = Guid.Parse("e5ada48d-bf76-4290-8e8a-c33b39e04d5c"), Name = "A third fund manager", Biography = "some bio goes here", Location = Location.Luxembourg, ManagedSince = DateTime.Now.AddYears(-4) },
                new FundManager { Id = Guid.Parse("b44aa473-3d20-4bc1-b234-7d12dd19f233"), Name = "A fourth fund manager", Biography = "some bio goes here", Location = Location.London, ManagedSince = DateTime.Now.AddYears(-2) },
                new FundManager { Id = Guid.Parse("317065b3-b366-43ae-8f12-2a29c60d451d"), Name = "A fifth fund manager", Biography = "some bio goes here", Location = Location.NewYork, ManagedSince = DateTime.Now.AddYears(-5) },
                new FundManager { Id = Guid.Parse("4d335f80-de41-4bce-9733-625deca0704f"), Name = "A sixth fund manager", Biography = "some bio goes here", Location = Location.Zurich, ManagedSince = DateTime.Now.AddYears(-7) },
                new FundManager { Id = Guid.Parse("11575c63-5cbb-4844-8c2c-bada70bf0305"), Name = "A seventh fund manager", Biography = "some bio goes here", Location = Location.London, ManagedSince = DateTime.Now.AddYears(-2) }
            };

            foreach (var fundManager in items)
                _fundManagers.TryAdd(fundManager.Id, fundManager);
        }

        public Task<FundManager> GetBy(Guid id)
        {
            FundManager value;
            _fundManagers.TryGetValue(id, out value); // If the key is not in the dictionary return null.

            return Task.FromResult(value);
        }

        public Task<IQueryable<FundManager>> GetAll()
        {
            return Task.FromResult(_fundManagers.Values.AsQueryable());
        }

        public Task<PageData<FundManager>> GetPageData(int pageSize = 0, int pageNo = 0,
    string orderByField = null, bool ascending = true)
        {
            var query = _fundManagers.Values.AsQueryable();
            var pageData = new PageData<FundManager>()
            {
                TotalRowCount = query.Count()
            };

            switch (orderByField)
            {
                case "Id":
                    query = ascending ? query.OrderBy(f => f.Id) : query.OrderByDescending(f => f.Id);
                    break;
                case "Name":
                    query = ascending ? query.OrderBy(f => f.Name) : query.OrderByDescending(f => f.Name);
                    break;
                case "ManagedSince":
                    query = ascending ? query.OrderBy(f => f.ManagedSince) : query.OrderByDescending(f => f.ManagedSince);
                    break;
                case "Biography":
                    query = ascending ? query.OrderBy(f => f.Biography) : query.OrderByDescending(f => f.Biography);
                    break;
                case "Location":
                    query = ascending ? query.OrderBy(f => f.Location) : query.OrderByDescending(f => f.Location);
                    break;
                default:
                    // need apply OrderyBy for paging to work
                    query = query.OrderBy(f => true);
                    break;
            }


            if (pageSize > 0 && pageNo > 0)
            {
                query = query.Skip((pageNo - 1) * pageSize).Take(pageSize);
            }

            pageData.PageRows = query.AsQueryable();

            return Task.FromResult(pageData);
        }

        public void Update(Guid id, FundManager fundManager)
        {
            _fundManagers[id] = fundManager;
        }

        public void Delete(Guid id)
        {
            FundManager value;
            _fundManagers.TryRemove(id, out value);
        }

        public Guid Create(FundManager fundManager)
        {
            fundManager.Id = Guid.NewGuid();
            if (!_fundManagers.TryAdd(fundManager.Id, fundManager))
                throw new Exception("Cannot add manager - another manager with the same ID already exists."); // Unlikely as it's as the key is a GUID.

            return fundManager.Id;
        }
    }
}
