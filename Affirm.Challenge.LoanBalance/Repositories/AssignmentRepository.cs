using Affirm.Challenge.LoanBalance.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {

        private List<Assignment> assignedLoan;
        public AssignmentRepository()
        {
            assignedLoan = new List<Assignment>();
        }

        public virtual void add(Assignment assignment)
        {
            this.assignedLoan.Add(assignment);

        }

        public void writeOutPut(FileInfo fileInfo)
        {
            FileStream fs = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (var writer = new StreamWriter(fs))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {                

                csvWriter.WriteHeader<Assignment>();
                csvWriter.NextRecord();
               csvWriter.WriteRecords(assignedLoan);
                csvWriter.Flush();
               
            }
        }
    }
}
