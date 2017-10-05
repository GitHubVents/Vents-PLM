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
        TreeView treeView = new TreeView();
        AttributeProperty newAtribute;
        AttributeProperty selectedAttribut;
        int index = 0;
        bool saveOrUpdate = false;

        public AttributEditor()
        {
            InitializeComponent();
            treeView.GetAttributs();
            treeView.MakeTreeView(treeView1);            
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
                    selectedAttribut = treeView.attributePropList.Where(x => x.NAME.Equals(e.Node.Text)).ToList().FirstOrDefault();
                    propertyGrid1.SelectedObject = selectedAttribut;
                    index = treeView.attributePropList.IndexOf(selectedAttribut);
            }
        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            selectedAttribut.DeleteAttribute(selectedAttribut);
            treeView.attributePropList.Remove(treeView.attributePropList.Where(x => x.ATTRIBUTE_ID == selectedAttribut.ATTRIBUTE_ID).First());
            treeView.MakeTreeView(treeView1);
        }  

        private void AddPropMenuItem_Click(object sender, EventArgs e)
        {
            saveOrUpdate = true;
            newAtribute = new AttributeProperty();
            propertyGrid1.SelectedObject = newAtribute;
            treeView.attributePropList.Add(newAtribute);
            treeView.MakeTreeView(treeView1);
        }


        private void cancelSavingNewAttr_Btn_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void saveNewAttr_Btn_Click(object sender, EventArgs e)
        {
            if (saveOrUpdate == false)
            {
                /*treeView.attributePropList[index].NAME = selectedAttribut.NAME;
                treeView.attributePropList[index].NOTE = selectedAttribut.NOTE;
                treeView.attributePropList[index].SHORT_NAME = selectedAttribut.SHORT_NAME;
                treeView.attributePropList[index].ALIAS = selectedAttribut.ALIAS;
                treeView.attributePropList[index].ATTRIBUTE_ID = selectedAttribut.ATTRIBUTE_ID;
                treeView.attributePropList[index].GUID = selectedAttribut.GUID;*/
                treeView.attributePropList[index] = selectedAttribut;
                selectedAttribut.UpdateAttribute(selectedAttribut);

            }
            else
            {
                newAtribute.SaveAttribute(newAtribute);
            }
                treeView.MakeTreeView(treeView1);
        }

        
    }
}