using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vents_PLM
{
    public partial class MainForm : Form
    {
        SQLConnection sqlObj = SQLConnection.SQLObj;
        ObjectsProperty myObj;

        Form1 tt = new Form1();
        int indexOfControlOnPanel = 0;

        public MainForm()
        {
            InitializeComponent();
            sqlObj.GetObjects();
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

                myObj = sqlObj.objectsProperty.Where(x => x.NAME.Equals(e.Node.Text)).ToList().FirstOrDefault();

                if (myObj != null)
                {
                    if (myObj.NAME == "Остатки")
                    {
                        tt.Dock = DockStyle.Left;
                        tt.BackColor = Color.LawnGreen;
                        tt.BackColor = Color.Lavender;
                        tt.Dock = DockStyle.Fill;
                        tableLayoutPanel1.Controls.Add(tt, 1, 0);
                        indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(tt);
                        MessageBox.Show(tt.GetHashCode().ToString());
                    }
                    else { indexOfControlOnPanel = 0; }
                }
                else { indexOfControlOnPanel = 0; }
            }

        }
    }
}