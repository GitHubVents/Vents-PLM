using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Vents_PLM
{
    class TreeView
    {
        public static SqlConnection con = new SqlConnection();
        SqlDataReader reader;
        string connectionString = "Data Source=pdmsrv;Initial Catalog=SWPlusDB;Persist Security Info=True;User ID=AirVentsCad;Password=1";
        public  List<AttributeProperty> attributePropList;

        public TreeView()
        {
            con.ConnectionString = connectionString;
            attributePropList = new List<AttributeProperty>();
        }


        public List<AttributeProperty> GetAttributs()
        {
            con.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM IMS_ATTRIBUTES", con);
            
            command.CommandType = CommandType.Text;

            reader = command.ExecuteReader();
            while(reader.Read())
            {
                attributePropList.Add(new AttributeProperty {
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

        public void MakeTreeView(System.Windows.Forms.TreeView tv)
        {
            tv.Nodes.Clear();

            TreeNode attrNode = new TreeNode();
            TreeNode objNode = new TreeNode();
            attrNode.Name = "Attribute";
            attrNode.Text = "Атрибуты";
            attrNode.Name = "ObjType";
            objNode.Text = "Типы объектов";
            tv.Nodes.Add(attrNode);
            tv.Nodes.Add(objNode);

            TreeNode node = new TreeNode();
            foreach (var item in attributePropList)
            {
                node.Name = item.NAME;
                tv.Nodes[0].Nodes.Add(node.Name);
            }
            tv.ExpandAll();
        }
    }
}