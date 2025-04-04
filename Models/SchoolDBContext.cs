﻿using MySql.Data.MySqlClient;

namespace _5125Cummulative1.Models
{
    public class SchoolDBContext
    {
        private static string User { get { return "root"; } }
        private static string Password { get { return "password"; } }
        private static string Database { get { return "schoool"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {
            get
            {
                return "server=" + Server
                     + ";user=" + User
                     + ";database=" + Database
                     + ";port=" + Port
                     + ";password=" + Password
                     + ";convert zero datetime=True";
            }
        }

        //public SchoolDBContext()
        //{
        //}

        public MySqlConnection AccessDatabase()
        {
            try
            {
                return new MySqlConnection(ConnectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database connection error: " + ex.Message);
                return null;
            }
        }
    }
}
