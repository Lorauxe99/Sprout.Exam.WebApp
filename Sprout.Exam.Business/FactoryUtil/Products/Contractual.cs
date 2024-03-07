using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.FactoryUtil.Products
{
    public class Contractual : IPayroll
    {
        public static EmployeeType EmployeeType
        {
            get { return EmployeeType.Contractual; }
        }


        /// <summary>
        /// Calculates the monthly salary of a contractual employee based on number of days worked.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public string GetMonthSalary(decimal days)
        {
            // Contractual workers have a base salary of 500.00 per day.
            decimal salaryPerDay = 500.00M;

            // Calculate Pay based on number of worked days
            decimal pay = salaryPerDay * days;

            // Return calculated pay if equal or greater than 0.
            return (pay >= 0M) ? pay.ToString("0.00") : "0.00";
        }
    }
}
