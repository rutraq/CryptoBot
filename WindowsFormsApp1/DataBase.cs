﻿using Npgsql;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    class DataBase
    {
        public void Insert(int username, string userId, string key, string secretKey)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=isilo.db.elephantsql.com;Port=5432;User Id=eiezunnd;Password=xoGAEzBfYFPTBbGLvTtV9ZU3Sdx2jsnb;Database=eiezunnd;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand($"insert into Users (Username, User_id, Key, Secret_key)  values ('{username}', '{userId}', '{key}', '{secretKey}')", conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public List<string> Getinfo(int username)
        {
            List<string> info = new List<string>();
            NpgsqlConnection conn = new NpgsqlConnection("Server=isilo.db.elephantsql.com;Port=5432;User Id=eiezunnd;Password=xoGAEzBfYFPTBbGLvTtV9ZU3Sdx2jsnb;Database=eiezunnd;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand($"select user_id, key, secret_key from users where username='{username}'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                info.Add(dr.GetString(0));
                info.Add(dr.GetString(1));
                info.Add(dr.GetString(2));
            }
            conn.Close();
            return info;
        }
        public void Insert(int username, int sum)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=isilo.db.elephantsql.com;Port=5432;User Id=eiezunnd;Password=xoGAEzBfYFPTBbGLvTtV9ZU3Sdx2jsnb;Database=eiezunnd;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand($"update users set sum={sum} where username={username}", conn);
            command.ExecuteNonQuery();
            conn.Close();
        }
        public int GetSum(int username)
        {
            int sum = 0;
            NpgsqlConnection conn = new NpgsqlConnection("Server=isilo.db.elephantsql.com;Port=5432;User Id=eiezunnd;Password=xoGAEzBfYFPTBbGLvTtV9ZU3Sdx2jsnb;Database=eiezunnd;");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand($"select sum from users where username='{username}'", conn);
            NpgsqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                sum = dr.GetInt32(0);
            }
            conn.Close();
            return sum;
        }
    }
}
