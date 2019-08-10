using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models.SchoolViewModels;
using System.Data.Common;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {

        // dykstra3
        // class variable for the database context
        private readonly SchoolContext _context;
        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Name"] = "Thuan Ho";
            ViewData["Email"] = "hot4@uw.edu";
            ViewData["Github"] = "https://github.com/thuanho124";
            ViewData["Message"] = "If you have any questions, feel free to contact me";

            return View();
        }

        // dykstra3
        // controller for the about page
        // the code first select student count and year rank, instead of enrollment date in the tutorial
        // then use IQueyable to group student's count by year rank 
        public async Task<ActionResult> About()
        {
            // group student by their year rank

            // store the list into a collection of EnrollementDateGroup view model
            List<EnrollmentDateGroup> groups = new List<EnrollmentDateGroup>();

            // conn to connect with Data tables 
            var conn = _context.Database.GetDbConnection();

            try
            {
                await conn.OpenAsync(); // open the connection with database to get entity value

                // create a variable for command
                using (var command = conn.CreateCommand())
                {
                    // query to group student by their year rank
                    string query = "SELECT YearRank, COUNT(*) AS StudentCount " // select year rank column and count how many student in a rank
                        + "FROM Person " // from person table
                        + "WHERE Discriminator = 'Student' " // all people who are 'student' are selected
                        + "GROUP BY YearRank"; // group by year rank

                    command.CommandText = query; // let the system know that string 'query' is a query
                    
                    // send the CommandText to the connection and builds a SqlDataReader
                    DbDataReader reader = await command.ExecuteReaderAsync();  

                    // if it reads that there is a table with rows
                    if (reader.HasRows)
                    {
                        // then add rows YearRank and Student Count 
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup { YearRank = reader.GetString(0), StudentCount = reader.GetInt32(1) };
                            groups.Add(row);
                        }
                    }

                    reader.Dispose(); // releasing unmanaged resourses to improve performance
                }
            }
            // if there are not rows, then the connection with database will be closed and all actions are not successful
            finally
            {
                conn.Close();
            }

            // configure the view for model
            return View(groups);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
