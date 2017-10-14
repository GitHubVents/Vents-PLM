using System;
using System.Collections.Generic;
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
    }
}