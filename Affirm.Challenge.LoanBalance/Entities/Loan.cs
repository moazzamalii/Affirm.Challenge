using System;
using System.Collections.Generic;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Entities
{
    public class Loan
    {
        public int id { get; set; }
        public decimal? amount { get; set; }
        public float? interest_rate { get; set; }
        public float? default_likelihood { get; set; }
        public string state { get; set; }
    }
}
