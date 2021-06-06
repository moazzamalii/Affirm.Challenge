using Affirm.Challenge.LoanBalance.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Services
{
    public interface ILoanStream
    {
        List<Loan> getLoanStream(FileInfo fileInfo);
    }
}
