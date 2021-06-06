using Affirm.Challenge.LoanBalance.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Services
{
    public class LoanStream : ILoanStream
    {
        private List<Loan> loans;
        public LoanStream()
        {
            this.loans = new List<Loan>();
        }


        public virtual List<Loan> getLoanStream(FileInfo fileInfo)
        {
            FileStream fs = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                var records = csv.GetRecords<Loan>();
                loans.AddRange(records);
            }

            return loans;
        }
    }
}
