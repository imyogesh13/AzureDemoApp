using AzureTableStorageApp.Models;
using AzureTableStorageApp.TableHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureTableStorageApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                TableManager tableManager = new TableManager("person");
                List<Student> studentsList = tableManager.RetriveEntity<Student>("RowKey eq '" + id + "'");
                Student studentObj = studentsList.FirstOrDefault();
                return View(studentObj);

            }
            return View(new Student());
        }

        [HttpPost]
        public ActionResult Index(FormCollection formData)
        {
            if (formData != null)
            {
                Student objStudent = new Student();
                objStudent = BuildStudentData(formData, objStudent, true);

                TableManager tableManager = new TableManager("person");
                tableManager.InsertEntity<Student>(objStudent);
            }
            return RedirectToAction("Index");
        }

        public ActionResult UpdateStudent(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                TableManager tableManager = new TableManager("person");
                List<Student> studentsList = tableManager.RetriveEntity<Student>("RowKey eq '" + id + "'");
                Student studentObj = studentsList.FirstOrDefault();
                return View(studentObj);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateStudent(string id, FormCollection formData)
        {
            if (formData != null)
            {
                Student objStudent = new Student();
                objStudent.ETag = Request["ETag"].ToString();
                objStudent = BuildStudentData(formData, objStudent, false, id);

                TableManager tableManager = new TableManager("person");
                tableManager.UpdateEntity<Student>(objStudent);
            }
            return RedirectToAction("GetAll");
        }

        

        private static Student BuildStudentData(FormCollection formData, Student objStudent, bool forInsert, string id = null)
        {
            objStudent.PartitionKey = "Student";
            if (forInsert)
            {
                objStudent.RowKey = Guid.NewGuid().ToString();
            }
            else
            {
                objStudent.RowKey = id;
            }
            objStudent.Name = formData["Name"] == "" ? null : formData["Name"];
            objStudent.Email = formData["Email"] == "" ? null : formData["Email"];
            objStudent.Department = formData["Department"] == "" ? null : formData["Department"];
            return objStudent;
        }

        public ActionResult GetAll()
        {
            bool isDelete = false;
            if(Request["isDelete"] != null)
            {
                isDelete = Convert.ToBoolean(Request["isDelete"]);
            }
            TableManager tableManager = new TableManager("person");
            List<Student> liStudent = tableManager.RetriveEntity<Student>(null);
            return View(liStudent);
        }

        public ActionResult Delete(string id)
        {
            TableManager tableManager = new TableManager("person");
            List<Student> liObjStudent = tableManager.RetriveEntity<Student>("RowKey eq '" + id + "'");
            Student objStudent = liObjStudent.FirstOrDefault();
            bool isDelete = tableManager.DeleteEntity<Student>(objStudent);
            return RedirectToAction("GetAll", isDelete);
        }
    }
}