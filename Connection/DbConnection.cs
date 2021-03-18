using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SizzlingHot
{
    class DbConnection : ConnectionInfo,IConnectable
    {
        private static DbConnection instance;

        public bool CanConnect { get; set; }

        private static MySqlConnection mySqlConnection;
        private static MySqlDataReader dataReader;

        public static DbConnection GetInstance()
        {
            if(instance == null)
            {
                instance = new DbConnection();
            }
            return instance;
        }

        protected DbConnection() { }

        public void CloseConnection()
        {
            if (mySqlConnection.State == 0)
            {
                mySqlConnection.Close();
            }
        }
        public bool TryConnection()
        {
            string connectionString = ConnectionInfo.GetConnectionString();
            mySqlConnection = new MySqlConnection(connectionString);

            try
            {
                mySqlConnection.Open();
                CanConnect = true;
            } catch(Exception e)
            {
                Console.WriteLine("Cannot connect to database!");
                Console.WriteLine(e.Message);
                CanConnect = false;
            }

            return CanConnect;
        }

        public bool ConnectDB(string user, string password)
        {
            bool logged = false;

            if (CanConnect)
            {
                string sqlToLogin = $"SELECT user,password FROM account WHERE user LIKE'{user.ToLower()}' AND password LIKE'{password.ToLower()}'";
                MySqlCommand tryLogin = new MySqlCommand(sqlToLogin, mySqlConnection);

                if (mySqlConnection.State == 0)
                {
                    mySqlConnection.Open();
                }
                
                dataReader = tryLogin.ExecuteReader();

                if (dataReader.HasRows)
                {
                    Account.Login(new Account(user.ToLower(), password));

                    // Verify if is root acount
                    if(user.Equals(Account.ADMIN_USER)){
                        Account.IsRoot = true;
                    }

                    Console.WriteLine("LOGGED IN!");
                    logged = true;
                }
                else
                {
                    Console.WriteLine("This account dosen't exist!");
                    mySqlConnection.Close();
                }
            }
            return logged;
        }
    }
}
