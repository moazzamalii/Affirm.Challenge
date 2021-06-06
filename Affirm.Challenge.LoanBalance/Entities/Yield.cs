using System;
using System.Collections.Generic;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Entities
{
    public class Yield
    {
        public int facility_id { get; set; }
        public decimal ?  expected_yield { get; set; }
    }
}
