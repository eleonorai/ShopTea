using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ShopTea;

namespace ShopTea
{
    public class ShopTeaApp
    {
        private string connectionString;
        public ShopTeaApp(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AddTea(Tea tea)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Teas (Name, Description, Cost) VALUES (@Name, @Description, @Cost)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", tea.Name);
                    command.Parameters.AddWithValue("@Description", tea.Description);
                    command.Parameters.AddWithValue("@Cost", tea.Cost);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void EditTea(Tea tea)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Teas SET Name = @Name, Description = @Description, Cost = @Cost WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", tea.Id);
                    command.Parameters.AddWithValue("@Name", tea.Name);
                    command.Parameters.AddWithValue("@Description", tea.Description);
                    command.Parameters.AddWithValue("@Cost", tea.Cost);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTea(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Teas WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<Tea> GetTeasWithKeyword(string keyword)
        {
            List<Tea> teas = new List<Tea>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Teas WHERE Description LIKE '%' + @Keyword + '%'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", keyword);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Id"];
                            string name = (string)reader["Name"];
                            string description = (string)reader["Description"];
                            decimal cost = (decimal)reader["Cost"];

                            Tea tea = new Tea(id, name, description, cost);
                            teas.Add(tea);
                        }
                    }
                }
            }

            return teas;
        }

        public List<Tea> GetTeasInCostRange(decimal minCost, decimal maxCost)
        {
            List<Tea> teas = new List<Tea>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Teas WHERE Cost >= @MinCost AND Cost <= @MaxCost";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MinCost", minCost);
                    command.Parameters.AddWithValue("@MaxCost", maxCost);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["Id"];
                            string name = (string)reader["Name"];
                            string description = (string)reader["Description"];
                            decimal cost = (decimal)reader["Cost"];

                            Tea tea = new Tea(id, name, description, cost);
                            teas.Add(tea);
                        }
                    }
                }
            }

            return teas;
        }
    }

    public class Tea
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Cost { get; private set; }
        public Tea(int id, string name, string description, decimal cost)
        {
            Id = id;
            Name = name;
            Description = description;
            Cost = cost;
        }
    }
}






public class Program
{
    public static void Main()
    {
        string connectionString = "Data Source=LAPTOP-8HJ85GUS\\ODESI;Initial Catalog=ShopTea;Integrated Security=True;";

        var teaShopApp = new ShopTeaApp(connectionString);

        var tea1 = new Tea(1, "Green Tea", "Delicious green tea with cherry flavor", 10.99m);
        teaShopApp.AddTea(tea1);

        var tea2 = new Tea(2, "Black Tea", "Premium black tea with cherry essence", 12.99m);
        teaShopApp.EditTea(tea2);

        int teaId = 3;
        teaShopApp.DeleteTea(teaId);

        string keyword = "cherry";
        List<Tea> teasWithKeyword = teaShopApp.GetTeasWithKeyword(keyword);

        decimal minCost = 5.00m;
        decimal maxCost = 15.00m;
        List<Tea> teasInCostRange = teaShopApp.GetTeasInCostRange(minCost, maxCost);
    }
}

