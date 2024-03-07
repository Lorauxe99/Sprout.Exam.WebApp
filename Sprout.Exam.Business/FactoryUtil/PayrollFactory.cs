using Sprout.Exam.Business.FactoryUtil.Products;
using Sprout.Exam.Common.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sprout.Exam.Business.FactoryUtil
{
    public abstract class PayrollFactory
    {
        protected abstract IPayroll CreateEmployeePayroll();

        public IPayroll GetEmployeePayroll()
        {
            return this.CreateEmployeePayroll();
        }
    }
}
