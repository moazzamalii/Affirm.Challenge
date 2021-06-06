using Affirm.Challenge.LoanBalance.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public interface IYieldRepository
    {
        void add(Yield yield);
        void writeOutput(FileInfo fileInfo );
    }
}
