using Affirm.Challenge.LoanBalance.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public interface  IAssignmentRepository
    {

        void add(Assignment assignment);
        void writeOutPut(FileInfo fileInfo);
    }
}
