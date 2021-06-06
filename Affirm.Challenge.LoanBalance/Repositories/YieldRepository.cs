using Affirm.Challenge.LoanBalance.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public class YieldRepository : IYieldRepository
    {
        private List<Yield> yields;

        public YieldRepository() {
            this.yields = new List<Yield>();
        }
        public void add(Yield yield)
        {
            this.yields.Add(yield);
        }

        public void writeOutput(FileInfo fileInfo)
        {
            FileStream fs = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            using (var writer = new StreamWriter(fs))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {


                csvWriter.WriteHeader<Yield>();
                csvWriter.NextRecord();
                csvWriter.WriteRecords(this.yields);
                csvWriter.Flush();

                var result = writer.ToString();
               
            }
        }
    }
}
