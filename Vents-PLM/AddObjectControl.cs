using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Vents_PLM
{
    public partial class AddObjectControl : UserControl
    {
        public AddObjectControl()
        {
            InitializeComponent();
            myNewObject = new IMS_Object();
            //propGridForObjects.SelectedObject = myNewObject;
            UserInterface2.AttributesListComboBX(attributeForObjectComboBX);
        }
        string attributeName;

        IMS_Object myNewObject;
        private void NewObjectAddBtn_Click(object sender, EventArgs e)
        {
            SQLConnection.SQLObj.SaveNewObject(myNewObject);// into bd IMS_OBJECTS
            SQLConnection.SQLObj.SaveAttributeForObject(myNewObject, labelName.Text, txtBoxName.Text);

            this.Hide();
            UserInterface2 ui = new UserInterface2();
            
            //make tree view
        }

        private void attributeForObjectComboBX_SelectedIndexChanged(object sender, EventArgs e)
        {
            attributeName = attributeForObjectComboBX.SelectedItem.ToString();
        }
    }
}