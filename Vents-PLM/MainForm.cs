using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vents_PLM
{
    public partial class MainForm : Form
    {
        //controlls
        AddObjectControl addObjectControl = new AddObjectControl();
        MountingFrame.MountingFrameControl mountingFrame = new MountingFrame.MountingFrameControl();
        AirVentsCad.AirVentsCadControl airVentsCad = new AirVentsCad.AirVentsCadControl();
        Form1 residualMControl = new Form1();


        SQLConnection sqlObj = SQLConnection.SQLObj;
        static ObjectsProperty myObj;
        public static string selected_Object_Type;


        int indexOfControlOnPanel = 0;
        public static IMS_Object_Attributes selectedObj;
        public static bool EditOrNotEdit = false;

        public MainForm()
        {
            InitializeComponent();
            sqlObj.GetObjectsTypes();
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
            treeViewMain.Sort();
        }        

        private void treeViewMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            EditOrNotEdit = false;
            if (e.Button == MouseButtons.Left)
            {
                dataGridView1.Visible = true;
                myObj = sqlObj.objectsProperty.Where(x => x.NAME.Equals(e.Node.Text)).ToList().FirstOrDefault();

                    if (myObj != null)
                    {
                        sqlObj.GetObjectWithAttr(myObj);// получили список(objectsWithAttributes) обьектов по выбраному обьекту-типу

                        selected_Object_Type = myObj.OBJECT_TYPE;
                        if (indexOfControlOnPanel > 1)
                        {
                            tableLayoutPanel1.Controls.RemoveAt(indexOfControlOnPanel);// удаляем предыдущий контрол
                            indexOfControlOnPanel = 0;
                        }

                        if (sqlObj.objectsWithAttributes.Count != 0)
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.DataSource = sqlObj.objectsWithAttributes;
                        }
                        else { dataGridView1.DataSource = null; }

                        /////////////////////////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
            }

            else if (e.Button == MouseButtons.Right)
            {
                    if (indexOfControlOnPanel > 1)
                    {
                        tableLayoutPanel1.Controls.RemoveAt(indexOfControlOnPanel);
                        indexOfControlOnPanel = 0;
                    }
                    dataGridView1.Visible = false;
                    ContextMenu contextMenuForObjects = new ContextMenu();
                    MenuItem menuItemAdd = new MenuItem("Создать объект", new System.EventHandler(this.AddObjectMenuItem_Click));
                    contextMenuForObjects.MenuItems.Add(menuItemAdd);
                    contextMenuForObjects.Show(treeViewMain, e.Location);
            }


        }



        private void AddObjectMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Visible = false;
            ControlShow(myObj.NAME);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu contextMenuForObjectsEditing = new ContextMenu();
                MenuItem menuObjectUpdate = new MenuItem("Редактировать объект", new System.EventHandler(this.MenuObjectUpdate_Click));
                MenuItem menuObjectDelete = new MenuItem("Удалить объект", new System.EventHandler(this.MenuObjectDelete_Click));
                contextMenuForObjectsEditing.MenuItems.Add(menuObjectUpdate);
                contextMenuForObjectsEditing.MenuItems.Add(menuObjectDelete);
                contextMenuForObjectsEditing.Show(dataGridView1, e.Location);
            }
            else if (e.Button == MouseButtons.Left)
            {
                selectedObj = (IMS_Object_Attributes)dataGridView1.CurrentRow.DataBoundItem;                
            }
        }
        private void MenuObjectUpdate_Click(object sender, EventArgs e)
        {
            EditOrNotEdit = true;
            dataGridView1.Visible = false;
            ControlShow(myObj.NAME);
            addObjectControl.ControlEdit(myObj.NAME, selectedObj);
        }
        private void MenuObjectDelete_Click(object sender, EventArgs e)
        {
            SQLConnection.SQLObj.DeleteObject(selectedObj);
            treeViewMain.Refresh();
        }

        private void ControlShow(string objectTypeName)
        {
            switch (objectTypeName)
            {
                case "Вид изделия":
                    addObjectControl.BackColor = Color.BlueViolet;
                    addObjectControl.Dock = DockStyle.Top;
                    tableLayoutPanel1.Controls.Add(addObjectControl, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(addObjectControl);
                    break;

                case "AirVentsCad":
                    airVentsCad.BackColor = Color.LightSteelBlue;
                    airVentsCad.Dock = DockStyle.Top;
                    tableLayoutPanel1.Controls.Add(airVentsCad, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(airVentsCad);
                    break;

                case "Остатки":
                    residualMControl.BackColor = Color.Lavender;
                    residualMControl.Dock = DockStyle.Fill;
                    tableLayoutPanel1.Controls.Add(residualMControl, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(residualMControl);
                    break;

                case "Рама монтажная":
                    mountingFrame.BackColor = Color.PeachPuff;
                    mountingFrame.Dock = DockStyle.Fill;
                    tableLayoutPanel1.Controls.Add(mountingFrame, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(mountingFrame);
                    break;




                default: indexOfControlOnPanel = 0;
                    break;

            }
        }
        
    }
}