using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System;
using Vents_PLM;

namespace ResidualMaterials
{
     class MyDtTable
     {
        string connection = "Data Source=pdmsrv;Initial Catalog=TaskDataBase;Persist Security Info=True;User ID=airventscad;Password=1";
        public MyDtTable()
        {
            dataList = new List<Balance>();
            date = GetCurrentDate();
        }
        
        SqlConnection objCon;
        
        public List<Balance> dataList;
        public List<Balance> dataListToView;// не удалять на него ссылаеться dynamic

        int LastBalanceId;
        int lastVersion;

        public static Balance itemToCutFrom;
        List<Balance> inputMaterial;
        public static bool residualType;
        public static bool isFieldsFilled;
        DateTime date;


        public static int widthDim { get; set; }
        public static int length { get; set; }
        public static int height { get; set; }
        public static int lengthWP { get; set; }
        public static int widthWP { get; set; }
        public static int name { get; set; }
        public static int version { get; set; }


        public void Load_Data(bool type)
        {
            //objCon = new SqlConnection(connection);
            //objCon.Open();
            SQLConnection.con.Open();
            //SqlCommand cmd = new SqlCommand("SELECT * FROM Balance " + type, objCon);
            SqlCommand cmd = new SqlCommand(@"select F_OBJECT_ID, [Длина], [Ширина], [Наименование]
                                                from	(select atr.F_NAME, objAtr.F_STRING_VALUE, temp.F_OBJECT_ID
		                                                from
			                                                 (select F_OBJECT_ID from IMS_OBJECTS where F_OBJECT_TYPE = 1001)
		                                                as temp, IMS_OBJECT_ATTRS objAtr, IMS_ATTRIBUTES atr 
		                                                where temp.F_OBJECT_ID = objAtr.F_OBJECT_ID
		                                                and objAtr.F_ATTRIBUTE_ID = atr.F_ATTRIBUTE_ID) as TEMPORAR
                                                pivot
                                                (
	                                                max(TEMPORAR.F_STRING_VALUE) for TEMPORAR.F_NAME IN ( [Длина], [Ширина], [Наименование])
                                                )
                                                AS TESTpIVOT");
            cmd.Connection = SQLConnection.con;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dataList.Add(new Balance()
                    {
                        //BalanceID = (int)reader["BalanceId"],
                        //Type = (bool)reader["Type"],
                        //Dim = (int)reader["Dim"],
                        //Length = (int)reader["Lenth"],
                        //W = (int)reader["W"],
                        //H = (int)reader["H"],
                        //Name = (int)reader["Name"],
                        //Version = (int)reader["Version"],
                        //Date = Convert.ToDateTime(reader["Data"])


                        BalanceID = Convert.ToInt32(reader["F_OBJECT_ID"].ToString()), 
                        //Type = (bool)reader["Type"],
                        //Dim = (int)reader["Dim"],
                        Length = Convert.ToInt32(reader["Длина"]),
                        //W = (int)reader["W"],
                        //H = (int)reader["H"],
                        //Name = (int)reader["Name"],
                        //Version = (int)reader["Version"],
                        //Date = Convert.ToDateTime(reader["Data"])

                    });
                }
            }
            //objCon.Close();
            cmd.Connection.Close();
        }
        public List<Balance> MakingDataList()
        {

            DataTable dataTable = new DataTable();
            List<string> columnList;
            if (MainForm.myObj != null)
            {
                dataTable = SQLConnection.SQLObj.GetObjectWithAttr(
                                                            SQLConnection.SQLObj.GetObjTypeIntByName(MainForm.myObj.NAME),
                                                            out columnList);
                ConvertingDtToList(dataTable);
            }


            List<Balance> result = new List<Balance>();
            Balance bal;

            var list = (from it in 
                            (from item in dataList /*where item.Type.Equals(residualType)*/ group item by item.Name ) 
                        orderby version descending select it);
            foreach (var item in list)
            {
                item.AsEnumerable();
                bal = new Balance() {BalanceID = item.Last().BalanceID, Type = item.Last().Type,
                                     Dim = item.Last().Dim, Length = item.Last().Length, W = item.Last().W,
                                     H = item.Last().H, Name = item.Last().Name, Version = item.Last().Version, Date = item.Last().Date};
                result.Add(bal);
            }
            
            return result;
        }
        public List<Balance> GetItemsofTheSameVersion()
        {
            var list = (from item in dataList where item.Name.Equals(itemToCutFrom.Name) select item).ToList();
            return list;
        }
        public void ConvertingDtToList(DataTable data)
        {
            foreach (var item in data.AsEnumerable())
            {
                dataList.Add(new Balance
                {
                    BalanceID = Convert.ToInt32(item["F_OBJECT_ID"]),
                    Dim = Convert.ToInt32((Convert.ToString(item["Диаметр"]) != string.Empty) ? item["Диаметр"] : null),
                    //H = Convert.ToInt32((Convert.ToString(item["Высота"]) != string.Empty) ? item["Высота"] : null),
                    Name = Convert.ToInt32((Convert.ToString(item["Наименование"]) != string.Empty) ? item["Наименование"] : null),
                    Length = Convert.ToInt32((Convert.ToString(item["Длина"]) != string.Empty) ? item["Длина"] : null),
                    Date = Convert.ToDateTime((Convert.ToString(item["Дата создания"]) != string.Empty) ? item["Дата создания"] : null),
                    W = Convert.ToInt32((Convert.ToString(item["Ширина"]) != string.Empty) ? item["Ширина"] : null)
                    //Version = Convert.ToInt32((item["Версия"]) != null ? item["Версия"] : null),
                    //Type = Convert.ToBoolean((item["Тип"]) != null ? item["Тип"] : null)
                });
            }
        }

