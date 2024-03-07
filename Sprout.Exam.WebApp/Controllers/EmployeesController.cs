using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Helper;
using Sprout.Exam.WebApp.Data;
using Microsoft.Extensions.Configuration;
using Sprout.Exam.Business.FactoryUtil;
using Sprout.Exam.Business.FactoryUtil.Products;
using Sprout.Exam.Business.FactoryUtil.ConcreteCreators;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly string _connString;
        public EmployeesController(IConfiguration config)
        {
            this._connString = config.GetConnectionString("DefaultConnection");
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await Task.FromResult(DatabaseHelper.GetEmployees(_connString));
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await Task.FromResult(DatabaseHelper.GetEmployee(_connString, id));
                return Ok(result.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            try
            {


                var item = await Task.FromResult(DatabaseHelper.UpdateEmployee(_connString, input));
                if (item == null) return NotFound();
                /*
                item.FullName = input.FullName;
                item.Tin = input.Tin;
                item.Birthdate = input.Birthdate.ToString("yyyy-MM-dd");
                item.TypeId = input.TypeId;
                */
                return Ok(item.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            try
            {
                var id = await Task.FromResult(DatabaseHelper.CreateEmployee(_connString, input));
                /*
                StaticEmployees.ResultList.Add(new EmployeeDto
                {
                    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                    FullName = input.FullName,
                    Id = id,
                    Tin = input.Tin,
                    TypeId = input.TypeId
                });
                */

                return Created($"/api/employees/{id.Result}", id.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await Task.FromResult(DatabaseHelper.DeleteEmployee(_connString, id));
                if (result == null)
                    return NotFound();
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(PayrollDto payrollInfo)
        {
            if(payrollInfo.AbsentDays < 0)
            {
                return UnprocessableEntity("Absent Days cannot be less than 0");
            }

            if(payrollInfo.WorkedDays < 0)
            {
                return UnprocessableEntity("Worked Days cannot be less than 0");
            }

            try
            {

                var result = await Task.FromResult(DatabaseHelper.GetEmployee(_connString, payrollInfo.Id));

                if (result.Result == null) return NotFound();

                var type = (EmployeeType)result.Result.TypeId;
                IPayroll payroll = null;
                switch (type)
                {
                    case EmployeeType.Regular:
                        payroll = new RegularFactory().GetEmployeePayroll();
                        return Ok(payroll.GetMonthSalary(payrollInfo.AbsentDays));
                    case EmployeeType.Contractual:
                        payroll = new ContractualFactory().GetEmployeePayroll();
                        return Ok(payroll.GetMonthSalary(payrollInfo.WorkedDays));
                    default:
                        return NotFound("Employee Type not found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
