using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.FactoryUtil
{
    public interface IPayroll
    {
        /// <summary>
        /// Calculates the monthly salary of an employee based on Employee type.
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        string GetMonthSalary(decimal days);
    }
}
