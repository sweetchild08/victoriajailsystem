using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

namespace eserve2
{
    class db
    {
        private string server;
        private string database;
        private string uid;
        private string password;

        private MySqlConnection conn;
        public MySqlCommand comm = new MySqlCommand();
        public MySqlDataReader dr;
        public MySqlDataReader da;

        //Constructor
        public db()
        {
            init();
        }
        private void init()
        {
            server = Properties.Settings.Default.DBhost;
            database = Properties.Settings.Default.DBname;
            uid = Properties.Settings.Default.DBusername;
            password = Properties.Settings.Default.DBpassword;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            conn = new MySqlConnection(connectionString);
            this.comm.Connection = conn;


        }
        public void openconn()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                comm.Parameters.Clear();
                if(dr!=null)
                dr.Close();
            }
            conn.Open();

        }
        public bool testCon()
        {
            try
            {
                this.openconn();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public int delete(string table, int id)
        {
            openconn();
            comm.CommandText = "delete from `" + table + "` where id=@id";
            comm.Parameters.AddWithValue("@id", id);
            return comm.ExecuteNonQuery();
        }
        public static string[] views = {"front_view","left_view","right_view","whole_body" };

    }
}
