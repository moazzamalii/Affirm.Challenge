using Affirm.Challenge.LoanBalance.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public interface IFacilitiyRepository
    {
        facility fetchCheapestRateWithMostAmount(decimal amount, float defaultLikelihood, string state);
        facility GetFacility(int facilityId);
        void setAmount(int facilityId, decimal newAmount);
        void loadCovenants(FileInfo covenantFile);
        void loadFacilities(FileInfo facilityFile);
    }
}
