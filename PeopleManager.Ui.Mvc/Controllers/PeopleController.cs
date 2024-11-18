using Microsoft.AspNetCore.Mvc;
using PeopleManager.Ui.Mvc.Core;
using PeopleManager.Ui.Mvc.Models;
using System;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class PeopleController : Controller
    {

        private readonly PeopleManagerDbContext _peopleManagerDbContext;

        public PeopleController(PeopleManagerDbContext peopleManagerDbContext)
        {
            _peopleManagerDbContext = peopleManagerDbContext;
        }

        public IActionResult Index()
        {
            var people = _peopleManagerDbContext.People.ToList();

            return View(people);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm]Person person)
        {
            _peopleManagerDbContext.People.Add(person);

            _peopleManagerDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit([FromRoute]int id)
        {
            var person = _peopleManagerDbContext.People.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, [FromForm]Person person)
        {
            var dbperson = _peopleManagerDbContext.People.FirstOrDefault(x => x.Id == id);
            if (dbperson == null)
            {
                return RedirectToAction("Index");
            }

            dbperson.FirstName = person.FirstName;
            dbperson.LastName = person.LastName;
            dbperson.Email = person.Email;

            _peopleManagerDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete([FromRoute]int id)
        {
            var person = _peopleManagerDbContext.People.FirstOrDefault(x => x.Id == id);
            if (person == null)
            {
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        [Route("People/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirm([FromRoute]int id)
        {
            var person = new Person
            {
                Id = id,
                FirstName = string.Empty,
                LastName = string.Empty
            };

            _peopleManagerDbContext.People.Attach(person);

            _peopleManagerDbContext.People.Remove(person);

            _peopleManagerDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}