﻿using models;
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
            List<Icon> iconlist = new List<Icon>();

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
                        iconlist.Add(tinNhan);
                    }
                }
            }
            return iconlist;
        }
        public Icon TinNhaICon(int id)
        {
            Icon tinNhan_Icon = null;
            using (SqlConnection connection = DBUtils.GetDBConnection())
            {
                string selectQuery = "SELECT * FROM Icon WHERE id=@id";
                SqlCommand command = new SqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    tinNhan_Icon = new Icon
                    {
                        id = Convert.ToInt32(reader["id"]),
                        icons = reader["icons"].ToString(),
                        thoigianthem = Convert.ToDateTime(reader["thoigianthem"]),
                    };
                }
            }
            return tinNhan_Icon;
        }
    }
}