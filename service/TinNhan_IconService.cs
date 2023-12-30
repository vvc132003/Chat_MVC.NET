using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class TinNhan_IconService
    {
        public List<TinNhan_Icon> GetallTinNhanIcon()
        {
            List<TinNhan_Icon> tinNhan_Iconlist = new List<TinNhan_Icon>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM TinNhan_Icon ";
                SqlCommand command = new SqlCommand(query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TinNhan_Icon tinNhan_Icon = new TinNhan_Icon()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            idntinnhan = Convert.ToInt32(reader["idntinnhan"]),
                            idicon = Convert.ToInt32(reader["idicon"]),
                            thoigian = Convert.ToDateTime(reader["thoigian"]),
                        };
                        tinNhan_Iconlist.Add(tinNhan_Icon);
                    }
                }
            }

            return tinNhan_Iconlist;
        }
        public TinNhan_Icon TinNhaICon(int idntinnhan)
        {
            TinNhan_Icon tinNhan_Icon = null;

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string selectQuery = "SELECT * FROM TinNhan_Icon WHERE idntinnhan=@idntinnhan";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@idntinnhan", idntinnhan);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    tinNhan_Icon = new TinNhan_Icon
                    {
                        id = Convert.ToInt32(reader["id"]),
                        idntinnhan = Convert.ToInt32(reader["idntinnhan"]),
                        idicon = Convert.ToInt32(reader["idicon"]),
                        thoigian = Convert.ToDateTime(reader["thoigian"]),
                    };
                }
            }
            return tinNhan_Icon;
        }
        public async Task ThemTinNhanIcon(int idntinnhan, int idicon)
        {
            string query = "INSERT INTO TinNhan_Icon (thoigian, idntinnhan, idicon) VALUES (@thoigian, @idntinnhan, @idicon)";

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@thoigian", DateTime.Now);
                    command.Parameters.AddWithValue("@idntinnhan", idntinnhan);
                    command.Parameters.AddWithValue("@idicon", idicon);
                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}