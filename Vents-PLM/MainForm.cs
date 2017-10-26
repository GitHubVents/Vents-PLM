using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Vents_PLM
{
    public partial class MainForm : Form
    {
        //controlls
        AddObjectControl addObjectControl;
        MountingFrame.MountingFrameControl mountingFrame;
        AirVentsCad.AirVentsCadControl airVentsCad;
        Form1 residualMControl;


        SQLConnection sqlObj = SQLConnection.SQLObj;
        public static ObjectsProperty myObj;
        public static string selected_Object_Type;


        int indexOfControlOnPanel = 0;

        public static List<IMS_Object_Attributes> selectedObj;

        public static bool EditOrNotEdit = false;// edit - true, save new - false
                                

        public MainForm()
        {
            InitializeComponent();
            sqlObj.GetObjectsTypes();
            sqlObj.GetObjects();
            MakeTreeView();
        }

        DataTable dt = null;
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
                myObj = sqlObj.objectsProperty.Where(x => x.NAME.Equals(e.Node.Text)).FirstOrDefault();
                if (myObj != null)
                {
                    selected_Object_Type = myObj.OBJECT_TYPE;
                    List<string> columnNames;

                    dt = sqlObj.GetObjectWithAttr(myObj.OBJECT_TYPE, out columnNames);// получили список(objectsWithAttributes) обьектов по выбраному обьекту-типу

                    if (indexOfControlOnPanel > 1)
                    {
                        tableLayoutPanel1.Controls.RemoveAt(indexOfControlOnPanel);// удаляем предыдущий контрол
                        indexOfControlOnPanel = 0;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = null;
                            
                        DataGridViewColumn[] column_array = new DataGridViewColumn[columnNames.Count];
                            
                        for (int cnt = 0; cnt < columnNames.Count; cnt++)
                        {
                            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                            col.Name = columnNames[cnt];
                            column_array[cnt] = col;
                        }

                        dataGridView1.Columns.AddRange(column_array);

                        

                        int i = 0;
                        foreach (var item in columnNames)
                        {
                            dataGridView1.Columns[item].DataPropertyName = dt.Columns[i].Caption;

                            i++;
                        }
                        
                        dataGridView1.DataSource = dt; 
                    }
                    else { dataGridView1.DataSource = null; }  
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
                MenuItem menuItemAdd = new MenuItem("Создать объект", new EventHandler(this.AddObjectMenuItem_Click));
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
                MenuItem menuObjectUpdate = new MenuItem("Редактировать объект", new EventHandler(this.MenuObjectUpdate_Click));
                MenuItem menuObjectDelete = new MenuItem("Удалить объект", new EventHandler(this.MenuObjectDelete_Click));
                contextMenuForObjectsEditing.MenuItems.Add(menuObjectUpdate);
                contextMenuForObjectsEditing.MenuItems.Add(menuObjectDelete);
                contextMenuForObjectsEditing.Show(dataGridView1, e.Location);
            }
            else if (e.Button == MouseButtons.Left)
            {
                DataRowView selectedRow = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
                if (selectedRow != null)
                {
                   selectedObj = UserInterface2.GetSelectedObject(selectedRow);
                }
                else { MessageBox.Show("null");}
            }
        }

        private void MenuObjectUpdate_Click(object sender, EventArgs e)
        {
            EditOrNotEdit = true;
            dataGridView1.Visible = false;
            ControlShow(myObj.NAME);
            switch (myObj.NAME)
            {
                case "Остатки":
                    residualMControl.FillForm1Residual(selectedObj);
                    break;

                case "Вид изделия":
                    addObjectControl.AddObjectControlFill(selectedObj);
                    break;

                default:

                    break;
            }
            
        }
        private void MenuObjectDelete_Click(object sender, EventArgs e)
        {
            List<string> columnNames;
            SQLConnection.SQLObj.DeleteObject(selectedObj);
            treeViewMain.Refresh();
            dt = sqlObj.GetObjectWithAttr(myObj.OBJECT_TYPE, out columnNames);// получили список(objectsWithAttributes) обьектов по выбраному обьекту-типу

            dataGridView1.DataSource = dt;
        }

        private void ControlShow(string objectTypeName)
        {
            switch (objectTypeName)
            {
                case "Вид изделия":
                    addObjectControl = new AddObjectControl();//инициализация контрола
                    addObjectControl.BackColor = Color.BlueViolet;
                    addObjectControl.Dock = DockStyle.Top;
                    tableLayoutPanel1.Controls.Add(addObjectControl, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(addObjectControl);
                    break;

                case "AirVentsCad":
                    airVentsCad = new AirVentsCad.AirVentsCadControl();
                    airVentsCad.BackColor = Color.LightSteelBlue;
                    airVentsCad.Dock = DockStyle.Top;
                    tableLayoutPanel1.Controls.Add(airVentsCad, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(airVentsCad);
                    break;

                case "Остатки":
                    residualMControl = new Form1();
                    residualMControl.BackColor = Color.Lavender;
                    residualMControl.Dock = DockStyle.Fill;
                    tableLayoutPanel1.Controls.Add(residualMControl, 1, 0);
                    indexOfControlOnPanel = tableLayoutPanel1.Controls.GetChildIndex(residualMControl);
                    ResidualMaterials.MyDtTable myDT = new ResidualMaterials.MyDtTable();
                    myDT.MakingDataList();
                    break;

                case "Рама монтажная":
                    mountingFrame = new MountingFrame.MountingFrameControl();
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