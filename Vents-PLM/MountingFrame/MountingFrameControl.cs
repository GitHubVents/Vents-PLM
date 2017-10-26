using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDMWebService.Data.Solid.ElementsCase;
using SolidWorksLibrary.Builders.ElementsCase;

namespace Vents_PLM.MountingFrame
{
    public partial class MountingFrameControl : UserControl
    {
        public MountingFrameControl()
        {
            InitializeComponent();
        }
        MountingFrameBuilder mountingFrContr = new MountingFrameBuilder();
        private void btnFrameBuild_Click(object sender, EventArgs e)
        {
            string name = "name";
            mountingFrContr.SetValues(name, "Тип рамки" , textBoxDisplacement.Text, textBoxWidth.Text, textBoxLenth.Text, "Тип материала", "Толщина" );

            txtBoxForTesting.Text = mountingFrContr.Build();

            SpigotBuilder spigot = new SpigotBuilder();
            MessageBox.Show(spigot.RootFolder.ToString());
            
            spigot.Build(ServiceTypes.Constants.SpigotType_e.Twenty_mm, new Vector2(0, 1));
        }
    }
}
