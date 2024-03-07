using Sprout.Exam.Business.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Helper
{
    public static class DatabaseHelper
    {
        #region Read

        /// <summary>
        /// Retrieves a list of Employees from the database.
        /// Will not retrieve soft-deleted records.
        /// </summary>
        /// <param name="connString">Connection string of the database.</param>
        /// <returns>Returns a list of Employee records.</returns>
        public static async Task<IEnumerable<EmployeeDto>> GetEmployees(string connString)
        {
            List<EmployeeDto> employeeList = new List<EmployeeDto>();

            string query = "SELECT e.Id, e.FullName, e.Birthdate, e.TIN, e.EmployeeTypeId " +
                           "FROM Employee e " +
                           "WHERE e.IsDeleted = 0 ";
            try
            {

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                employeeList.Add(new EmployeeDto()
                                {
                                    Id = Convert.ToInt32(reader[0]),
                                    FullName = Convert.ToString(reader[1]),
                                    Birthdate = Convert.ToDateTime(reader[2]).ToString("yyyy-MM-dd"),
                                    Tin = Convert.ToString(reader[3]),
                                    TypeId = Convert.ToInt32(reader[4])
                                });
                            }
                        }

                    }

                    conn.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return employeeList;
        }

        /// <summary>
        /// Retrieves an Employee record based on a given id.
        /// Will not retrieve soft-deleted records.
        /// </summary>
        /// <param name="connString">Connection string of the database</param>
        /// <param name="id">Id of the employee</param>
        /// <returns>Returns an employee record</returns>
        public static async Task<EmployeeDto> GetEmployee(string connString, int id)
        {
            EmployeeDto employeeRecord = new EmployeeDto();

            string query = "SELECT e.Id, e.FullName, e.Birthdate, e.TIN, e.EmployeeTypeId " +
                           "FROM Employee e " +
                           "WHERE e.Id = @Id ";

            try
            {

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                employeeRecord = new EmployeeDto()
                                {
                                    Id = Convert.ToInt32(reader[0]),
                                    FullName = Convert.ToString(reader[1]),
                                    Birthdate = Convert.ToDateTime(reader[2]).ToString("yyyy-MM-dd"),
                                    Tin = Convert.ToString(reader[3]),
                                    TypeId = Convert.ToInt32(reader[4])
                                };
                            }
                        }

                    }

                    conn.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return employeeRecord;
        }

        #endregion

        #region Create



        /// <summary>
        /// Creates a new employee record.
        /// </summary>
        /// <param name="connString">Conenction string of the database</param>
        /// <param name="employeeRecord">Employee Record to be created</param>
        /// <returns>Returns id of the created record if successful, else false.</returns>
        public static async Task<int> CreateEmployee(string connString, CreateEmployeeDto employeeRecord)
        {
            int id = 0;

            string query = "INSERT INTO Employee (FullName, Birthdate, TIN, EmployeeTypeId, IsDeleted) OUTPUT INSERTED.Id " +
                           " VALUES (@name, @birthdate, @tin, @typeId, @isDel); ";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction sqlTrnsct = conn.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn, sqlTrnsct))
                            {

                                cmd.Parameters.AddWithValue("@name", employeeRecord.FullName);
                                cmd.Parameters.AddWithValue("@birthdate", employeeRecord.Birthdate);
                                cmd.Parameters.AddWithValue("@tin", employeeRecord.Tin);
                                cmd.Parameters.AddWithValue("@typeId", employeeRecord.TypeId);
                                cmd.Parameters.AddWithValue("@isDel", false);

                                id = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                                await sqlTrnsct.CommitAsync();
                            }
                        }
                        catch (Exception)
                        {
                            await sqlTrnsct.RollbackAsync();
                            throw;
                        }
                    }

                    conn.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return id;
        }
        #endregion



        #region Update

        /// <summary>
        /// Updates a given employee record.
        /// </summary>
        /// <param name="connString">Conenction string of the database</param>
        /// <param name="employeeRecord">Employee Record to be updated</param>
        /// <returns>Returns true if successful, else false</returns>
        public static async Task<EmployeeDto> UpdateEmployee(string connString, EditEmployeeDto employeeRecord)
        {
            EmployeeDto updatedRecord = new EmployeeDto();

            string query = "UPDATE Employee SET" +
                           " FullName = @name, " +
                           " Birthdate = @birthdate, " +
                           " TIN = @tin, " +
                           " EmployeeTypeId = @typeId " +
                           " WHERE Id = @id ";

            try
            {

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction sqlTrnsct = conn.BeginTransaction())
                    {
                        try
                        {


                            using (SqlCommand cmd = new SqlCommand(query, conn, sqlTrnsct))
                            {

                                cmd.Parameters.AddWithValue("@id", employeeRecord.Id);
                                cmd.Parameters.AddWithValue("@name", employeeRecord.FullName);
                                cmd.Parameters.AddWithValue("@birthdate", employeeRecord.Birthdate);
                                cmd.Parameters.AddWithValue("@tin", employeeRecord.Tin);
                                cmd.Parameters.AddWithValue("@typeId", employeeRecord.TypeId);

                                await cmd.ExecuteNonQueryAsync();

                                updatedRecord.Id = employeeRecord.Id;
                                updatedRecord.FullName = employeeRecord.FullName;
                                updatedRecord.Birthdate = employeeRecord.Birthdate.ToString("yyyy-MM-dd");
                                updatedRecord.Tin = employeeRecord.Tin;
                                updatedRecord.TypeId = employeeRecord.TypeId;

                                await sqlTrnsct.CommitAsync();
                            }
                        }
                        catch (Exception)
                        {
                            await sqlTrnsct.RollbackAsync();
                            throw;
                        }
                    }

                    conn.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return updatedRecord;
        }

        #endregion


        #region Delete


        /// <summary>
        /// Softly deletes a given employee record.
        /// </summary>
        /// <param name="connString">Conenction string of the database</param>
        /// <param name="id">Id of the employee to be deleted</param>
        /// <returns></returns>
        public static async Task<int> DeleteEmployee(string connString, int id)
        {
            string query = "UPDATE Employee SET" +
                           " IsDeleted = 1 " +
                           " WHERE Id = @id ";

            try
            {

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction sqlTrnsct = conn.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn, sqlTrnsct))
                            {

                                cmd.Parameters.AddWithValue("@id", id);
                                await cmd.ExecuteNonQueryAsync();

                                await sqlTrnsct.CommitAsync();
                            }
                        }
                        catch (Exception)
                        {
                            await sqlTrnsct.RollbackAsync();
                            throw;
                        }
                    }

                    conn.Close();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return id;
        }
        #endregion
    }
}
