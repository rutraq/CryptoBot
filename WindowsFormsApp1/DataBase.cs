using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class DataBase
    {
        public void Insert(string username, string userId, string key, string secretKey)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=isilo.db.elephantsql.com;Port=5432;User Id=eiezunnd;Password=xoGAEzBfYFPTBbGLvTtV9ZU3Sdx2jsnb;Database=eiezunnd;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand($"insert into Users (Username, User_id, Key, Secret_key)  values ('{username}', '{userId}', '{key}', '{secretKey}')", conn);
            //NpgsqlDataReader dr = command.ExecuteReader();
            //while (dr.Read())
            //{
            //    int test = dr.GetInt32(1);
            //}
            command.ExecuteNonQuery();
            conn.Close();
        }
        public List<string> Getinfo(string username)
        {

        }
    }
}
