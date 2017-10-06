using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;


namespace ResidualMaterials
{
    class UserInterface
    {
        public void ManagingUserInterface(Label l6, Label l3, Label l4, TextBox txt1, TextBox txt3)
        {
            if (MyDtTable.residualType == false)
            {
                MyDtTable.residualType = false;
                l3.Visible = false;
                l6.Visible = false;
                l4.Text = "Диаметр";
                txt1.Visible = false;
                txt3.Visible = false;
            }
            else
            {
                MyDtTable.residualType = true;
                l3.Visible = true;
                l6.Visible = true;
                l4.Text = "Ширина листа/полосы";
                txt1.Visible = true;
                txt3.Visible = true;
            }
        }

        public void CheckIfFieldsAreFilled(DataGridView dgv, TextBox width, TextBox length)//for cutting
        {
            if (MyDtTable.residualType == true)
            {
                if (!string.IsNullOrEmpty(width.Text) && !string.IsNullOrEmpty(length.Text) && dgv.SelectedRows != null)

                { MyDtTable.isFieldsFilled = true; }
                else MyDtTable.isFieldsFilled = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(length.Text) && dgv.SelectedRows != null)

                { MyDtTable.isFieldsFilled = true; }
                else MyDtTable.isFieldsFilled = false;
            }
        }
        public void CheckIfFieldsAreFilled(TextBox name, TextBox width, TextBox length, TextBox height)//for insertion
        {
            if (MyDtTable.residualType == true)
            {
                if (!string.IsNullOrEmpty(name.Text) && !string.IsNullOrEmpty(width.Text) && !string.IsNullOrEmpty(length.Text) && !string.IsNullOrEmpty(height.Text))

                {
                    MyDtTable.isFieldsFilled = true;
                }
                else MyDtTable.isFieldsFilled = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(name.Text) && !string.IsNullOrEmpty(length.Text) && !string.IsNullOrEmpty(width.Text))

                {
                    MyDtTable.isFieldsFilled = true;
                }
                else MyDtTable.isFieldsFilled = false;
            }
        }

        public void ConvTextToDecimal(TextBox name, TextBox w, TextBox l, TextBox h)//for input
        {
            if (MyDtTable.isFieldsFilled == true)
            {                
                if (MyDtTable.residualType == false)
                {
                    MyDtTable.widthDim = Convert.ToInt32(w.Text);
                    MyDtTable.length = Convert.ToInt32(l.Text);
                }
                else
                {
                    MyDtTable.widthDim = Convert.ToInt32(w.Text);
                    MyDtTable.length = Convert.ToInt32(l.Text);
                    MyDtTable.height = Convert.ToInt32(h.Text);
                }
                MyDtTable.name = Convert.ToInt32(name.Text);
            }
            else MessageBox.Show("Заполнены не все параметры остатка!");
        }
        public void ConvTxtToDecimal(TextBox w, TextBox l)//for cut
        {
            if (MyDtTable.isFieldsFilled == true)

            {
                if (MyDtTable.residualType == false)
                {
                    MyDtTable.lengthWP = Convert.ToInt32(l.Text);
                }
                else
                {
                    MyDtTable.widthWP = Convert.ToInt32(w.Text);
                    MyDtTable.lengthWP = Convert.ToInt32(l.Text);
                }
            }
            else { }
        }


        public void CheckType(ComboBox bx)
        {
            if (bx.SelectedIndex == 0) { MyDtTable.residualType = false; } else MyDtTable.residualType = true;
        }

        public Balance GetSelectedBalance(DataGridView dgv)
        {
            Balance currentObject = (Balance)dgv.CurrentRow.DataBoundItem;
            
            return currentObject;
        }

        public void AddParametersOfDeletedWPLength(dynamic j, DataGridView dgv, string colName)
        {
            dgv.Columns.Add(colName, colName);
            List<Balance> list = j.GetItemsofTheSameVersion();

            for (int i = 0; i < list.Count-1; i++)
            {
                dgv[colName, i].Value = list[i].Length - list[i+1].Length;
            }
        }
        public void AddParametersOfDeletedWP(dynamic j, DataGridView dgv, string colNameLength, string colNameWidth)
        {
            dgv.Columns.Add(colNameLength, colNameLength);
            dgv.Columns.Add(colNameWidth, colNameWidth);

            List<Balance> list = j.GetItemsofTheSameVersion();

            for (int i = 0; i < list.Count - 1; i++)
            {
                dgv[colNameLength, i].Value = list[i].Length - list[i + 1].Length;
                dgv[colNameWidth, i].Value = list[i].W - list[i + 1].W;
            }
        }

        public void FillTxtBoxes(TextBox name, TextBox widthDim, TextBox length, TextBox height)
        {
            name.Text = MyDtTable.itemToCutFrom.Name.ToString();
            length.Text = MyDtTable.itemToCutFrom.Length.ToString();

            if (MyDtTable.residualType)
            {
                widthDim.Text = MyDtTable.itemToCutFrom.W.ToString();
                height.Text = MyDtTable.itemToCutFrom.H.ToString();
            }
            else widthDim.Text = MyDtTable.itemToCutFrom.Dim.ToString();     
        }

        public void ClearTxtBoxes(TextBox name, TextBox widthDim, TextBox length, TextBox height, TextBox length2, TextBox width)
        {
            name.Text = "";
            widthDim.Text = "";
            length.Text = "";
            height.Text = "";
            length2.Text = "";
            width.Text = "";

        }
        public void SuperPuper(DataGridView dgv, dynamic o)
        {
            dgv.Columns.Clear();
            o.dataListToView = o.MakingDataList();

            dgv.AutoGenerateColumns = false;
            dgv.AutoSize = true;

            string[] columnName = null;

            if (MyDtTable.residualType == false)
            {
                columnName = new string[] { "№", "Диаметр", "Длина", "Версия", "Дата добавления" };
            }
            else { columnName = new string[] { "№", "Длина", "Ширина", "Толщина", "Версия", "Дата добавления"}; }


            DataGridViewColumn[] column_array = new DataGridViewColumn[columnName.Length];
            for (int cnt = 0; cnt < columnName.Length; cnt++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = columnName[cnt];
                column_array[cnt] = col;
            }
            dgv.Columns.AddRange(column_array);

            var bindingList = new BindingList<Balance>(o.dataListToView);
            var source = new BindingSource(bindingList, null);


            dgv.Columns["№"].DataPropertyName = "Name";
            dgv.Columns["Версия"].DataPropertyName = "Version";
            dgv.Columns["Дата добавления"].DataPropertyName = "Date";
            if (MyDtTable.residualType == false)
            {
                dgv.Columns["Диаметр"].DataPropertyName = "Dim";
                dgv.Columns["Длина"].DataPropertyName = "Length";
            }
            else
            {
                dgv.Columns["Длина"].DataPropertyName = "Length";
                dgv.Columns["Ширина"].DataPropertyName = "W";
                dgv.Columns["Толщина"].DataPropertyName = "H";
            }
            dgv.DataSource = source;
        }
        public void SuperPuper2(DataGridView dgv, dynamic oDT, dynamic oUsInter)
        {
            dgv.Columns.Clear();
            oDT.dataListToView = oDT.GetItemsofTheSameVersion();

            dgv.AutoGenerateColumns = false;
            dgv.AutoSize = true;
            string[] columnName = null;

            if (MyDtTable.residualType == false)
            {
                columnName = new string[] { "№", "Диаметр", "Длина", "Версия", "Дата вырезания" };
            }
            else { columnName = new string[] { "№", "Длина", "Ширина", "Толщина", "Версия", "Дата вырезания" }; }

            DataGridViewColumn[] column_array = new DataGridViewColumn[columnName.Length];
            for (int cnt = 0; cnt < columnName.Length; cnt++)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = columnName[cnt];
                column_array[cnt] = col;
            }
            dgv.Columns.AddRange(column_array);

            var bindingList = new BindingList<Balance>(oDT.dataListToView);
            var source = new BindingSource(bindingList, null);
            dgv.Columns["№"].DataPropertyName = "Name";
            dgv.Columns["Версия"].DataPropertyName = "Version";
            dgv.Columns["Дата вырезания"].DataPropertyName = "Date";

            dgv.DataSource = source;
            if (MyDtTable.residualType == false)
            {
                dgv.Columns["Диаметр"].DataPropertyName = "Dim";
                dgv.Columns["Длина"].DataPropertyName = "Length";
                oUsInter.AddParametersOfDeletedWPLength(oDT, dgv, "Длина вырезаной заготовки");
            }
            else
            {
                dgv.Columns["Длина"].DataPropertyName = "Length";
                dgv.Columns["Ширина"].DataPropertyName = "W";
                dgv.Columns["Толщина"].DataPropertyName = "H";
                oUsInter.AddParametersOfDeletedWP(oDT, dgv, "Длина вырезаной заготовки", "Ширина вырезаной заготовки");
            }
        }
    }
}
