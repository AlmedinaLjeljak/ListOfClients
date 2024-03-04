using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Security;
using static MyStore.Pages.Clients.IndexModel;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
				String connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
                    String sql = "SELECT * FROM client WHERE id=@id"; 
					
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using(SqlDataReader reader=command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
								clientInfo.email = reader.GetString(2);
								clientInfo.phone = reader.GetString(3);
								clientInfo.adress = reader.GetString(4);
							}
                        }

						
					}

				}

			}
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
			clientInfo.name = Request.Form["name"];
			clientInfo.email = Request.Form["email"];
			clientInfo.phone = Request.Form["phone"];
			clientInfo.adress = Request.Form["adress"];

			
			if (clientInfo.id.Length==0 || clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
				clientInfo.phone.Length == 0 || clientInfo.adress.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}
			try
			{

				String connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE client " +
						"SET name=@name, email=@email, phone=@phone, adress=@adress WHERE id=@id";


					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@email", clientInfo.email);
						command.Parameters.AddWithValue("@phone", clientInfo.phone);
						command.Parameters.AddWithValue("@adress", clientInfo.adress);
						command.Parameters.AddWithValue("@id", clientInfo.id);

						command.ExecuteNonQuery();
					}

				}

			}
			catch(Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}
			Response.Redirect("/Clients/Index");

		}
    }
}
