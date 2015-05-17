using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FundsLibrary.InterviewTest.Web.Repositories;
using FundsLibrary.InterviewTest.Web.Models;
using FundsLibrary.InterviewTest.Web.ViewModels;


namespace FundsLibrary.InterviewTest.Web.Controllers
{
    public class FundManagerController : Controller
    {
        private readonly IFundManagerModelRepository _repository;

        // ReSharper disable once UnusedMember.Global
        public FundManagerController()
            : this(new FundManagerModelRepository())
        {}

        public FundManagerController(IFundManagerModelRepository repository)
        {
            _repository = repository;
        }

        // GET: FundManager
        public async Task<ActionResult> Index(int page = 1, string orderByField = null, bool ascending = true)
        {
            ViewBag.CurrentOrderByField = orderByField;
            ViewBag.CurrentOrderAscending = ascending;

            var pageSize = 3;
            var pageData = await _repository.GetPageData(pageSize, page, orderByField, ascending);

            return View(new FundManagerPagedDataViewModel
            {
                PageCount = (int)Math.Ceiling((decimal)pageData.TotalRowCount / pageSize),
                PageNumber = page,
                PageRows = pageData.PageRows

            });

        }


        // GET: FundManager/Details/id
        public async Task<ActionResult> Details(string id)
        {
            FundManagerModel manager = null;

            Guid managerId;
            if (Guid.TryParse(id, out managerId))
            {
                manager = await _repository.Get(managerId);
            }

            if (manager != null)
            {
                return View(manager);
            }
            else
            {
                return RedirectToAction("EntityNotFound", "Error", new { entityName = "Fund Manager"});   
            }
        }
    }
}
