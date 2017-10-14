using System;
using System.Windows.Forms;

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
                SQLConnection.SQLObj.SaveAttributeForObject(MainForm.selectedObj, labelName.Text, txtBoxName.Text, MainForm.EditOrNotEdit);
            }
            else
            {
                SQLConnection.SQLObj.SaveNewObject(myNewObject);       // into bd IMS_OBJECTS
                SQLConnection.SQLObj.SaveAttributeForObject(MainForm.selectedObj, labelName.Text, txtBoxName.Text, MainForm.EditOrNotEdit);
            }


            MainForm.EditOrNotEdit = false;
            this.Hide();
        }
        

        public void ControlEdit(string objectTypeName, IMS_Object_Attributes selectedObject)
        {
            txtBoxName.Text = selectedObject.STRING_VALUE;
        }
    }
}