﻿using ItSys.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaiAdmin.Entity.It
{
    public class Asset : AuditCompanyEntity
    {
        public int input_status { get; set; }
        public int? supplier_id { get; set; }
        [ForeignKey("supplier_id")]
        [Navigate(  NavigateType.OneToMany,nameof(supplier_id))]
        public Supplier Supplier { get; set; }
        public int type_id { get; set; }
        [ForeignKey("type_id")]
        [Navigate( NavigateType.OneToOne,nameof(type_id))]
        public ItAssetType Type { get; set; }
        public string no { get; set; }
        public string model { get; set; }
        public string diy_no { get; set; }
        public decimal price { get; set; }
        public string sn { get; set; }
        public string source { get; set; }
        public int amount { get; set; }
        public int used { get; set; }
        public int scrap_amount { get; set; }
        public DateTime inbound_date { get; set; }
        public string remarks { get; set; }
        public int? use_dep_id { get; set; }
        //[ForeignKey("use_dep_id")]
        [Navigate( NavigateType.OneToOne,nameof(use_dep_id))]
        public HrDep UseDep { get; set; }
        public int? use_employee_id { get; set; }
        //[ForeignKey("use_employee_id")]
        [Navigate(NavigateType.OneToOne, nameof(use_employee_id))]
        public HrEmployee UseEmployee { get; set; }
        public bool is_repair { get; set; }
        public int? stock_warning_id { get; set; }
        [ForeignKey("stock_warning_id")]
        [Navigate ( NavigateType.ManyToOne,nameof(stock_warning_id))]
        public ItAssetStockWarning StockWarning { get; set; }
        public string attach_guid { get; set; }
    }
}
