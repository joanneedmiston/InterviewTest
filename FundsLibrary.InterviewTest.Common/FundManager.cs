using System;

namespace FundsLibrary.InterviewTest.Common
{
    public class FundManager
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ManagedSince { get; set; }
        public string Biography { get; set; }
        public Location Location { get; set; }
    }

    public enum Location
    {
        Amsterdam = 1,
        Frankfurt = 2,
        London = 3,
        Luxembourg = 4,
        NewYork = 5,
        Zurich = 6
    }

}
