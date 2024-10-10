using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MvvmDemo.Models
{
    public class EmployeeService
    {
        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;

        public EmployeeService() 
        {
              ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EMSConnection"].ConnectionString);
              ObjSqlCommand = new SqlCommand();
              ObjSqlCommand.Connection = ObjSqlConnection;
              ObjSqlCommand.CommandType = CommandType.StoredProcedure;   
        
        }

        public List<Employee> GetAll()
        {
           List<Employee> ObjEmployeeList  = new List<Employee>();
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "_SelectAllEmployees";

                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows) {
                    Employee ObjEmployee = null;
                    while (ObjSqlDataReader.Read()) { 
                      ObjEmployee = new Employee();
                        ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                        ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                        string ageString = ObjSqlDataReader.GetString(2);
                        ObjEmployee.Age = int.Parse(ageString);

                        ObjEmployeeList.Add(ObjEmployee);
                    }
                    ObjSqlDataReader.Close();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { 
                ObjSqlConnection.Close();           
            }
            return ObjEmployeeList;
        }

        public bool Add(Employee objNewemployee) 
        {
            bool IsAdded = false;
            if (objNewemployee.Age < 21 || objNewemployee.Age > 60)
                throw new ArgumentException("Invalid Age Limit for Employee");

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "_InsertEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id", objNewemployee.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objNewemployee.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", objNewemployee.Age);

                ObjSqlConnection.Open();
                int NoofRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsAdded = NoofRowsAffected > 0;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }


            return true;    
        }

        public bool Update(Employee objEmployeeUpdate) 
        { 
            bool IsUpdated = false;
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "_UpdateEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id", objEmployeeUpdate.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objEmployeeUpdate.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", objEmployeeUpdate.Age);

                ObjSqlConnection.Open();
                int NoofRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsUpdated = NoofRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }


            return IsUpdated;
        }

        public bool Delete(int id) 
        {
            bool IsDeleted = false;
           
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "_DeleteEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id", id);

                ObjSqlConnection.Open();
                int NoofRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsDeleted = NoofRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return IsDeleted;   
         }

        public Employee Search(int id)
        {
            Employee ObjEmployee = null;

            try
            {

                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "_SelectEmployeebyId";
                ObjSqlCommand.Parameters.AddWithValue("@Id", id);

                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();
                if (ObjSqlDataReader.HasRows)
                {
                    ObjSqlDataReader.Read();
                    ObjEmployee = new Employee();
                    ObjEmployee.Id = ObjSqlDataReader.GetInt32(0);
                    ObjEmployee.Name = ObjSqlDataReader.GetString(1);
                    string ageString = ObjSqlDataReader.GetString(2);
                    ObjEmployee.Age = int.Parse(ageString);
                    ObjSqlDataReader.Close();
                }
            }
            catch (Exception ex) { throw ex; }
            finally { ObjSqlConnection.Close(); }

            return ObjEmployee;


        }

    }
}
