﻿using Demo.BLL.Interface;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class EmployeeController : Controller
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public EmployeeController(IEmployeeRepository employeeRepository) // configure DI in Startup
        {
            EmployeeRepository = employeeRepository;
        }


        public IActionResult Index()
        {
            return View(EmployeeRepository.GetAll());
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return NotFound();
            var Employee = EmployeeRepository.Get(id);
            if (Employee == null)
                return NotFound();
            return View(ViewName, Employee);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                EmployeeRepository.Add(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    EmployeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(employee);
                }
            }
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int? id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            try
            {
                EmployeeRepository.Delete(employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(employee);
            }
        }
    }
}
