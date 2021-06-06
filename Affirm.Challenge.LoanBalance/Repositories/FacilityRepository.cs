using Affirm.Challenge.LoanBalance.Entities;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace Affirm.Challenge.LoanBalance.Repositories
{
    public  class ConvenantMap : ClassMap<Covenant>
    {
        public ConvenantMap()
        {
            Map(m => m.max_default_likelihood).Name("max_default_likelihood").TypeConverterOption.NullValues(string.Empty);
            Map(m => m.banned_state).Name("banned_state").Default("");
        }
    }
    public class FacilityRepository: IFacilitiyRepository
    {
        List<facility> facilities = new List<facility>();
        public FacilityRepository(FileInfo facilityFile, FileInfo covenantFile)
        {
            loadFacilities(facilityFile);
            loadCovenants(covenantFile);
        }

        public void loadCovenants(FileInfo covenantFile)
        {
            FileStream fs = covenantFile.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                
                var records = csv.GetRecords<Covenant>();
                var groups = records.GroupBy(g => g.facility_id).Select(group => new { Key = group.Key, Items = group.ToList() });
                foreach (var item in groups)
                {
                    var facility = facilities.Where(w => w.id == item.Key).FirstOrDefault();
                    if (facility != null)
                    {
                        facility.covenants = new List<Covenant>();
                        facility.covenants.AddRange(item.Items);

                    }
                }
            }

        }
        public void loadFacilities(FileInfo facilityFile)
        {

            //Open file for Read\Write
            FileStream fs = facilityFile.Open(FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using (var reader = new StreamReader(fs))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<facility>();
                facilities.AddRange(records);
            }
        }

        public facility fetchCheapestRateWithMostAmount(decimal amount,
                                              float defaultLikelihood,
                                              string state)
        {
            var CheaperFacilities = facilities.Where(w => w.amount > 0 && w.amount >= amount  ).OrderBy(o => o.interest_rate);
            foreach (var facility in CheaperFacilities)
            {
                // Checking Covenants
                var count  = facility.covenants.Count(w => w.banned_state != state && w.max_default_likelihood >= defaultLikelihood);
                if (count > 0)
                {
                    return facility;
                }
            
            }
            return null;
            
        }

        public facility GetFacility(int facilityId)
        {
            return this.facilities.FirstOrDefault(w => w.id == facilityId);
        }

       public virtual  void  setAmount(int facilityId ,decimal newAmount)
        {
            var item = this.facilities.FirstOrDefault(w => w.id == facilityId);
            item.amount = newAmount;
             
        }
    }
}
             
