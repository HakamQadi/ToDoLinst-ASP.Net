using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using TodoList.Models;

namespace TodoList.Pages
{
    public class UpdateModel : PageModel
    {

        private readonly IConfiguration _configuration;

        [BindProperty]
        public TodoModel? Todo { get; set; }

        public UpdateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            Todo = GetById(id);
            return Page();
        }

        private TodoModel GetById(int id)
        {
            var toDoRecord = new TodoModel();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "SELECT * FROM [ToDo].[dbo].[ToDo] WHERE Id = @Id";
                tableCmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;

                using (SqlDataReader reader = tableCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        toDoRecord.Id = reader.GetInt32(0);
                        toDoRecord.Task = reader.GetString(1);
                    }
                }
            }
            return toDoRecord;

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure; // Set command type to StoredProcedure
                cmd.CommandText = "UpdateTask"; // Name of the stored procedure
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = Todo.Id;
                cmd.Parameters.Add(new SqlParameter("@Task", SqlDbType.NVarChar, -1)).Value = Todo.Task;

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");
        }


    }
}
