using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class NhanTinService
    {
        public async Task ThemTinNhan(int idnguoidunggui, int idnguoidungnhan, string NoiDung)
        {
            string query = "INSERT INTO TinNhan (idnguoidunggui, idnguoidungnhan, NoiDung, ThoiGianGui) " +
                           "VALUES (@idnguoidunggui, @idnguoidungnhan, @NoiDung, GETDATE())";

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnguoidunggui", idnguoidunggui);
                    command.Parameters.AddWithValue("@idnguoidungnhan", idnguoidungnhan);
                    command.Parameters.AddWithValue("@NoiDung", NoiDung);
                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        public List<NhanTin> GetAllTinNhanByIdGuiNhan(int idnguoidunggui, int idnguoidungnhan)
        {
            List<NhanTin> tinNhanList = new List<NhanTin>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM TinNhan WHERE (idnguoidunggui = @idnguoidunggui AND idnguoidungnhan = @idnguoidungnhan) OR (idnguoidunggui = @idnguoidungnhan AND idnguoidungnhan = @idnguoidunggui)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idnguoidunggui", idnguoidunggui);
                command.Parameters.AddWithValue("@idnguoidungnhan", idnguoidungnhan);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NhanTin tinNhan = new NhanTin();
                        tinNhan.id = reader.GetInt32(reader.GetOrdinal("id"));
                        tinNhan.idnguoidunggui = reader.GetInt32(reader.GetOrdinal("idnguoidunggui"));
                        tinNhan.idnguoidungnhan = reader.GetInt32(reader.GetOrdinal("idnguoidungnhan"));
                        tinNhan.NoiDung = reader.GetString(reader.GetOrdinal("NoiDung"));
                        tinNhan.ThoiGianGui = reader.GetDateTime(reader.GetOrdinal("ThoiGianGui"));
                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }
        public List<NhanTin> GetAllTinNhanById(int idnguoidunggui)
        {
            List<NhanTin> tinNhanList = new List<NhanTin>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM TinNhan WHERE idnguoidunggui=@idnguoidunggui";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idnguoidunggui", idnguoidunggui);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NhanTin tinNhan = new NhanTin();
                        tinNhan.id = reader.GetInt32(reader.GetOrdinal("id"));
                        tinNhan.idnguoidunggui = reader.GetInt32(reader.GetOrdinal("idnguoidunggui"));
                        tinNhan.idnguoidungnhan = reader.GetInt32(reader.GetOrdinal("idnguoidungnhan"));
                        tinNhan.NoiDung = reader.GetString(reader.GetOrdinal("NoiDung"));
                        tinNhan.ThoiGianGui = reader.GetDateTime(reader.GetOrdinal("ThoiGianGui"));
                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }
        public NhanTin GetAllByTinNhanMoi(int idnguoidunggui, int idnguoidungnhan)
        {
            NhanTin latestMessage = new NhanTin();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();

                string query = @"SELECT TOP 1 *
                        FROM TinNhan
                        WHERE 
                            (idnguoidunggui = @idnguoidunggui AND idnguoidungnhan = @idnguoidungnhan)
                            OR
                            (idnguoidunggui = @idnguoidungnhan AND idnguoidungnhan = @idnguoidunggui)
                        ORDER BY ThoiGianGui DESC";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@idnguoidunggui", idnguoidunggui);
                command.Parameters.AddWithValue("@idnguoidungnhan", idnguoidungnhan);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        latestMessage.id = reader.GetInt32(reader.GetOrdinal("id"));
                        latestMessage.idnguoidunggui = reader.GetInt32(reader.GetOrdinal("idnguoidunggui"));
                        latestMessage.idnguoidungnhan = reader.GetInt32(reader.GetOrdinal("idnguoidungnhan"));
                        latestMessage.NoiDung = reader.GetString(reader.GetOrdinal("NoiDung"));
                        latestMessage.ThoiGianGui = reader.GetDateTime(reader.GetOrdinal("ThoiGianGui"));
                    }
                }
            }

            return latestMessage;
        }
    }
}
