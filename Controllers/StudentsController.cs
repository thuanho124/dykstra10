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
    public class StudentsController : Controller
    {
        private readonly SchoolContext _context;

        public StudentsController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            // dykstra3
            /* The code passes a sortOrder parameter from the URL query string to the action method.
             * Then switch statement will check for what's needed to be sorted and sort the results based on what
             * users click on.
             * After that, the code is supposed to create an IQueryable variable, which is modified in the switch statement and
             * then the ToListAsync method will be called to convert the IQueryable object into a collection.
             * Lastly, ViewData will be called to configure the column heading hyperlinks with query string values.
             * Overall Purpose: to add sorting to Student index page based on what users click on and provide
             * heading hyperlinks for the view
             * 
             * Changes: add the sorting for GPA and Email, beside Last name and Enrollment date
             */

            // this code configures the view for current sort order, which keeps the current sort order
            // the same while clicking on next or previous page.
            ViewData["CurrentSort"] = sortOrder;

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "LastName_desc" : "";
            ViewData["DateSortParm"] =
                 sortOrder == "EnrollmentDate" ? "EnrollmentDate_desc" : "EnrollmentDate";
            
            // used for paging
            // if search string is changed during page, the page resets to number 1 page.
            // this makes sure there wont be different data displaying in the view when filtering
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            // configures the view with current filter string.
            // It is used to maintain the filter setting while paging.
            ViewData["CurrentFilter"] = searchString;
            var students = from s in _context.Students
                           select s;

            
            /* The string parameter "searchString" is added to the method in order to receive the string value
             * from a text box in the Index view.
             * searchString parameter pass to an action method and then if statement will check if string is not
             * null or empty, it will look for a name in database using where clause, which selects
             * only first name or last name that contains the search string. 
             * ViewData "CurrentFilter" configures the view for results.
             */
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }

            // if nothing sorted, then last name will be sorted first
            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "LastName";
            }

            // set descending false
            bool descending = false;

            // if sortOrder variables (such as LastName_desc, IntGPA_desc, etc) end with "_desc", 
            // then it will remove "_desc" by using substring, and keep the rest of words.
            // For example, LastName_desc will become LastName,
            // EnrollmentDate_desc => EnrollmentDate
            // Also, it set descending true to get sortOrder sorted
            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            // if descending = true, values in the view will be sorted by descending by passing sortOrder into EF Property method
            if (descending)
            {
                students = students.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }

            // if it is false, then  values in the view will be sorted by ascending by passing sortOrder into EF Property method
            else
            {
                students = students.OrderBy(e => EF.Property<object>(e, sortOrder));
            }


            // default page size = 3
            int pageSize = 3;
            // this code convert the list of student to a single page in a collection that supports paging.
            // Then it will be passed to the view.
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // dysktra2
            /* FirstOrDefaultAsync method is used to call student's id from database in order to 
             * get all information of a student.
             * Then the Include method is used to load the Student.Enrollments navigation property and then
             * the ThenInclude method would be called to load Enrollments.Course in order to get each enrollment,
             * which is related to a student's id.
             * AsNoTracking makes sure that the entity or information in the Details page will not be updated in the current context.
             * Overall Purpose: to get a single student's information from database and then connect it to courses, which are registered with
             * the student id.
             */
            var student = await _context.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,EmailAddress,YearRank,AverageGrade,EnrollmentDate")] Student student)
        {
            // dysktra2
            /* ASP.NET Core MVC model binder is used to change posted values to CLR types and then pass them
             * to action method. The model binder [Bind(...)] here is used to create an initial Student entity using values from Form collection.
             * Removing ID in Bind attribute from create method makes sure that users cannot set ID for a student
             * since ID, the primary key in SQL server, will set automatically.
             * The try-catch block displays an error message when there is an exception from DbUpateException caught
             * Purpose: The code is modified to create a Student entity based on property values in Database using
             * ASP.NET MVC model binder. Also, model binder helps enhance the security of the website.
             */
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);

        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // dysktra2
        /*FirstOrDefaultAsync method is used to call student's id from database in order to 
        * get all information of a student. In order words, it reads the existing entity from database.
        * Then TryUpdateModelAsync is used to update the retrieved entity called by FirstOrDefaultAsync method
        * based on the input in the posted form data.
        * The SaveChangeAsync method is then called to make sure that row values of the database will be updated.
        * Overall Purpose: to call the existing entity from the database, let user edit it and then save any changes
        * to the database.
        */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Student>(
                studentToUpdate,
                "",
                s => s.FirstName, s => s.LastName, s => s.EnrollmentDate, 
                s => s.EmailAddress, s => s.YearRank, s => s.AverageGrade))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }
        // dysktra2
        // GET: Students/Delete/5
        /* Again, FirstOrDefaultAsync retrieves the selected entity from Student based on id.
         * if an id or student is null or not found, or if any errors occuring when the database is updated, then:
         * the try-block will handle the error.
         * the HttpPost Delete method calls the HttpGet Delete, and pass a parameter that tell HttpGet that
         * there is an error. Then HttpGet will display an error message to users, and ask users to try again or cancel
         * the delete.
         * Overall purpose: display a delete view for the user. If there are any errors, ask the user to cancel or try again
         * If even there is not an error and users click the delete button, there may be nothing changed in the database
         * since this is a controller for the view.
         * The entity can be deleted properly in the database with HttpPost method.
         */
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        // dysktra2
        /* The FindAsync method is used to find a primary key. If a primary key is not found, then it redirects  URL to chosen name,
         * that user wants to delete. This means it fails to delete an entity
         * Then the try-catch block runs to remove an actual entity in the database and check for errors.
         * If there are any errors, then it will pass a parameter to HttpGet method to display an error message
         * Overall Purpose: to delete an actual entity in the database and check for errors. 
         */
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }



    }
}
