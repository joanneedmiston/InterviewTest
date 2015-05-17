using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundsLibrary.InterviewTest.Common
{
    public class PageData<TEntity> where TEntity : class
    {
        public int TotalRowCount { get; set; }
        public IEnumerable<TEntity> PageRows { get; set; }
    }
}
