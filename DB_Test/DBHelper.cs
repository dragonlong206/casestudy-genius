using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

namespace AdvisingSystem.Lib
{
    // Exception for database connection error
    public class DBHelperException : Exception
    {
        private string _message;
        public DBHelperException(string message)
        {
            _message = message;
        }

        public string Message
        {
            get { return _message; }
        }
    }

    // Database helper class for reading and writing data to database
    public class DBHelper
    {
        // Singleton: keep unique instance of database connection throughout application
        static DBHelper _dbHelper = null;
        private DBHelper()
        {
        }

        // sql connection member
        SqlConnection _conn;

        // Reading and writing methods
        public Object ExecuteScalar(SqlCommand command)
        {
            try
            {
                command.Connection = _conn;
                return command.ExecuteScalar();
            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            try
            {
                command.Connection = _conn;
                return command.ExecuteReader();
            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        public int ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                command.Connection = _conn;
                return command.ExecuteNonQuery();
            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        public DataTable RetrieveTable(SqlCommand command)
        {
            try
            {
                command.Connection = _conn;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        public DataSet RetrieveDataSet(SqlCommand command)
        {
            try
            {
                command.Connection = _conn;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                return dataSet;
            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        // Opening and closing methods
        public void Open()
        {
            try
            {
                if(_conn.State == ConnectionState.Closed)
                    _conn.Open();

            }

            catch (Exception currentException)
            {
                throw new DBHelperException(currentException.Message);
            }
        }

        public void Close()
        {
            _conn.Close();
        }

        // static method to retrieve unique instance of database connection
        public static DBHelper Connection
        {
            get
            {
                try
                {
                    if (_dbHelper == null)
                    {
                        _dbHelper = new DBHelper();
                        _dbHelper._conn = new SqlConnection(ConfigurationManager.ConnectionStrings["primaryConnection"].ToString());
                        _dbHelper._conn.Close();
                    }
                    else
                    {
                        if (_dbHelper._conn.State == ConnectionState.Open)
                            _dbHelper._conn.Close();
                    }
                    return _dbHelper;
                }

                catch (Exception currentExeption)
                {
                    throw new DBHelperException(currentExeption.Message);
                }
            }
        }

        public static void SetConnection(string connection)
        {
            if (_dbHelper == null)
            {
                _dbHelper = new DBHelper();
            }
            _dbHelper._conn = new SqlConnection(connection);
        }
    }
}
