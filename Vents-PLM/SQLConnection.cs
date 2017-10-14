using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
        public List<IMS_Object_Attributes> objectsWithAttributes;
        public List<string> productTypeList;

        public static int lastObjectID = 0;
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
            objectsWithAttributes = new List<IMS_Object_Attributes>();
            productTypeList = new List<string>();
        }


        public List<AttributeProperty> GetAttributes()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM IMS_ATTRIBUTES", con);
            con.Open();

            SqlDataReader reader = command.ExecuteReader();
              while (reader.Read())
                {
                    attributePropList.Add(new AttributeProperty
                    {
                        ATTRIBUTE_ID = Convert.ToInt32(reader["F_ATTRIBUTE_ID"]).ToString(),
                        GUID = ((Guid)(reader["F_GUID"])).ToString(),
                        NAME = reader["F_NAME"] == null ? string.Empty : reader["F_NAME"].ToString(),
                        SHORT_NAME = reader["F_SHORT_NAME"] == null ? string.Empty : reader["F_SHORT_NAME"].ToString(),
                        NOTE = reader["F_NOTE"] == null ? string.Empty : reader["F_NOTE"].ToString(),
                        List = ConvertMultipleValue(reader)
                    });
                } 
            con.Close();
            reader.Close();
            return attributePropList;
        }
        private string ConvertMultipleValue(SqlDataReader reader)
        {
            string temp = "";
            switch (reader["F_MULTIPLE_VALUED"].ToString())
            {
                case "0":
                    temp = StringListConverter.s0;
                    break;
                case "1":
                    temp = StringListConverter.s1;
                    break;
                case "2":
                    temp = StringListConverter.s2;
                    break;
                case "3":
                    temp = StringListConverter.s3;
                    break;
            }
            return temp;
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
            reader.Close();

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
            reader.Close();

            return objects;
        }

        public void GetObjectWithAttr(ObjectsProperty myObj)
        {
            var list = GetListAttrByObjType(myObj); 
            string attributesNames = "";
            if (list.Count != 0)
            {
                foreach (var item in list)
                {
                    if (!((item == "") || item == null))
                    { attributesNames += "[" + item + "], "; }
                }
                attributesNames = attributesNames.Remove(attributesNames.Count() - 2, 2);
            }

            objectsWithAttributes.Clear();
            con.Open();
            SqlCommand command = new SqlCommand(@"select F_OBJECT_ID + " + attributesNames +
                                                @"from	(select atr.F_NAME, objAtr.F_STRING_VALUE, temp.F_OBJECT_ID
		                                                from
			                                                 (select F_OBJECT_ID from IMS_OBJECTS where F_OBJECT_TYPE = " + myObj.OBJECT_TYPE + @")
		                                                as temp, IMS_OBJECT_ATTRS objAtr, IMS_ATTRIBUTES atr 
		                                                where temp.F_OBJECT_ID = objAtr.F_OBJECT_ID
		                                                and objAtr.F_ATTRIBUTE_ID = atr.F_ATTRIBUTE_ID) as TEMPORAR
                                                pivot
                                                (
	                                                MIN(TEMPORAR.F_STRING_VALUE) for TEMPORAR.F_NAME IN ( " + attributesNames + @")
                                                )
                                                AS TESTpIVOT ", con);
            command.CommandType = CommandType.Text;

            ////////////////////////////////////////////////////////////////////////////////////////////////////


            //создавать класс динамически со свойствами 
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                objectsWithAttributes.Add( new IMS_Object_Attributes { 
                                    
                    //OBJECT_ID = reader["F_OBJECT_ID"].ToString(),
                    STRING_VALUE = reader[list[2]].ToString()

                });

            }
            con.Close();
            reader.Close();
        }


        public void SaveNewObject(IMS_Object myNewObject)
        { 
            SqlCommand save = new SqlCommand();
            save.Connection = con;
            save.CommandType = CommandType.StoredProcedure;
            save.CommandText = "IMS_ADD_OBJECT";            

                string temp;
                if (myNewObject.OBJECT_ID == null)
                {
                    temp = "-1";
                }
                else { temp = myNewObject.OBJECT_ID; }

            save.Parameters.AddWithValue("inID", temp);
            save.Parameters.AddWithValue("inOBJECT_TYPE", MainForm.selected_Object_Type);     
            save.Parameters.Add("@outOBJECT_ID", SqlDbType.Int).Direction = ParameterDirection.Output;     

            save.Connection.Open();
            reader = save.ExecuteReader();
            lastObjectID = (int)save.Parameters["@outOBJECT_ID"].Value;

            reader.Close();
            save.Connection.Close();           
        }

        public void SaveAttributeForObject(IMS_Object_Attributes selectedObj, string attrName, string value, bool editOrNotEdit)
        {
            SqlCommand save = new SqlCommand();
            save.Connection = con;
            save.CommandType = CommandType.StoredProcedure;

            save.CommandText = "IMS_ADD_OBJECT_ATTRS";

            if (editOrNotEdit == true)
            {
                save.Parameters.AddWithValue("inOBJECT_ID", selectedObj.OBJECT_ID);
            }
            else
            {
                save.Parameters.AddWithValue("inOBJECT_ID", lastObjectID);
            }
            save.Parameters.AddWithValue("inATTRIBUTE_ID", attributePropList.Where(x=>x.NAME.Equals(attrName)).First().ATTRIBUTE_ID);
            save.Parameters.AddWithValue("in_INLIST_ID", 0 );
            save.Parameters.AddWithValue("inSTRING_VALUE", value);

            save.Connection.Open();
            reader = save.ExecuteReader();

            reader.Close();
            save.Connection.Close();

        }

        public void DeleteObject(IMS_Object_Attributes selectedObject)
        {
            SqlCommand delete = new SqlCommand();
            delete.Connection = con;
            delete.CommandType = CommandType.StoredProcedure;
            delete.CommandText = "IMS_DELETE_OBJECT";

            delete.Parameters.AddWithValue("inOBJECT_ID", selectedObject.OBJECT_ID);

            delete.Connection.Open();
            reader = delete.ExecuteReader();

            reader.Close();
            delete.Connection.Close();

        }


        public void GetComboBxProductType()
        {
            SqlCommand get = new SqlCommand(@"select objAtr.F_STRING_VALUE from (select F_OBJECT_ID from IMS_OBJECTS where F_OBJECT_TYPE = 1007) as temp, IMS_OBJECT_ATTRS objAtr 
                                            where temp.F_OBJECT_ID = objAtr.F_OBJECT_ID", con);

            con.Open();
            
            get.CommandType = CommandType.Text;

            reader = get.ExecuteReader();
            while (reader.Read())
            {
                productTypeList.Add(reader["F_STRING_VALUE"].ToString());
            }

            con.Close();
            reader.Close();
        }


        public List<string> GetListAttrByObjType(ObjectsProperty myObj)
        {
            List<string> listAttrByObjType = new List<string>();
            con.Open();
            
            SqlCommand get = new SqlCommand(@"SELECT distinct(F_NAME) FROM IMS_ATTRIBUTES ATR
                                                 RIGHT JOIN
	                                                (SELECT F_ATTRIBUTE_ID FROM IMS_OBJECT_ATTRS objATTR
	                                                RIGHT JOIN
		                                                (SELECT F_OBJECT_ID FROM IMS_OBJECTS WHERE F_OBJECT_TYPE = " + myObj.OBJECT_TYPE + @") AS tempOBJ
		                                                 ON objATTR.F_OBJECT_ID = tempOBJ.F_OBJECT_ID) AS ATRid
		                                                 ON ATR.F_ATTRIBUTE_ID = ATRid.F_ATTRIBUTE_ID");
            get.Connection = con;
            get.CommandType = CommandType.Text;
            reader = get.ExecuteReader();
            while (reader.Read())
            {
                listAttrByObjType.Add(reader["F_NAME"].ToString());
            }
            con.Close();
            reader.Close();



            return listAttrByObjType;
        }




        public static IEnumerable<T> Empty<T>(T dummyValue)
        {
             return Enumerable.Empty<T>();
        }

    }
}