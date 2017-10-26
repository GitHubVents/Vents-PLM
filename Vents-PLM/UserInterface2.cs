using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vents_PLM
{
    class UserInterface2
    {
        public void MakeTreeView(TreeView tv, dynamic list, string parentName)
        {
            tv.Nodes.Clear();

            TreeNode attrNode = new TreeNode();           
            attrNode.Name = "Attribute";
            attrNode.Text = "Атрибуты";
            tv.Nodes.Add(attrNode);
            

            TreeNode node = new TreeNode();
            foreach (var item in list)
            {
                node.Name = item.NAME;
                tv.Nodes[parentName].Nodes.Add(node.Name);
            }
            tv.ExpandAll();
            tv.Sort();
        }        
       
        public static void AttributesListComboBX(ComboBox cmbBX)
        {
            foreach (var item in SQLConnection.SQLObj.attributePropList)
            {
                cmbBX.Items.Add(item.NAME);
            }
        }
        public static List<IMS_Object_Attributes> GetSelectedObject(DataRowView selectedObj)
        {
            int countColumns = selectedObj.Row.Table.Columns.Count;
            string colName = string.Empty;
            List<IMS_Object_Attributes> listAttrValues = new List<IMS_Object_Attributes>();
            string value, id, objID;

            objID = selectedObj.Row.Field<string>("F_OBJECT_ID");

            for (int i = 1; i < countColumns; i++)
            {

                colName = selectedObj.Row.Table.Columns[i].ColumnName;

                value = selectedObj.Row.Field<string>(colName);

                id = SQLConnection.SQLObj.attributePropList.Where(x => x.NAME == colName).Select(x => x.ATTRIBUTE_ID).First<string>();


                listAttrValues.Add(new IMS_Object_Attributes { ATTRIBUTE_ID = id, OBJECT_ID = objID, STRING_VALUE = value });

            }
            return listAttrValues;
        }




    }
}