        public void PushingDataInTable(string newName, string type, string newDim, string newlength, string newWidth, string newHeight )
        {
            if (isFieldsFilled == true)
            {
                if (CheckNameUniquenes())
                {
                   
                    if (residualType == true)
                    {
                        inputMaterial = ConvertInputDataToList(name, length, widthDim, height);
                    }
                    else
                    {
                        inputMaterial = ConvertInputDataToList(name, length, widthDim);
                    }


                    IMS_Object newbie = new IMS_Object();
                    IMS_Object_Attributes atr = new IMS_Object_Attributes();
                    SQLConnection.SQLObj.SaveNewObject(newbie);
                    SQLConnection.SQLObj.SaveAttributeForObject(atr, "Наименование", newName, false);
                    SQLConnection.SQLObj.SaveAttributeForObject(atr, "Дата создания", date.ToString(), false);
                    //SQLConnection.SQLObj.SaveAttributeForObject(atr, "Вид изделия", type, false);
                    if (MyDtTable.residualType)
                    {
                        SQLConnection.SQLObj.SaveAttributeForObject(atr, "Ширина", newWidth, false);
                        SQLConnection.SQLObj.SaveAttributeForObject(atr, "Высота", newHeight, false);
                    }
                    else
                    {
                        SQLConnection.SQLObj.SaveAttributeForObject(atr, "Диаметр", newDim, false);
                    }

                    //SaveNewMaterialDb(inputMaterial[0].Name, inputMaterial[0].Type, inputMaterial[0].Dim, inputMaterial[0].Length, inputMaterial[0].W, inputMaterial[0].H, 0);
                }
                else MessageBox.Show("Материал с таким именем уже существует!");
            }
        }

