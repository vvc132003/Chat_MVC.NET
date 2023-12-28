using models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service
{
    public class NguoiDungService
    {
        public NguoiDung CheckThongTinDangNhap(string matkhau, string tendangnhap)
        {
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string CheckThongTinDangNhap = "SELECT * from  NguoiDung where tendangnhap=@tendangnhap and  matkhau=@matkhau ";
                using (SqlCommand command = new SqlCommand(CheckThongTinDangNhap, connection))
                {
                    command.Parameters.AddWithValue("@tendangnhap", tendangnhap);
                    command.Parameters.AddWithValue("@matkhau", matkhau);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NguoiDung nguoiDung = new NguoiDung()
                            {
                                id = Convert.ToInt32(reader["id"]),
                                hovaten = reader["hovaten"].ToString(),
                                matkhau = reader["matkhau"].ToString(),
                                trangthai = reader["trangthai"].ToString(),
                                email = reader["email"].ToString(),
                                anhdaidien = reader["anhdaidien"].ToString(),
                                tendangnhap = reader["tendangnhap"].ToString(),
                                ngaythamgia = (DateTime)reader["ngaythamgia"],
                            };
                            return nguoiDung;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public NguoiDung GetAllById(int id)
        {
            NguoiDung nguoiDung = new NguoiDung();
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string query = "SELECT * FROM NguoiDung WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nguoiDung.id = Convert.ToInt32(reader["id"]);
                            nguoiDung.tendangnhap = reader["tendangnhap"].ToString();
                            nguoiDung.matkhau = reader["matkhau"].ToString();
                            nguoiDung.email = reader["email"].ToString();
                            nguoiDung.hovaten = reader["hovaten"].ToString();
                            nguoiDung.anhdaidien = reader["anhdaidien"].ToString();
                            nguoiDung.trangthai = reader["trangthai"].ToString();
                            nguoiDung.ngaythamgia = Convert.ToDateTime(reader["ngaythamgia"]);
                        }
                    }
                }
            }

            return nguoiDung;
        }
        public List<NguoiDung> GetAllNguoiDungbyName(string hovaten)
        {
            List<NguoiDung> tinNhanList = new List<NguoiDung>();

            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                connection.Open();
                string query = "SELECT * FROM NguoiDung WHERE hovaten=@hovaten";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@hovaten", hovaten);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NguoiDung tinNhan = new NguoiDung()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            tendangnhap = reader["tendangnhap"].ToString(),
                            matkhau = reader["matkhau"].ToString(),
                            email = reader["email"].ToString(),
                            hovaten = reader["hovaten"].ToString(),
                            anhdaidien = reader["anhdaidien"].ToString(),
                            trangthai = reader["trangthai"].ToString(),
                            ngaythamgia = Convert.ToDateTime(reader["ngaythamgia"]),
                        };
                        tinNhanList.Add(tinNhan);
                    }
                }
            }

            return tinNhanList;
        }
    }
}
