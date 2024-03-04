using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Security;
using static MyStore.Pages.Clients.IndexModel;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.adress = Request.Form["adress"];

            if(clientInfo.name.Length==0 || clientInfo.email.Length==0 ||
                clientInfo.phone.Length==0 || clientInfo.adress.Length==0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True";
                using(SqlConnection connection=new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO client " +
                        "(name, email, phone, adress) VALUES" +
                        "(@name, @email, @phone, @adress);";
                    using(SqlCommand command=new SqlCommand(sql,connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@adress", clientInfo.adress);

                        command.ExecuteNonQuery();
					}

                }

			}
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = "";clientInfo.email = "";
            clientInfo.phone = ""; clientInfo.adress = "";
            successMessage = "New Client Added Correctly";

            Response.Redirect("/Clients/Index");


		}
    }
}