        private List<Balance> ConvertInputDataToList(int n, int l, int w, int h)
        {
            var list = new List<Balance>() { new Balance
            { 
                Name = n,
                Type = true,
                Length = l,
                W = w,
                H = h
            } };

            return list;
        }
        private List<Balance> ConvertInputDataToList(int n, int l, int dim)
        {
            var list = new List<Balance>() { new Balance
            {
                Name = n,
                Type = false,
                Length = l,
                Dim = dim
            } };

            return list;           
        }
        private bool CheckNameUniquenes()
        {
            var list = (from it in dataList where it.Name.Equals(name) select it).ToList();

            if (list.Count != 0)
            {
                return false;
            }
            else return true;
        }
        private int LastRowBalanceId()
        {
            int maxBalanceID;
            objCon = new SqlConnection(connection);
            objCon.Open();
            SqlDataReader reader;
            SqlCommand command = new SqlCommand("GetLastBalanceID", objCon);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@param1", SqlDbType.Int).Direction = ParameterDirection.Output;

            reader = command.ExecuteReader();
            maxBalanceID = (int)command.Parameters["@param1"].Value;
            objCon.Close();
            return maxBalanceID;
        }
        private void SaveNewMaterialDb(int name, bool type, int dim, int length, int w, int h, int version)
        {
            objCon.Open();
            SqlCommand save = new SqlCommand("AddMaretial");
            SqlDataReader reader;
            save.CommandType = CommandType.StoredProcedure;
            save.Connection = objCon;

            save.Parameters.AddWithValue("@name", name);
            save.Parameters.AddWithValue("@type", type);
            save.Parameters.AddWithValue("@dim", dim);
            save.Parameters.AddWithValue("@length", length);
            save.Parameters.AddWithValue("@w", w);
            save.Parameters.AddWithValue("@h", h);
            save.Parameters.AddWithValue("@version", version);
            save.Parameters.AddWithValue("@date", date);
            reader = save.ExecuteReader();

            objCon.Close();
            if (LastBalanceId == 0)
            {
                LastBalanceId = LastRowBalanceId();
            }
            else { LastBalanceId++; }
            dataList.Add(new Balance { BalanceID = LastBalanceId, Version = lastVersion, Type = type, Dim = dim, Length = length, W = w, H = h, Name = name, Date = date });
        }

