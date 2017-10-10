using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
        public List<IMS_Object> objects;

        private int lastObjectID = 0;
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
            objects = new List<IMS_Object>();
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
                });
            }
            con.Close();

            return attributePropList;
        }
        public List<ObjectsProperty> GetObjectsTypes()
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
                    NOTE = reader["F_NOTE"] == null ? string.Empty : reader["F_NOTE"].ToString(),
                    OBJECT_TYPE = reader["F_OBJECT_TYPE"].ToString()                
                });
            }
            con.Close();

            return objectsProperty;
        }
        public List<IMS_Object> GetObjects()
        {
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM IMS_OBJECTS", con);

            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                objects.Add(new IMS_Object
                {
                    OBJ_CREATE = reader["F_OBJ_CREATE"].ToString(),
                    OBJECT_TYPE = reader["F_OBJECT_TYPE"].ToString()                 
                });
            }
            con.Close();
            
            return objects;
        }


        public void GetObjectWithAttr(out dynamic list, ObjectsProperty myObj)
        {
            var listAnanymous = Empty(new {
               // F_NAME = string.Empty,
                F_OBJ_ID  =string.Empty,
                //F_OBJECT_Type = string.Empty,
                //F_INTEGER_VALUE = string.Empty,
                F_STRING_VALUE = string.Empty
                //F_DOUBLE_VALUE = string.Empty,
                //F_DATA_VALUE = string.Empty
            }).ToList();

            con.Open();
            SqlCommand command = new SqlCommand(@"SELECT objAttr.F_STRING_VALUE,
                                                obj.F_OBJECT_ID
                                                FROM (IMS_OBJECTS obj left join IMS_OBJECT_ATTRS objAttr ON obj.F_OBJECT_ID = objAttr.F_OBJECT_ID) 
                                                left join IMS_ATTRIBUTES attr on objAttr.F_ATTRIBUTE_ID = attr.F_ATTRIBUTE_ID WHERE obj.F_OBJECT_TYPE = " + myObj.OBJECT_TYPE, con);

            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader();
            while (reader.Read())
            {
                listAnanymous.Add(new
                {
                    //F_NAME = reader["F_NAME"].ToString(),
                    F_OBJ_ID = reader["F_OBJECT_ID"].ToString(),
                    //F_OBJECT_Type = reader["F_OBJECT_Type"].ToString(), 
                    //F_INTEGER_VALUE = reader["F_INTEGER_VALUE"].ToString(),
                    F_STRING_VALUE = reader["F_STRING_VALUE"].ToString()
                    //F_DOUBLE_VALUE = reader["F_DOUBLE_VALUE"].ToString(),
                    //F_DATA_VALUE = reader["F_DATE_VALUE"].ToString()
                });

            }
            con.Close();
            list = listAnanymous;
        }
        public static IEnumerable<T> Empty<T>(T dummyValue)
        {
            return Enumerable.Empty<T>();
        }

        public void SaveNewObject(IMS_Object myNewObject)
        { 
            SqlCommand save = new SqlCommand();
            SqlCommand getID = new SqlCommand();

            save.Connection = SQLConnection.con;
            save.CommandType = CommandType.StoredProcedure;

            getID.Connection = SQLConnection.con;
            getID.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader;
            save.CommandText = "IMS_ADD_OBJECT";
            getID.CommandText = "IMS_GET_MAX_OBJECT_ID";

            save.Parameters.AddWithValue("inOBJECT_TYPE", MainForm.selected_Object_Type);
            getID.Parameters.Add("@maxID", SqlDbType.Int).Direction = ParameterDirection.Output;      

            save.Connection.Open();
            reader = save.ExecuteReader();
            reader.Close();

            reader = getID.ExecuteReader();
            lastObjectID = (int)getID.Parameters["@maxID"].Value;

            reader.Close();
            save.Connection.Close();           
        }

        public void SaveAttributeForObject(IMS_Object myNewObject, string attrName, string value)
        {
            SqlCommand save = new SqlCommand();
            save.Connection = con;
            save.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader;
            save.CommandText = "IMS_ADD_OBJECT_ATTRS";


            save.Parameters.AddWithValue("inOBJECT_ID", lastObjectID);
            save.Parameters.AddWithValue("inATTRIBUTE_ID", attributePropList.Where(x=>x.NAME.Equals(attrName)).First().ATTRIBUTE_ID);
            save.Parameters.AddWithValue("in_INLIST_ID", 0 );
            save.Parameters.AddWithValue("inSTRING_VALUE", value);

            save.Connection.Open();
            reader = save.ExecuteReader();

            reader.Close();
            save.Connection.Close();

        }
        
    }
}