using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class PayrollDto
    {
        public int Id { get; set; }
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }
    }
}
