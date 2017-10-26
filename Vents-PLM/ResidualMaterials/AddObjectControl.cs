using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ResidualMaterials;

namespace Vents_PLM
{
    public partial class AddObjectControl : UserControl
    {
        public AddObjectControl()
        {
            InitializeComponent();
            myNewObject = new IMS_Object();
        }
        IMS_Object myNewObject;
        private void NewObjectAddBtn_Click(object sender, EventArgs e)
        {
            if (MainForm.EditOrNotEdit)
            {
                foreach (var item in MainForm.selectedObj)
                {
                    SQLConnection.SQLObj.SaveAttributeForObject(item, labelName.Text, txtBoxName.Text, MainForm.EditOrNotEdit);
                }
            }
            else
            {
                SQLConnection.SQLObj.SaveNewObject(myNewObject);       // into bd IMS_OBJECTS
               
                SQLConnection.SQLObj.SaveAttributeForObject(null, labelName.Text, txtBoxName.Text, MainForm.EditOrNotEdit);
               
            }


            MainForm.EditOrNotEdit = false;
            this.Hide();
        }

        public void AddObjectControlFill(List<IMS_Object_Attributes> selectedObject)
        {
            txtBoxName.Text = selectedObject.Select(x => x.STRING_VALUE).First();
        }
    }
}