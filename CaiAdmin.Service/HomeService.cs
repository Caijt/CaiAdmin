using CaiAdmin.Dto;
using CaiAdmin.Database;
using CaiAdmin.Database.SqlSugar;
using CaiAdmin.Service.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CaiAdmin.Service
{
    public class HomeService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        public HomeService(CityOceanRepository cityOceanRepository)
        {
            _cityOceanRepository = cityOceanRepository;
        }

        public async Task<TodaySummaryDto> GetTodaySummaryAsync()
        {
            var result = new TodaySummaryDto();
            var invoiceSummary = _cityOceanRepository.Context.Ado.SqlQuerySingle<dynamic>("Select Count(1) as Total, Count(IIF(Status=6,1,Null)) as Fail,Count(iif(Status<>6,1,Null)) as Success From CO_Fam.dbo.InvoiceItems ii where ii.CreationTime >=@Today", new { Today = DateTime.Now.Date });
            result.InvoiceTotal = invoiceSummary.Total;
            result.InvoiceFail = invoiceSummary.Fail;
            result.InvoiceSuccess = invoiceSummary.Success;
            var customerSummary = _cityOceanRepository.Context.Ado.SqlQuerySingle<dynamic>("Select Count(1) as Total From CO_Crm.dbo.Customers c where c.CreationTime >=@Today", new { Today = DateTime.Now.Date });
            result.CustomerTotal = customerSummary.Total;
            var examineStatusExceptionCustomerTotal = await _cityOceanRepository.Context.Ado.GetScalarAsync(@"Select 
                    count(1)
                    from CO_CRM.dbo.customers cus
                inner join icp3.pub.customers cus2 on cus.id = cus2.id
                outer apply(select top 1 * from icp3.pub.CustomerConfirms cc where cc.CustomerID = cus2.ID order by cc.ApplyDate desc) t1
                 where cus.ExamineState = 2 and isnull(t1.State,0) <> 3");
            result.ExamineStatusExceptionCustomerTotal = Convert.ToInt32(examineStatusExceptionCustomerTotal);

            var customerRepeat = _cityOceanRepository.Context.Ado.SqlQuerySingle<dynamic>(@"Select count(1) as Total From (Select 
                        1 as a
                    From co_crm.dbo.customers Where IsDeleted = 0 And Id=MergerId
                    Group by NameUniqueValue having count(1) > 1 and max(CreationTime)>=@Today) t", new { Today = DateTime.Now.Date });
            if (customerRepeat != null)
            {
                result.CustomerRepeatTotal = customerRepeat.Total;
            }
            var customerZhRepeat = _cityOceanRepository.Context.Ado.SqlQuerySingle<dynamic>(@"Select count(1) as Total From (Select 
                        1 as a
                    From co_crm.dbo.customers Where IsDeleted = 0 And Id=MergerId
                    Group by NameLocalizationUniqueValue having count(1) > 1 and max(CreationTime)>=@Today) t", new { Today = DateTime.Now.Date });
            if (customerZhRepeat != null)
            {
                result.CustomerZhRepeatTotal = customerZhRepeat.Total;
            }

            return result;
        }
    }
}
