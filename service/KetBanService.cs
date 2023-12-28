using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class KetBanService
    {
        public List<KetBan> GetAllKetBanByIdNguoiDung(int idnguoidung)
        {
            List<KetBan> ketBanList = new List<KetBan>();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT  NguoiDung.id as idnguoidung,NguoiDung.anhdaidien, KetBan.id,KetBan.idnguoidung1,KetBan.idnguoigui,KetBan.idnguoidung2,KetBan.thoigianketban,KetBan.trangthai" +
                    "\r\nFROM KetBan\r\nLEFT JOIN NguoiDung ON KetBan.idnguoidung2 = NguoiDung.id \r\nWHERE idnguoidung1 = @idnguoidung AND KetBan.trangthai = N'đã kết bạn'\r\nUNION\r\n" +
                    "SELECT  NguoiDung.id as idnguoidung,NguoiDung.anhdaidien, KetBan.id,KetBan.idnguoidung1,KetBan.idnguoidung2,KetBan.idnguoigui,KetBan.thoigianketban,KetBan.trangthai" +
                    "\r\nFROM KetBan\r\nLEFT JOIN NguoiDung ON KetBan.idnguoidung1 = NguoiDung.id \r\nWHERE idnguoidung2 = @idnguoidung AND KetBan.trangthai = N'đã kết bạn'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnguoidung", idnguoidung);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        KetBan ketBan = new KetBan();
                        ketBan.id = Convert.ToInt32(reader["id"]);
                        ketBan.idnguoidung1 = Convert.ToInt32(reader["idnguoidung1"]);
                        ketBan.idnguoigui = Convert.ToInt32(reader["idnguoigui"]);
                        ketBan.idnguoidung = Convert.ToInt32(reader["idnguoidung"]);
                        ketBan.idnguoidung2 = Convert.ToInt32(reader["idnguoidung2"]);
                        ketBan.trangthai = reader["trangthai"].ToString();
                        ketBan.anhdaidien = reader["anhdaidien"].ToString();
                        ketBan.thoigianketban = Convert.ToDateTime(reader["thoigianketban"]);
                        ketBanList.Add(ketBan);
                    }

                    reader.Close();
                }
            }
            return ketBanList;
        }
        public List<KetBan> GetAllKetBanByIdNguoiDungMoiNhat(int idnguoidung1)
        {
            List<KetBan> ketBanList = new List<KetBan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = @"
                    SELECT NguoiDung.id AS idnguoidung, KetBan.id, KetBan.idnguoidung1, KetBan.idnguoidung2, KetBan.thoigianketban, KetBan.trangthai, MAX(tn.ThoiGianGui) AS ThoiGianTinNhan
                    FROM KetBan
                    LEFT JOIN NguoiDung ON (KetBan.idnguoidung1 = NguoiDung.id OR KetBan.idnguoidung2 = NguoiDung.id)
                    LEFT JOIN TinNhan tn ON (tn.idnguoidunggui = KetBan.idnguoidung1 AND tn.idnguoidungnhan = KetBan.idnguoidung2)
                                          OR (tn.idnguoidunggui = KetBan.idnguoidung2 AND tn.idnguoidungnhan = KetBan.idnguoidung1)
                    WHERE (KetBan.idnguoidung1 = @idnguoidung1 OR KetBan.idnguoidung2 = @idnguoidung1)
                      AND (NguoiDung.id != @idnguoidung1)
                    GROUP BY NguoiDung.id, KetBan.id, KetBan.idnguoidung1, KetBan.idnguoidung2, KetBan.thoigianketban, KetBan.trangthai
                    ORDER BY MAX(tn.ThoiGianGui) DESC, KetBan.thoigianketban DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnguoidung1", idnguoidung1);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        KetBan ketBan = new KetBan();
                        ketBan.id = Convert.ToInt32(reader["id"]);
                        ketBan.idnguoidung = Convert.ToInt32(reader["idnguoidung"]);
                        ketBan.idnguoidung1 = Convert.ToInt32(reader["idnguoidung1"]);
                        ketBan.idnguoidung2 = Convert.ToInt32(reader["idnguoidung2"]);
                        ketBan.trangthai = reader["trangthai"].ToString();
                        ketBan.thoigianketban = Convert.ToDateTime(reader["thoigianketban"]);
                        ketBanList.Add(ketBan);
                    }

                    reader.Close();
                }
            }

            return ketBanList;
        }
        public List<KetBan> GetAllKetBanById(int idnguoidung1, int idnguoidung2)
        {
            List<KetBan> ketBanList = new List<KetBan>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT *\r\nFROM KetBan\r\nWHERE (idnguoidung1 = @idnguoidung1 AND idnguoidung2 = @idnguoidung2 \r\n      OR idnguoidung1 = @idnguoidung2 AND idnguoidung2 = @idnguoidung1)\r\n ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idnguoidung1", idnguoidung1);
                    command.Parameters.AddWithValue("@idnguoidung2", idnguoidung2);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        KetBan ketBan = new KetBan();
                        ketBan.id = Convert.ToInt32(reader["id"]);
                        ketBan.idnguoidung1 = Convert.ToInt32(reader["idnguoidung1"]);
                        ketBan.idnguoigui = Convert.ToInt32(reader["idnguoigui"]);
                        ketBan.idnguoidung2 = Convert.ToInt32(reader["idnguoidung2"]);
                        ketBan.trangthai = reader["trangthai"].ToString();
                        ketBan.thoigianketban = Convert.ToDateTime(reader["thoigianketban"]);
                        ketBanList.Add(ketBan);
                    }

                    reader.Close();
                }
            }

            return ketBanList;
        }

        public void TKetBan(KetBan ketBan)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string insertQuery = "INSERT INTO KetBan (idnguoidung1, idnguoidung2,idnguoigui, trangthai, thoigianketban) VALUES (@IdNguoiDung1, @IdNguoiDung2,@idnguoigui, @TrangThai, @ThoiGianKetBan)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@IdNguoiDung1", ketBan.idnguoidung1);
                command.Parameters.AddWithValue("@IdNguoiDung2", ketBan.idnguoidung2);
                command.Parameters.AddWithValue("@idnguoigui", ketBan.idnguoigui);
                command.Parameters.AddWithValue("@TrangThai", ketBan.trangthai);
                command.Parameters.AddWithValue("@ThoiGianKetBan", ketBan.thoigianketban);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public KetBan ketbangetbyid(int idnguoidung1, int idnguoidung2)
        {
            KetBan ketBanResult = null;

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string selectQuery = "SELECT * FROM KetBan WHERE (idnguoidung1 = @IdNguoiDung1 AND idnguoidung2 = @IdNguoiDung2) OR (idnguoidung1 = @IdNguoiDung2 AND idnguoidung2 = @IdNguoiDung1)";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@IdNguoiDung1", idnguoidung1);
                command.Parameters.AddWithValue("@IdNguoiDung2", idnguoidung2);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ketBanResult = new KetBan
                    {
                        id = Convert.ToInt32(reader["id"]),
                        idnguoidung1 = Convert.ToInt32(reader["idnguoidung1"]),
                        idnguoidung2 = Convert.ToInt32(reader["idnguoidung2"]),
                        trangthai = reader["trangthai"].ToString(),
                        thoigianketban = Convert.ToDateTime(reader["thoigianketban"])
                    };
                }
            }
            return ketBanResult;
        }
        public void Xacnhanketban(int idNguoiDung1, int idNguoiDung2)
        {
            string query = "UPDATE KetBan " +
                      "SET trangthai = @trangthai " +
                      "WHERE (idnguoidung1 = @idNguoiDung1 AND idnguoidung2 = @idNguoiDung2) " +
                      "OR (idnguoidung1 = @idNguoiDung2 AND idnguoidung2 = @idNguoiDung1)";
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@trangthai", "đã kết bạn");
                    command.Parameters.AddWithValue("@idNguoiDung1", idNguoiDung1);
                    command.Parameters.AddWithValue("@idNguoiDung2", idNguoiDung2);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
