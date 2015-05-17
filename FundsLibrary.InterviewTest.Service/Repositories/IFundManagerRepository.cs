using System;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;

namespace FundsLibrary.InterviewTest.Service.Repositories
{
    public interface IFundManagerRepository
    {
        Task<FundManager> GetBy(Guid id);
        Task<IQueryable<FundManager>> GetAll();
        Task<PageData<FundManager>> GetPageData(int pageSize, int pageNo, string orderByField = null, bool ascending = false);
        void Update(Guid id, FundManager fundManager);
        void Delete(Guid id);
        Guid Create(FundManager fundManager);
    }
}
