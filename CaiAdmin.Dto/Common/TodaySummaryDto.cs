using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto
{
    public class TodaySummaryDto
    {
        public int InvoiceTotal { get; set; }
        public int InvoiceSuccess { get; set; }
        public int InvoiceFail { get; set; }

        public int CustomerTotal { get; set; }
        public int ExamineStatusExceptionCustomerTotal { get; set; }

        public int CustomerRepeatTotal { get; set; }
        public int CustomerZhRepeatTotal { get; set; }
    }
}
