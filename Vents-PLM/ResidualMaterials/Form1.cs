using System;
using System.Windows.Forms;
using ResidualMaterials;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Vents_PLM
{
    public partial class Form1 : UserControl
    {
        public Form1()
        {
            InitializeComponent();
            dt = new MyDtTable();
            usInter = new UserInterface();

            GetComboValues();
            comboBox1.SelectedIndex = 0;
            dataGridView.DataSource = dt.dataList;
            //usInter.SuperPuper(dataGridView, dt);
        }
        MyDtTable dt;
        DataTable dataTable;
        UserInterface usInter;
        List<string> columnList;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            usInter.ClearTxtBoxes(txtName, txtWidthDim, txtLength, txtH, txtLengthWP, txtWidthWP);
            usInter.CheckType(comboBox1);
            usInter.ManagingUserInterface(lblWidthWP, lblH, lblWidthDim, txtWidthWP, txtH);

            dataGridView.DataSource = dt.dataList;
            //usInter.SuperPuper(dataGridView, dt);
            dataGridView2.Columns.Clear();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            usInter.CheckIfFieldsAreFilled(txtName, txtWidthDim, txtLength, txtH);
            usInter.ConvTextToDecimal(txtName, txtWidthDim, txtLength, txtH);
            dt.PushingDataInTable(txtName.Text, comboBox1.SelectedItem.ToString(), txtWidthDim.Text, txtLength.Text, txtWidthDim.Text, txtH.Text);
            
            usInter.SuperPuper(dataGridView, dt);
        }

        private void cutOut_Click(object sender, EventArgs e)
        {
            usInter.CheckIfFieldsAreFilled(dataGridView, txtWidthWP, txtLengthWP);
            usInter.ConvTxtToDecimal(txtWidthWP, txtLengthWP);
            dt.CutOut();

            usInter.SuperPuper(dataGridView, dt);
            usInter.SuperPuper2(dataGridView2, dt, usInter);
        }
        private static void AllowUserInputOnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                        (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }


        #region AllowUserInputOnlyNumbers
        private void txtWidthDim_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowUserInputOnlyNumbers(sender, e);
        }

        private void txtLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowUserInputOnlyNumbers(sender, e);
        }

        private void txtH_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowUserInputOnlyNumbers(sender, e);
        }

        private void txtLengthWP_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowUserInputOnlyNumbers(sender, e);
        }

        private void txtWidthWP_KeyPress(object sender, KeyPressEventArgs e)
        {
            AllowUserInputOnlyNumbers(sender, e);
        }
        #endregion

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            MyDtTable.itemToCutFrom = usInter.GetSelectedBalance(dataGridView);
            usInter.SuperPuper2(dataGridView2, dt, usInter);
            usInter.FillTxtBoxes(txtName, txtWidthDim, txtLength, txtH);
        }

        private void CancelDeletingButton_Click(object sender, EventArgs e)
        {
            dt.CancelCuttingWP();
            usInter.SuperPuper(dataGridView, dt);
            usInter.SuperPuper2(dataGridView2, dt, usInter);
        }

        private void DeleteResidualButton_Click(object sender, EventArgs e)
        {
            dt.DeleteResidualMaterial();
            usInter.SuperPuper(dataGridView, dt);
            usInter.SuperPuper2(dataGridView2, dt, usInter);
        }

        private void EditMaterialButton_Click(object sender, EventArgs e)
        {
            usInter.CheckIfFieldsAreFilled(txtName, txtWidthDim, txtLength, txtH);
            usInter.ConvTextToDecimal(txtName, txtWidthDim, txtLength, txtH);
            dt.EditResidual();

            usInter.SuperPuper(dataGridView, dt);
            usInter.SuperPuper2(dataGridView2, dt, usInter);
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            MyDtTable.itemToCutFrom = usInter.GetSelectedBalance(dataGridView);
        }

        private void GetComboValues()
        {
            SQLConnection.SQLObj.GetComboBxProductType();
            foreach (var item in SQLConnection.SQLObj.productTypeList)
            {
                comboBox1.Items.Add(item);
            }
        }
        public void FillForm1Residual(List<IMS_Object_Attributes> selectedObject)
        {
            var list = (from selObj in selectedObject
                           join attr in SQLConnection.SQLObj.attributePropList.DefaultIfEmpty()
                           on selObj.ATTRIBUTE_ID equals attr.ATTRIBUTE_ID
                           into temp
                           from l in temp
                           where l.NAME.Equals("Наименование") || l.NAME.Equals("Длина") || l.NAME.Equals("Ширина") || l.NAME.Equals("Диаметр")
                        select new { selObj.STRING_VALUE, l        .NAME}).ToList();

            txtName.Text = list.Where(x=>x.NAME.Equals("Наименование")).Select(x=>x.STRING_VALUE).FirstOrDefault();
            txtLength.Text = list.Where(x => x.NAME.Equals("Длина")).Select(x => x.STRING_VALUE).FirstOrDefault();
            if (!MyDtTable.residualType)
            {
                txtWidthDim.Text = list.Where(x => x.NAME.Equals("Диаметр")).Select(x => x.STRING_VALUE).FirstOrDefault();
            }
            else
            {
                txtWidthDim.Text = list.Where(x => x.NAME.Equals("Ширина")).Select(x => x.STRING_VALUE).FirstOrDefault();
            }






            // selectedObject.Select(x => x.).First();
        }

    }
}