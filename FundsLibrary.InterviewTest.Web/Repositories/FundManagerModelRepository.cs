using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundsLibrary.InterviewTest.Common;
using FundsLibrary.InterviewTest.Web.Models;
using FundsLibrary.InterviewTest.Web.Models.Mappers;

namespace FundsLibrary.InterviewTest.Web.Repositories
{
    public interface IFundManagerModelRepository
    {
        Task<IEnumerable<FundManagerModel>> GetAll();
        Task<PageData<FundManagerModel>> GetPageData(int pageSize = 0, int pageNo = 0,
             string orderByField = null, bool ascending = true);
        Task<FundManagerModel> Get(Guid id);
    }

    public class FundManagerModelRepository : IFundManagerModelRepository
    {
        private const string _serviceAppUrl = "http://localhost:50135/Service/";

        private readonly IHttpClientWrapper _client;
        private readonly IMapper<FundManager, FundManagerModel> _toModelMapper;

        public FundManagerModelRepository(
            IHttpClientWrapper client = null,
            IMapper<FundManager, FundManagerModel> toModelMapper = null)
        {
            _client = client ?? new HttpClientWrapper(_serviceAppUrl);
            _toModelMapper = toModelMapper ?? new ToFundManagerModelMapper();
        }

        public async Task<IEnumerable<FundManagerModel>> GetAll()
        {
            var managers = await _client.GetAndReadFromContentGetAsync<IEnumerable<FundManager>>("api/FundManager/");
            return managers.Select(s => _toModelMapper.Map(s));
        }

        public async Task<PageData<FundManagerModel>> GetPageData(int pageSize = 0, int pageNo = 0,
            string orderByField = null, bool ascending = true)
        {
            var url = string.Format("api/FundManager/?pageSize={0}&pageNo={1}", pageSize, pageNo);
            if (orderByField != null)
            {
                url += string.Format("&orderByField={0}&ascending={1}", orderByField, ascending);
            }

            var sourcePageData = await _client.GetAndReadFromContentGetAsync<PageData<FundManager>>(url);

            var pageData = new PageData<FundManagerModel>()
            {
                TotalRowCount = sourcePageData.TotalRowCount,
                PageRows = sourcePageData.PageRows.Select(f => _toModelMapper.Map(f))
            };

            return pageData;

        }


        public async Task<FundManagerModel> Get(Guid id)
        {
            var manager = await _client.GetAndReadFromContentGetAsync<FundManager>("api/FundManager/" + id);
            return manager != null ? _toModelMapper.Map(manager) : null;
        }
    }
}
