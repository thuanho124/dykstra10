using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;

        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Departments.Include(d => d.Administrator);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // store the query into string and pass it into FromSql
            string query = "SELECT * FROM Department WHERE DepartmentID = {0}";

            // eager loading for Administrator
            var department = await _context.Departments
                .FromSql(query, id) // FromSql method receives the query and retrieves a department 
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);


            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID,RowVersion")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // eager loading for Administrator
            var department = await _context.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);


            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // eager loading - read the department to be updated
            var departmentToUpdate = await _context.Departments.Include(i => i.Administrator).FirstOrDefaultAsync(m => m.DepartmentID == id);

            // if departmentToUpdate get a return of null, the department was deleted by another user
            if (departmentToUpdate == null)
            {
                //use the posted form values to create a deparment entity 
                Department deletedDepartment = new Department();
                await TryUpdateModelAsync(deletedDepartment);

                // display error message
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The department was deleted by another user.");

                // show deleted department on the view
                ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", deletedDepartment.InstructorID);
                return View(deletedDepartment);
            }

            // stores original values of RowVersion in the hidden field
            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;

            // if the updathe is possible, allow user to change Deparment entites
            if (await TryUpdateModelAsync<Department>(
                departmentToUpdate,
                "",
                s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                // save changes after a user inputs values in the field
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // if no rows are somehow affected by update, throws DbUpdateConcurrencyException exception 
                catch (DbUpdateConcurrencyException ex)
                {
                    // get the affected Department entity that has an updated value on the exception object
                    var exceptionEntry = ex.Entries.Single();

                    // get new values entered by the user and put it in the Entries collection
                    var clientValues = (Department)exceptionEntry.Entity;

                    // get current database values
                    var databaseEntry = exceptionEntry.GetDatabaseValues();

                    //  if database value is null, show a error message stating that the department was deleted
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The department was deleted by another user.");
                    }
                    else
                    {
                        // get current database values and put it in collection
                        var databaseValues = (Department)databaseEntry.ToObject();

                        // compare database values and new values entered by the user
                        // if these values are different, show a message
                        if (databaseValues.Name != clientValues.Name) // compare name
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.Name}"); // error message
                        }
                        if (databaseValues.Budget != clientValues.Budget) // compare Budget
                        {
                            ModelState.AddModelError("Budget", $"Current value: {databaseValues.Budget:c}"); // error message
                        }
                        if (databaseValues.StartDate != clientValues.StartDate) // compare StartDate
                        {
                            ModelState.AddModelError("StartDate", $"Current value: {databaseValues.StartDate:d}"); // error message
                        }
                        if (databaseValues.InstructorID != clientValues.InstructorID) // compare Instructor 
                        {
                            // get Instructor entity 
                            Instructor databaseInstructor = await _context.Instructors.FirstOrDefaultAsync(i => i.ID == databaseValues.InstructorID);

                            ModelState.AddModelError("InstructorID", $"Current value: {databaseInstructor?.FullName}"); // error message
                        }

                        // show confirmation message after the user inputs new  values
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");

                        // set RowVersion value to new value retrieved from the database
                        // RowVersion will be stored in hidden field
                        departmentToUpdate.RowVersion = (byte[])databaseValues.RowVersion;

                        // remove old RowVersion value to get database updated with new values
                        ModelState.Remove("RowVersion");
                    }
                }
            }
            // show Instructor entities on the view
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", departmentToUpdate.InstructorID);
            return View(departmentToUpdate);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? concurrencyError) // added a boolean, which sets concurrencyError to null
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (department == null)
            {
                // if concurrencyError is true and the deparement  does not exist, the code redirects to the Index page 
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }

            // // if concurrencyError is true and the deparement exists, the code shows an error message through ViewData
            if (concurrencyError.GetValueOrDefault())
            {
                ViewData["ConcurrencyErrorMessage"] = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Department department) // change id parameter to a Department entity to access to the RowVersion property value
        {
            try
            {
                // using DepartmentID to get Department entites
                // if the department is deleted, it redirects to the Index page.
                if (await _context.Departments.AnyAsync(m => m.DepartmentID == department.DepartmentID))
                {
                    _context.Departments.Remove(department);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            // if a concurrency error got caught, display the Delete confirmation page, and set concurrencyError flag to true
            // when concurrencyError flag is true, display am error message in HttpGet Delete
            catch (DbUpdateConcurrencyException /* ex */)
            {
                
                return RedirectToAction(nameof(Delete), new { concurrencyError = true, id = department.DepartmentID });
            }
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentID == id);
        }
    }
}
