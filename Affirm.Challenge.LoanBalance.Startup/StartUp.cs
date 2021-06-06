using Affirm.Challenge.LoanBalance.Entities;
using Affirm.Challenge.LoanBalance.Repositories;
using Affirm.Challenge.LoanBalance.Services;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Affirm.Challenge.LoanBalance.Startup
{
    public class StartUp
    {

        public const string DataSetName = "large";

        public const string FACILITY_NAME = @"\inputs\" + DataSetName + @"\facilities.csv";
        public const string COVENANT_FALIENAME = @"\inputs\" + DataSetName + @"\covenants.csv";
        public const string LOANS_FILENAME = @"\inputs\" + DataSetName + @"\loans.csv";

        public const string ASSIGNMENT_FILENAME = @"\output\" + DataSetName + @"\assignments_";
        public const string YIELDS_FILENAME = @"\output\" + DataSetName + @"\yields_";

        static void Main(string[] args)
        {
            string directory = GetApplicationRoot();

            //Input
            FileInfo facililyCSV = new FileInfo(Path.Combine(directory + FACILITY_NAME));
            FileInfo covenantCSV = new FileInfo(Path.Combine(directory + COVENANT_FALIENAME));
            FileInfo loanCSV = new FileInfo(Path.Combine(directory + LOANS_FILENAME));

            //output
            FileInfo assignmentCSV = new FileInfo(Path.Combine(directory + ASSIGNMENT_FILENAME + DateTime.Now.Ticks.ToString() + ".csv"));
            FileInfo yieldCSV = new FileInfo(Path.Combine(directory + YIELDS_FILENAME + DateTime.Now.Ticks.ToString() + ".csv"));

            //setup our DI
            var serviceProvider = new ServiceCollection()

                .AddSingleton<IFacilitiyRepository, FacilityRepository>(x => new FacilityRepository(facililyCSV, covenantCSV))
                .AddSingleton<IAssignmentRepository, AssignmentRepository>()
                .AddSingleton<IYieldRepository, YieldRepository>()
                .AddSingleton<IAssignmentRepository, AssignmentRepository>()
                .AddTransient<ILoanStream, LoanStream>()
                .AddTransient<ILoanBalanceService, LoanBalanceService>()

                .BuildServiceProvider();

            var loanBalanceService = serviceProvider.GetService<ILoanBalanceService>();
            Console.WriteLine("Facilities and Convenants are loaded successfully !!!");
            Console.WriteLine("Loading LoanStream !!!");

            var loansStream = serviceProvider.GetService<ILoanStream>();
            var loans = loansStream.getLoanStream(loanCSV);
            Console.WriteLine(string.Format("LoanStream is loaded successfully !!!"));
            foreach (var loan in loans)
            {
                var facilityId = loanBalanceService.assign(loan);
                Console.WriteLine(string.Format("Loan Id: {0}, is Assigned to Facility ID {1}", loan.id.ToString(), facilityId.ToString()));
            }

            loanBalanceService.writeOutput(assignmentCSV, yieldCSV);

            Console.WriteLine(string.Format(" All Loans are processed successfully, Yield and Assignment Files are available at \n( {0} \n ) ", assignmentCSV.FullName));


        }


        public static string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
    }
}
