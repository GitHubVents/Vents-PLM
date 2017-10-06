using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Vents_PLM
{
    class SQLConnection
    {
        public static SqlConnection con = new SqlConnection();
        SqlDataReader reader;
        string connectionString = "Data Source=pdmsrv;Initial Catalog=SWPlusDB;Persist Security Info=True;User ID=AirVentsCad;Password=1";
        public  List<AttributeProperty> attributePropList;
        public List<ObjectsProperty> objectsProperty;

        private static SQLConnection instance = new SQLConnection();
        public static SQLConnection SQLObj
        {
            get { return instance; }
            set
            {
                if (instance == null)
                { instance = new SQLConnection(); }
            }
        }

        private SQLConnection()
        {
            con.ConnectionString = connectionString;
            attributePropList = new List<AttributeProperty>();
            objectsProperty = new List<ObjectsProperty>();
        }


        public List<AttributeProperty> GetAttributes()
        {
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM IMS_ATTRIBUTES", con);
            
            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader();
            while(reader.Read())
            {
                attributePropList.Add(new AttributeProperty
                {
                    ATTRIBUTE_ID = Convert.ToInt32(reader["F_ATTRIBUTE_ID"]).ToString(),
                    GUID = ((Guid)(reader["F_GUID"])).ToString(),
                    NAME = reader["F_NAME"] == null ? string.Empty : reader["F_NAME"].ToString(),
                    SHORT_NAME = reader["F_SHORT_NAME"] == null ? string.Empty : reader["F_SHORT_NAME"].ToString(),
                    NOTE = reader["F_NOTE"] == null ? string.Empty : reader["F_NOTE"].ToString()
                    //List = (Listik)reader["F_MULTIPLE_VALUES"]
                  
                });
            }
            con.Close();

            return attributePropList;
        }
        public List<ObjectsProperty> GetObjects()
        {
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM IMS_OBJECT_TYPES", con);

            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                objectsProperty.Add(new ObjectsProperty
                {
                    TYPE_NAME  = reader["F_OBJ_TYPE_NAME"].ToString(),
                    GUID = ((Guid)(reader["F_GUID"])).ToString(),
                    NAME = reader["F_OBJ_NAME"].ToString(),
                    SHORT_NAME = reader["F_SHORT_NAME"] == null ? string.Empty : reader["F_SHORT_NAME"].ToString(),
                    NOTE = reader["F_NOTE"] == null ? string.Empty : reader["F_NOTE"].ToString()                    
                });
            }
            con.Close();

            return objectsProperty;
        }        
    }
}