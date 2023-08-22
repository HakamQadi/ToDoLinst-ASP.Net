    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using TodoList.Models;

namespace TodoList.Pages
{
    public class DeleteModel : PageModel
    {

        private readonly IConfiguration _configuration;

        [BindProperty]
        public TodoModel? Todo { get; set; }

        public DeleteModel(IConfiguration configuration)
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

        public IActionResult OnPost(int id)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure; // Set command type to StoredProcedure
                cmd.CommandText = "DeleteToDo"; // Name of the stored procedure
                cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)).Value = id;

                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");
        }
    }
}
