using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.FactoryUtil.Products
{
    public class Regular : IPayroll
    {
        public static EmployeeType EmployeeType
        {
            get { return EmployeeType.Regular; }
        }

        /// <summary>
        /// Calculates the monthly salary of a regular employee based on number of absents.
        /// </summary>
        /// <param name="days"></param>
        /// <returns>Returns the calculated salary in string format.</returns>
        public string GetMonthSalary(decimal days)
        {
            // Regular Employees have a base salary of 20,000.00
            decimal salaryAmt = 20000.00M;

            // 1 day deduction is equal to Salary / 22
            decimal salaryPerDay = salaryAmt / 22M;

            // Regular Employees have 12% tax deduction
            decimal taxDeduct = salaryAmt * 0.12M;

            // Calculate Pay based on number of absents.
            decimal pay = salaryAmt - (days * salaryPerDay) - taxDeduct;
            
            
            // Return calculated pay if equal or greater than 0.
            return  (pay >= 0M) ? pay.ToString("0.00") : "0.00";
        }
    }
}
