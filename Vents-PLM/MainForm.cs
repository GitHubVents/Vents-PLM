using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vents_PLM
{
    public partial class MainForm : Form
    {
        SQLConnection sqlObj = SQLConnection.SQLObj;
        static ObjectsProperty myObj;
        public static string selected_Object_Type;
        AddObjectControl addObjectControl;
        Form1 residualMControl = new Form1();
        int indexOfControlOnPanel = 0;

        dynamic listObjWithAttr;
        public MainForm()
        {
            InitializeComponent();
            sqlObj.GetObjectsTypes();
            sqlObj.GetObjects();
           // sqlObj.GetObjectWithAttr(out listObjWithAttr, myObj );
            MakeTreeView();
        }

        public void MakeTreeView()
        {
            treeViewMain.Nodes.Clear();

            TreeNode nodeInfo = new TreeNode();
            TreeNode objNode = new TreeNode();
            nodeInfo.Name = "Attribute";
            nodeInfo.Text = "Информационное пространство";
            objNode.Name = "Objects";
            objNode.Text = "Объекты";
            treeViewMain.Nodes.Add(nodeInfo);
            treeViewMain.Nodes[0].Nodes.Add(objNode);

            TreeNode node = new TreeNode();
            foreach (var item in sqlObj.objectsProperty)
            {
                node.Name = item.NAME;
                treeViewMain.Nodes[0].Nodes["Objects"].Nodes.Add(node.Name);
            }
            treeViewMain.ExpandAll();
        }        

        private void treeViewMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        { 
            if (indexOfControlOnPanel == 1) { tableLayoutPanel1.Controls.RemoveAt(1); }

            if (e.Button == MouseButtons.Left)
            {
                dataGridView1.Visible = true;
                myObj = sqlObj.objectsProperty.Where(x => x.NAME.Equals(e.Node.Text)).ToList().FirstOrDefault();
                sqlObj.GetObjectWithAttr(out listObjWithAttr, myObj);
                
                
                if (myObj != null)
                {
                    selected_Object_Type = myObj.OBJECT_TYPE;
                    if (myObj.NAME == "Остатки")
                    {
                        residualMControl.BackColor = Color.Lavender;
                        residualMControl.Dock = DockStyle.Fill;
                        tableLayoutPanel1.Controls.Add(residualMControl, 1, 0);
                        indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(residualMControl);
                    }
                    else
                    {
                        indexOfControlOnPanel = 0;
                        tableLayoutPanel1.Controls.Remove(residualMControl);
                    }

                    dataGridView1.DataSource = listObjWithAttr;
                }
                else
                {
                    indexOfControlOnPanel = 0;
                    dataGridView1.DataSource = null;
                }
                tableLayoutPanel1.Controls.Remove(addObjectControl);
            }
            else if (e.Button == MouseButtons.Right)
            {
                dataGridView1.Visible = false;
                ContextMenu contextMenuForObjects = new ContextMenu();
                MenuItem menuItemAdd = new MenuItem("Создать объект", new System.EventHandler(this.AddObjectMenuItem_Click));
                MenuItem menuItemDelete = new MenuItem("Удалить объект", new System.EventHandler(this.MenuItemDelete_Click));
                contextMenuForObjects.MenuItems.Add(menuItemAdd);
                contextMenuForObjects.MenuItems.Add(menuItemDelete);
                contextMenuForObjects.Show(treeViewMain, e.Location);
            }


        }

        private void MenuItemDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void AddObjectMenuItem_Click(object sender, EventArgs e)
        {            
            addObjectControl = new AddObjectControl();
            addObjectControl.BackColor = Color.BlueViolet;
            addObjectControl.Dock = DockStyle.Top;
            tableLayoutPanel1.Controls.Add(addObjectControl, 1, 0);
        }
    }
}