using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using TodoList.Models;

namespace TodoList.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public TodoModel? Todo { get; set; }


        public IActionResult OnPost()
        {
            //search ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                   @$"INSERT INTO [ToDo].[dbo].[ToDoList] (text)
                      VALUES('{Todo.Task}')";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("./Index");

        }
    }
}
