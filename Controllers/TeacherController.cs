using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AadritChauhanProjectModels;

namespace AadritChauhanProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This function returns list teachers
        /// </summary>
        /// <param name="Search_key"></param>
        /// <returns>list of Teachers</returns>
        //GET : /Teachers/List
        public ActionResult List(string Search_key = null)
        {

            TeachersController controller = new TeachersController();
            IEnumerable<Teacher> Teachers = controller.ListTeachers(Search_key);
            return View(Teachers);
        }


        /// <summary>
        /// rThis function returns multiple details of teachers.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>details of NewTeacher</returns>
        //GET: /Teacher/Show/{id}

        public ActionResult Show(int id)
        {
            TeachersController controller = new TeachersController();
            Teacher Selected_teacher = controller.FindTeacher(id);
            return View(Selected_teacher);
        }

        //GET: /Teacher/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            TeachersController controller = new TeachersController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }

        //POST : /Teacher/Delete/{id}
        [HttpPost]
        public ActionResult Delete(int id)
        {
            TeachersController controller = new TeachersController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET: /Teacher/New

        public ActionResult New()
        {

            return View();
        }

        //POST : /Teacher/Create
        public ActionResult Create(string Teacher_fname, string Teacher_lname, string Employee_number, decimal Salary)
        {

            Debug.WriteLine("Works fine");
            Debug.WriteLine(Teacher_fname);
            Debug.WriteLine(Teacher_lname);
            Debug.WriteLine(Employee_number);
            Debug.WriteLine(Salary);

            Teacher NewTeacher = new Teacher();
            NewTeacher.Teacher_fname = Teacher_fname;
            NewTeacher.Teacher_lname = Teacher_lname;
            NewTeacher.Employee_number = Employee_number;
            NewTeacher.Salary = Salary;

            TeachersController Controller = new TeachersController();
            Controller.AddTeacher(NewTeacher);
            return RedirectToAction("List");
        }


        //GET : /teacher/Update/{id}
        /// <summary>
        /// This a GET request for updation.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {

            TeachersController controller = new TeachersController();
            Teacher Selected_teacher = controller.FindTeacher(id);


            return View(SelectedTeacher);
        }

        //POST : /Teacher/Update/10
        /// <summary>
        /// This is a POST request for updation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Teacher_fname"></param>
        /// <param name="Teacher_lname"></param>
        /// <param name="Employee_number"></param>
        /// <param name="Salary"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(int id, string Teacher_fname, string Teacher_lname, string Employee_number, decimal Salary)
        {
            Teacher TeacherInfo = new Teacher();
            TeacherInfo.Teacher_fname = Teacher_fname;
            TeacherInfo.Teacher_lname = Teacher_lname;
            TeacherInfo.Employee_number = Employee_number;
            TeacherInfo.Salary = Salary;

            TeachersController Controller = new TeachersController();
            Controller.UpdateTeacher(id, TeacherInfo);


            return RedirectToAction("Show/" + id);
        }
    }
}