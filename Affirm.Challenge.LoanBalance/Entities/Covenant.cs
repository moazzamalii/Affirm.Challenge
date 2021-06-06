using System;
using System.Collections.Generic;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Entities
{
    public class Covenant
    {
        public int bank_id { get; set; }
        public int facility_id { get; set; }
        public float? max_default_likelihood { get; set; }
        public string banned_state { get; set; }

       
    }
}
    