        private DateTime GetCurrentDate()
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
        }

        public void CutOut()
        {
            int temp;

            bool possOrNot = CheckingWorkpieceLessThanResidual();
            
            if (possOrNot == true)
            {
                #region 
                int new_Length = 0;
                int new_Width = 0;
                lastVersion = LastVersion(itemToCutFrom.BalanceID);

                if (residualType == false)
                {
                    new_Length = itemToCutFrom.Length - lengthWP;                    
                }
                else
                {
                    
                L1:
                    if (itemToCutFrom.Length > lengthWP)
                    {
                        if (itemToCutFrom.W > widthWP)
                        {/////////////
                            new_Length = itemToCutFrom.Length - lengthWP;
                            new_Width = itemToCutFrom.W - widthWP;
                        }
                        else if (itemToCutFrom.W == widthWP)
                        {
                            new_Length = itemToCutFrom.Length - lengthWP;
                            new_Width = itemToCutFrom.W;
                        }
                        else if (itemToCutFrom.W < widthWP)
                        {/////////////
                            new_Width = itemToCutFrom.W;
                            new_Length = itemToCutFrom.Length - widthWP;
                        }
                    }

                    else if (itemToCutFrom.Length == lengthWP)
                    {
                        if (itemToCutFrom.W > widthWP)
                        {
                            new_Length = itemToCutFrom.Length;
                            new_Width = itemToCutFrom.W - widthWP;
                        }

                        else if (itemToCutFrom.W == widthWP)
                        {
                            new_Length = 0;
                            new_Width = 0;

                            //нужно удалять остаток
                        }
                    }

                    else if (itemToCutFrom.Length < lengthWP)
                    {
                        temp = lengthWP;
                        lengthWP = widthWP;
                        widthWP = temp;
                        goto L1;
                    }
                    //меняем значения длины/ширины обратно
                    temp = lengthWP;
                    lengthWP = widthWP;
                    widthWP = temp;                    
                }
#endregion
                SaveNewMaterialDb(itemToCutFrom.Name, itemToCutFrom.Type, itemToCutFrom.Dim, new_Length, new_Width, itemToCutFrom.H, lastVersion);
                lastVersion = 0;
            }
            else { MessageBox.Show("Невозможно вырезать заготовку. Параметры заготовки больше параметров остатка!"); }
        }

        private bool CheckingWorkpieceLessThanResidual()
        {
            int l = lengthWP;
            int w = widthWP;
            if (isFieldsFilled == true)
            {
                #region если поля заполнены
                if (residualType == true)
                {
                    int temp;

                    //определяем большую сторону заготовки
                    if (l >= w) { }
                    else
                    {
                        temp = l;
                        l = w;
                        w = temp;
                    }

                    if (itemToCutFrom.Length >= l)
                    {
                        if (itemToCutFrom.W >= w)
                        {
                            return true;
                        }
                        else
                        {
                            if (itemToCutFrom.Length >= w)
                            {
                                if (itemToCutFrom.W >= l)

                                { return true;
                                }

                            }
                            else
                                return false;
                        }

                        return false;
                    }
                    else
                    {
                        if (itemToCutFrom.W >= l)
                        {
                            if (itemToCutFrom.Length >= w)

                            { return true;
                            }

                            else return false;
                        }
                        return false;
                    }
                }

                else
                {
                    l = lengthWP;
                    if (itemToCutFrom.Length >= l) return true;
                    else return false;
                }
                #endregion
            }
            else
            {
                MessageBox.Show("Выберите остаток и заполните параметры заготовки!");
                return false;
            }
        }
        private int LastVersion(int balID)
        {
            objCon = new SqlConnection(connection);
            objCon.Open();
            SqlDataReader reader;
            SqlCommand command = new SqlCommand("GetLastVersion", objCon);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@balID", balID);
            command.Parameters.Add("@lastVersion", SqlDbType.Int).Direction = ParameterDirection.Output;
            reader = command.ExecuteReader();
            version = (int)command.Parameters["@lastVersion"].Value;
            objCon.Close();
            return version + 1;
        }
        
        private int RowToInsert()
        {
            int id = 0;
            
            var col = (from item in dataList where item.BalanceID.Equals(itemToCutFrom.BalanceID) select item);
            foreach (var item in col)
            {
                id = dataList.IndexOf(item);
            }
            return id;
        }

        private void DeleteItemFromBD()
        {
            objCon.Open();
            SqlCommand cancelCutting = new SqlCommand("CancelCutting", objCon);
            cancelCutting.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader;

            cancelCutting.Parameters.AddWithValue("name", itemToCutFrom.Name);
            cancelCutting.Parameters.AddWithValue("version", itemToCutFrom.Version);
            reader = cancelCutting.ExecuteReader();
            objCon.Close();

            dataList.RemoveAt(RowToInsert());
        }

        public void DeleteResidualMaterial()
        {
            if (itemToCutFrom.Version == 0)
            {
                DeleteItemFromBD();
            }
            else { MessageBox.Show("Нельзя удалить материал!"); }
        }
        public void CancelCuttingWP()
        {
            if (itemToCutFrom.Version > 0)
            {
                DeleteItemFromBD();
            }
            else { MessageBox.Show("Нельзя отменить вырезание!"); }

        }
        public void EditResidual()
        {
            if (itemToCutFrom.Version == 0)
            {
                objCon.Open();
                SqlCommand save = new SqlCommand("EditMaterial");
                SqlDataReader reader;
                save.CommandType = CommandType.StoredProcedure;
                save.Connection = objCon;

                save.Parameters.AddWithValue("@BalanceID", itemToCutFrom.BalanceID);
                save.Parameters.AddWithValue("@Name", name);
                save.Parameters.AddWithValue("@Dim", widthDim);
                save.Parameters.AddWithValue("@Lenth", length);
                save.Parameters.AddWithValue("@W", widthDim);
                reader = save.ExecuteReader();

                objCon.Close();

                int id = RowToInsert();
                dataList[id].Name = name;
                dataList[id].Dim = widthDim;
                dataList[id].Length = length;
                dataList[id].W = widthDim;
            }
            else { MessageBox.Show("Нельзя изменить параметры остатка!"); }
        }
    }
}