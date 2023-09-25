using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ProvaDjalma.Models
{
    public class Conexao : IDisposable
    {
        // Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd=myPassword;

        public MySqlConnection conn;

        private readonly string _server = "127.0.0.1";
        private readonly string _port = "3306";
        private readonly string _database = "bd_aula";
        private readonly string _uid = "root";
        private readonly string _pwd = "cesar964625";

        public Conexao()
        {
            Conectar();
        }

        private void Conectar()
        {
            string _strConn = "Server=" + _server;
            _strConn += "; Port=" + _port;
            _strConn += "; Database=" + _database;
            _strConn += "; Uid=" + _uid;
            _strConn += "; Pwd=" + _pwd;

            conn = new MySqlConnection(_strConn);
            try
            {
                conn.Open();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Dispose()
        {
            conn.Close();
            conn.Dispose();
        }



    }
}