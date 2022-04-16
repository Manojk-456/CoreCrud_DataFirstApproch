using CrudCore.Db_Context;
using CrudCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrudCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        studentlistContext obj = new studentlistContext();
        public IActionResult Index()
        {
            var res = obj.Students.ToList();
            List<StudentMod> sobj = new List<StudentMod>();
            foreach (var item in res)
            {
                sobj.Add(new StudentMod 
                {
                   Id=item.Id, 
                   Name=item.Name, 
                   Email=item.Email, 
                   Password=item.Password 

                });
            }
            return View(sobj);
        }

        public IActionResult Delete(int id)
        {
            var del = obj.Students.Where(m => m.Id == id).First();
            obj.Students.Remove(del);
            obj.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddStudent(StudentMod sobj)
        {
            Student stobj = new Student();
            stobj.Id = sobj.Id;
            stobj.Name = sobj.Name;
            stobj.Email = sobj.Email;
            stobj.Password = sobj.Password;
            if (sobj.Id == 0)
            { 
            obj.Students.Add(stobj);
            obj.SaveChanges();
            }
            else
            {
                obj.Entry(stobj).State = EntityState.Modified;
                obj.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }

        public IActionResult Edit(int id)
        {
            StudentMod st = new StudentMod();
            var edititem = obj.Students.Where(m => m.Id == id).First();
            st.Id = edititem.Id;
            st.Name = edititem.Name;
            st.Email = edititem.Email;
            st.Password = edititem.Password;
            ViewBag.id = edititem.Id;
            return View("AddStudent",st);
        }

        public IActionResult Privacy()
        {
            HttpContext.Session.SetString("name","manoj kumar");
            var session = HttpContext.Session.GetString("name");
            //ViewBag.Session = HttpContext.Session.GetString("name");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
