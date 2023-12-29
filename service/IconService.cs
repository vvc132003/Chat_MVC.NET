using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class IconService
    {
        public List<Icon> GetallIcons()
        {
            List<Icon> tinNhanList = new List<Icon>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Icon ";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Icon tinNhan = new Icon()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            icons = reader["icons"].ToString(),
                            thoigianthem = Convert.ToDateTime(reader["thoigianthem"]),
                        };
                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }
    }
}