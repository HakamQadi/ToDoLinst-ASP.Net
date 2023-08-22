using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using TodoList.Models;

namespace TodoList.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<TodoModel> Records { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void OnGet()
        {
           Records= GetAllRecords();
        }
        private List<TodoModel> GetAllRecords()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var procedureCMD = new SqlCommand("GET_TASKS", connection);
                procedureCMD.CommandType=System.Data.CommandType.StoredProcedure; 
                  var tableData = new List<TodoModel>();
                using (SqlDataReader reader = procedureCMD.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableData.Add(new TodoModel
                        {
                            Id = reader.GetInt32(0),
                            Task = reader.GetString(1)
                        });
                    }
                    return tableData;
                }
            }
        }
    }
}