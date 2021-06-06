using Affirm.Challenge.LoanBalance.Entities;
using Affirm.Challenge.LoanBalance.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Affirm.Challenge.LoanBalance.Services
{
    public class LoanBalanceService : ILoanBalanceService
    {
        private IFacilitiyRepository _facilityRepository;
        private IAssignmentRepository _assignmentRepository;
        private IYieldRepository _yieldRepository;

        public LoanBalanceService(IFacilitiyRepository facilitiyRepository, IAssignmentRepository assignmentRepository, IYieldRepository yieldRepository)
        {
            this._facilityRepository = facilitiyRepository;
            this._assignmentRepository = assignmentRepository;
            this._yieldRepository = yieldRepository;
        }

        public object IyieldRepository { get; private set; }

        public virtual int assign(Loan loan)
        {
            facility facility = _facilityRepository.fetchCheapestRateWithMostAmount(
                loan.amount.Value,
                loan.default_likelihood.Value,
                loan.state
        );
            int facilityId = facility == null ? -1 : facility.id;
            _assignmentRepository.add(new Assignment() { loan_id =  loan.id, facility_id =  facilityId });
            if (facilityId > 0)
            {
                decimal newAmount = facility.amount.Value - loan.amount.Value;
                _facilityRepository.setAmount(facilityId,newAmount);
                decimal expectYield = calculateExpectedYield(loan, facility);
                _yieldRepository.add( new Yield() {
                facility_id = facilityId, 
                expected_yield=    expectYield
                });
            }
            return facilityId;
        }

        public virtual decimal calculateExpectedYield(Loan loan, facility facility)
        {
            decimal expectInterest = (1 - (loan.default_likelihood.HasValue ? (decimal)loan.default_likelihood.Value :0m) )
                                      * (loan.interest_rate.HasValue ? ((decimal)loan.interest_rate.Value) :0m)   * loan.amount.Value ;
            decimal expectLoss = (loan.amount.HasValue ? loan.amount.Value: 0m) * (loan.default_likelihood.HasValue ? (decimal)loan.default_likelihood.Value : 0.0m);
            decimal payToFacility = (loan.amount.HasValue ? loan.amount.Value : 0m) * (facility.interest_rate.HasValue ? (decimal)facility.interest_rate.Value:0.0m);
            decimal expectYield = expectInterest -  (expectLoss) - (payToFacility);
            
            return expectYield >= 0 ? expectInterest : 0M;
            
        }

        public void writeOutput(FileInfo assignmentFileInfo, FileInfo yieldFileInfo)
        {

            assignmentFileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
            yieldFileInfo.Directory.Create();
            _assignmentRepository.writeOutPut(assignmentFileInfo);
            _yieldRepository.writeOutput(yieldFileInfo);
        }
    }
}
