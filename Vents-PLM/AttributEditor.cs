using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vents_PLM
{
    public partial class AttributEditor : Form
    {
        SQLConnection sqlObj = SQLConnection.SQLObj;
        AttributeProperty newAtribute;
        AttributeProperty selectedAttribut;
        int index = 0;
        bool saveOrUpdate = false;

        public AttributEditor()
        {
            InitializeComponent();
            sqlObj.GetAttributes();

            //MakeTreeView();
            ui.MakeTreeView(treeView1, sqlObj.attributePropList, "Attribute");   
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItemAdd = new MenuItem("Создать атрибут", new System.EventHandler(this.AddPropMenuItem_Click));
                MenuItem menuItemDelete = new MenuItem("Удалить атрибут", new System.EventHandler(this.MenuItemDelete_Click));
                contextMenu.MenuItems.Add(menuItemAdd);
                contextMenu.MenuItems.Add(menuItemDelete);
                contextMenu.Show(treeView1, e.Location);
            }
            else if (e.Button == MouseButtons.Left)
            {
                selectedAttribut = sqlObj.attributePropList.Where(x => x.NAME.Equals(e.Node.Text)).ToList().FirstOrDefault();
                propertyGrid1.SelectedObject = selectedAttribut;
                index = sqlObj.attributePropList.IndexOf(selectedAttribut);
            }
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            selectedAttribut.DeleteAttribute(selectedAttribut);
            sqlObj.attributePropList.Remove(sqlObj.attributePropList.Where(x => x.ATTRIBUTE_ID == selectedAttribut.ATTRIBUTE_ID).First());
            // MakeTreeView();
            ui.MakeTreeView(treeView1, sqlObj.attributePropList, "Attribute");
        }  
        private void AddPropMenuItem_Click(object sender, EventArgs e)
        {
            saveOrUpdate = true;
            newAtribute = new AttributeProperty();
            propertyGrid1.SelectedObject = newAtribute;
            sqlObj.attributePropList.Add(newAtribute);
            //MakeTreeView();
            ui.MakeTreeView(treeView1, sqlObj.attributePropList, "Attribute");
        }


        private void cancelSavingNewAttr_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void saveNewAttr_Btn_Click(object sender, EventArgs e)
        {
            if (saveOrUpdate == false)
            {
                sqlObj.attributePropList[index] = selectedAttribut;
                selectedAttribut.UpdateAttribute(selectedAttribut);
            }
            else
            {
                newAtribute.SaveAttribute(newAtribute);
            }
            ui.MakeTreeView(treeView1, sqlObj.attributePropList, "Attribute");
        }


        //public void MakeTreeView()
        //{
        //    treeView1.Nodes.Clear();

        //    TreeNode attrNode = new TreeNode();
        //    TreeNode objNode = new TreeNode();
        //    attrNode.Name = "Attribute";
        //    attrNode.Text = "Атрибуты";
        //    attrNode.Name = "ObjType";
        //    objNode.Text = "Типы объектов";
        //    treeView1.Nodes.Add(attrNode);
        //    treeView1.Nodes.Add(objNode);

        //    TreeNode node = new TreeNode();
        //    foreach (var item in sqlObj.attributePropList)
        //    {
        //        node.Name = item.NAME;
        //        treeView1.Nodes[0].Nodes.Add(node.Name);
        //    }
        //    treeView1.ExpandAll();
        //}
        UserInterface2 ui = new UserInterface2();
        
    }
}