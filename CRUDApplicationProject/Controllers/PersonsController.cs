using CRUDApplicationProject.Models;
using CRUDApplicationProject.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDApplicationProject.Controllers
{
    public class PersonsController : Controller
    {
        private readonly AppDbContext _context;
        private IEnumerable<object> Persons;

        public PersonsController(AppDbContext context)
        {
            _context = context;
        }
        [Route("persons/index")]
        [Route("/")]
        public async Task<IActionResult> Index(string? searchBy, string? searchString, string? sortBy, string? sortOrder)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(Person.PersonName), "Person Name"},
                {nameof(Person.Email),"Email" },
                {nameof(Person.DateOfBirth),"Date of Birth" },
                {nameof(Person.Age),"Age" },
                {nameof(Person.Gender),"Gender" },
                {nameof(Person.CountryName),"Country Name" },
                {nameof(Person.Address),"Address" }
            };

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder;

            IEnumerable<Person> persons = _context.Persons;
             //List<Person>person = await _context.Persons.ToListAsync();
            if(!string.IsNullOrEmpty(searchString))
            {
                switch(searchBy)
                {
                    case "PersonName":
                        persons = persons.Where(p => p.PersonName.Contains(searchString));
                        break;
                    case "Email":
                        persons = persons.Where(p => p.Email.Contains(searchString));
                        break;
                    case "DateOfBirth":
                        if (DateTime.TryParse(searchString, out DateTime searchDate))
                        {
                            persons = persons.Where(p => p.DateOfBirth?.Date == searchDate.Date);
                        }
                        break;

                    case "Age":
                        if(int.TryParse(searchString,out int searchAge))
                        {
                            persons = persons.Where(p=>p.Age == searchAge);
                        }
         
                        break;
                    case "Gender":
                        persons = persons.Where(p => string.Compare(p.Gender,searchString,true) == 0);
                        break;
                    case "CountryName":
                        persons = persons.Where(p => p.CountryName.Contains(searchString));
                        break;

                }
            }
            else
            {
                persons = await _context.Persons.ToListAsync();
            }

            if (!string.IsNullOrEmpty(sortOrder) && !string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "personname":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.PersonName) : persons.OrderBy(p => p.PersonName);
                        break;
                    case "email":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.Email) : persons.OrderBy(p => p.Email);
                        break;
                    case "age":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.Age) : persons.OrderBy(p => p.Age);
                        break;
                    case "dateofbirth":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.DateOfBirth) : persons.OrderBy(p => p.DateOfBirth);
                        break;
                    case "gender":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.Gender) : persons.OrderBy(p => p.Gender);
                        break;
                    case "countryname":
                        persons = sortOrder == "desc" ? persons.OrderByDescending(p => p.CountryName) : persons.OrderBy(p => p.CountryName);
                        break;
                }
            }
            return View(persons);
        }

        [Route("/persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(Person persons)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _context.Persons.Add(persons);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("[action]/{personId}")] // Ex: action= Edit
        public async Task<IActionResult> Edit(int personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if(person == null)
            {
                return RedirectToAction("Index");
            }
            return View(person);
        }
        [HttpPost]
        [Route("[action]/{personId}")] // Ex: action= Edit
        public async Task<IActionResult> Edit(int personId,Person updatedPerson)
        {
            if (updatedPerson == null)
            {
                return RedirectToAction("Index");
            }
            if(personId != updatedPerson.PersonID)
            {
                return BadRequest();
            }
            
            if (!ModelState.IsValid)
            {
                return View(updatedPerson);
            }
            _context.Update(updatedPerson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

                
        }

        [HttpGet]
        [Route("[action]/{personId}")] // Ex: action= Edit
        public async Task<IActionResult> Delete(int personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return RedirectToAction("Index");
            }
            return View(person);
        }
        [HttpPost]
        [Route("[action]/{personId}")] // Ex: action= Edit
        public async Task<IActionResult> Delete(int personId , Person persons)
        {
            var deletePerson = await _context.Persons.FindAsync(personId);
            if (deletePerson == null)
            {
                return RedirectToAction("Index");
            }
            if (personId != deletePerson.PersonID)
            {
                return BadRequest();
            }

            _context.Persons.Remove(deletePerson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
}
