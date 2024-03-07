using Sprout.Exam.Business.FactoryUtil.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.FactoryUtil.ConcreteCreators
{
    public class ContractualFactory : PayrollFactory
    {
        protected override IPayroll CreateEmployeePayroll()
        {
            return new Contractual();
        }
    }
}
