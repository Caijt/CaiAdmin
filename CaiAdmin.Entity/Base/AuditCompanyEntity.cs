using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ItSys.Entity 
{
    public class AuditCompanyEntity : AuditEntity
    {
        public int company_id { get; set; }
        [ForeignKey("company_id")]
        [Navigate(NavigateType.OneToOne,nameof(company_id))]
        public SysCompany Company { get; set; }
    }
}
