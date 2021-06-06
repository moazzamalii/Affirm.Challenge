using Affirm.Challenge.LoanBalance.Entities;
using Affirm.Challenge.LoanBalance.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Services
{
    public interface ILoanBalanceService
    {
         int assign(Loan loan);
        decimal calculateExpectedYield(Loan loan, facility facility);
        void writeOutput(FileInfo assignmentFileInfo, FileInfo yieldFileInfo);
    }
}
