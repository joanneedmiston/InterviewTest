using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FundsLibrary.InterviewTest.Web.Models;

namespace FundsLibrary.InterviewTest.Web.ViewModels
{
    public class FundManagerPagedDataViewModel
    {
        public IEnumerable<FundManagerModel> PageRows { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
    }
}