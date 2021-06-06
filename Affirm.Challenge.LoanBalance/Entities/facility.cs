using System;
using System.Collections.Generic;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Entities
{
    public class facility
    {
      
       public int bank_id { get; set; }
        public int id { get; set; }
        public float? interest_rate { get; set; }
        public decimal? amount { get; set; }
        public List<Covenant> covenants { get; set; }

        internal void setAmount(decimal newAmount)
        {
            throw new NotImplementedException();
        }
    }
}
