using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResidualMaterials;

namespace Vents_PLM
{
    public partial class Form1 : UserControl
    {
        public Form1()
        {
            InitializeComponent();
            dt = new MyDtTable();
            usInter = new UserInterface();

            //////////////////////////////////////////////////
            //comboBox1.SelectedIndex = 0;
            dt.Load_Data(MyDtTable.residualType);

            usInter.SuperPuper(dataGridView, dt);
        }
        MyDtTable dt;
        UserInterface usInter;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            usInter.ClearTxtBoxes(txtName, txtWidthDim, txtLength, txtH, txtLengthWP, txtWidthWP);
            usInter.CheckType(comboBox1);
            usInter.ManagingUserInterface(lblWidthWP, lblH, lblWidthDim, txtWidthWP, txtH);

            usInter.SuperPuper(dataGridView, dt);
            dataGridView2.Columns.Clear();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            usInter.CheckIfFieldsAreFilled(txtName, txtWidthDim, txtLength, txtH);
            usInter.ConvTextToDecimal(txtName, txtWidthDim, txtLength, txtH);
            dt.PushingDataInTable();

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

    }
}