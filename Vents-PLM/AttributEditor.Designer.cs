namespace Vents_PLM
{
    partial class AttributEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveNewAttr_Btn = new System.Windows.Forms.Button();
            this.cancelSavingNewAttr_Btn = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // saveNewAttr_Btn
            // 
            this.saveNewAttr_Btn.Location = new System.Drawing.Point(844, 701);
            this.saveNewAttr_Btn.Name = "saveNewAttr_Btn";
            this.saveNewAttr_Btn.Size = new System.Drawing.Size(100, 33);
            this.saveNewAttr_Btn.TabIndex = 2;
            this.saveNewAttr_Btn.Text = "Применить";
            this.saveNewAttr_Btn.UseVisualStyleBackColor = true;
            this.saveNewAttr_Btn.Click += new System.EventHandler(this.saveNewAttr_Btn_Click);
            // 
            // cancelSavingNewAttr_Btn
            // 
            this.cancelSavingNewAttr_Btn.Location = new System.Drawing.Point(973, 701);
            this.cancelSavingNewAttr_Btn.Name = "cancelSavingNewAttr_Btn";
            this.cancelSavingNewAttr_Btn.Size = new System.Drawing.Size(100, 33);
            this.cancelSavingNewAttr_Btn.TabIndex = 3;
            this.cancelSavingNewAttr_Btn.Text = "Отмена";
            this.cancelSavingNewAttr_Btn.UseVisualStyleBackColor = true;
            this.cancelSavingNewAttr_Btn.Click += new System.EventHandler(this.cancelSavingNewAttr_Btn_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 27);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(244, 667);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(262, 27);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(886, 668);
            this.propertyGrid1.TabIndex = 1;
            // 
            // AttributEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 758);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.cancelSavingNewAttr_Btn);
            this.Controls.Add(this.saveNewAttr_Btn);
            this.Name = "AttributEditor";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button saveNewAttr_Btn;
        private System.Windows.Forms.Button cancelSavingNewAttr_Btn;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}

