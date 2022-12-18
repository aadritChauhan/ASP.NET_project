using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MySql.Data.MySqlClient;
using AadritChauhanProject.Models;

namespace AadritChauhanProject.Controllers
{
    public class TeachersController : ApiController
    {
        //The databse context class which allows us to access our MySql Database.
        private CumulativeProjectDb Teachers = new CumulativeProjectDb();
        //This Controller will access the teachers table of our project database.

        //This Controller will access the teachers table of our project database/
        /// <summary>
        /// Returns a list of teachers in the system.
        /// </summary>
        /// <example>
        /// GET api/Teachers/List
        /// </example>
        /// <param name="Search_key"></param>
        /// <returns>A list of teachers (first names and last names)</returns>
        [HttpGet]
        [Route("api/Teachers/ListTeachers/{Search_key?}")]
        public IEnumerable<Teacher> ListTeachers(string Search_key = null)
        {
            //Create an instance of a connection.
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the connection between the web server and database.
            Conn.Open();

            //Establish a new command(query) for our database.
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat (teacherfname, ' ', teacherlname)) like lower(@key)";

            //A extra step is done to protect the database against SQL injection, SearchKey is replaced by @key, so malacious inputs cannot execute.
            cmd.Parameters.AddWithValue("@key", "%" + Search_key + "%");
            cmd.Prepare();

            //Results of the query would be stored here.
            MySqlDataReader Results = cmd.ExecuteReader();

            //It is responsible for displaying the list of teachers.
            List<Teacher> TeachersInfo = new List<Teacher> { };


            //Loop through Each Row the Result Set
            while (Results.Read())
            {
                //Access Column information by the DB column name as an index.
                DateTime HireDate = (DateTime)Results["hiredate"];
                decimal Salary = (decimal)Results["salary"];
                string Employee_number = (string)Results["employeenumber"];
                int TeacherId = (int)Results["teacherid"];
                string Teacher_fname = (string)Results["teacherfname"];
                string Teacher_lname = (string)Results["teacherlname"];

                //Object is created to pass data to teacher class.
                Teacher NewTeacher = new Teacher();

                //data is passed to teacher class, and can be accessed through variables.
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.Teacher_fname = Teacher_fname;
                NewTeacher.Teacher_lname = Teacher_lname;
                NewTeacher.Employee_number = Employee_number;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                TeachersInfo.Add(NewTeacher);
            }

            //Close the connection between the web server and database.
            Conn.Close();


            //returns final list of teachers names.
            return TeachersInfo;


        }


        /// <summary>
        /// Returns teachers details in the system.
        /// </summary>
        /// <example>GET api/Teachers/Show </example>
        /// <param name="id"></param>
        /// <returns>returns details of teachers(hiredate, salary, employeenumber, teacherid, teacherfname, teacherlname)</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            //Create an instance of a connection.
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the connection between the web server and database.
            Conn.Open();

            //Establish a new command(query) for our database.
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from teachers where teacherid = " + id;

            //Results of the query would be stored here.
            MySqlDataReader Results = cmd.ExecuteReader();

            //Loop through Each Row the Result Set
            while (Results.Read())
            {
                //Access Column information by the DB column name as an index.
                DateTime HireDate = (DateTime)Results["hiredate"];
                decimal Salary = (decimal)Results["salary"];
                string Employee_number = (string)Results["employeenumber"];
                int TeacherId = (int)Results["teacherid"];
                string Teacher_fname = (string)Results["teacherfname"];
                string Teacher_lname = (string)Results["teacherlname"];

                //data is passed to teacher class, and can be accessed through variables.
                NewTeacher.TeacherId = TeacherId;
                NewTeacher.Teacher_fname = Teacher_fname;
                NewTeacher.Teacher_lname = Teacher_lname;
                NewTeacher.Employee_number = Employee_number;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;
            }

            //returns final details of teachers.
            return NewTeacher;
        }

        /// <summary>
        /// Returns a function to delete teacher
        /// </summary>
        /// <param name="id"></param>
        /// <example>POST : /api/Teachers/DeleteTeacher/3</example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the connection between the web server and database.
            Conn.Open();

            //Establish a new command(query) for our database.
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from teachers where teacherid = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// This function allows user to add teacher
        /// </summary>
        /// <param name="NewTeacher"></param>
        [HttpPost]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the connection between the web server and database.
            Conn.Open();

            //Establish a new command(query) for our database.
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate,salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber,CURRENT_DATE(),@Salary)";
            cmd.Parameters.AddWithValue("@Teacher_fname", NewTeacher.Teacher_fname);
            cmd.Parameters.AddWithValue("@Teacher_lname", NewTeacher.Teacher_lname);
            cmd.Parameters.AddWithValue("@Employee_number", NewTeacher.Employee_number);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// This function is used for updating a user
        /// It updates firstname, lastName, salary, employee_number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TeacherInfo"></param>
        public void UpdateTeacher(int id, [FromBody] Teacher TeacherInfo)
        {
            MySqlConnection Conn = Teachers.AccessDatabase();

            //Open the connection between the web server and database.
            Conn.Open();

            //Establish a new command(query) for our database.
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "update teachers set teacherfname=@Teacher_fname, teacherlname=@Teacher_lname, employeenumber=@Employee_number,salary=@Salary where teacherid=@TeacherId";
            cmd.Parameters.AddWithValue("@Teacher_fname", TeacherInfo.Teacher_fname);
            cmd.Parameters.AddWithValue("@Teacher_lname", TeacherInfo.Teacher_lname);
            cmd.Parameters.AddWithValue("@Employee_number", TeacherInfo.Employee_number);
            cmd.Parameters.AddWithValue("@Salary", TeacherInfo.Salary);
            cmd.Parameters.AddWithValue("@TeacherId", id);
            cmd.Prepare();


            cmd.ExecuteNonQuery();

            Conn.Close();

        }

    }
}
