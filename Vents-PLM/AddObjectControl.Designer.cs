namespace Vents_PLM
{
    partial class AddObjectControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NewObjectAddBtn = new System.Windows.Forms.Button();
            this.txtBox1 = new System.Windows.Forms.TextBox();
            this.txtBoxName = new System.Windows.Forms.TextBox();
            this.attributeForObjectComboBX = new System.Windows.Forms.ComboBox();
            this.labelName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NewObjectAddBtn
            // 
            this.NewObjectAddBtn.Location = new System.Drawing.Point(421, 124);
            this.NewObjectAddBtn.Name = "NewObjectAddBtn";
            this.NewObjectAddBtn.Size = new System.Drawing.Size(98, 40);
            this.NewObjectAddBtn.TabIndex = 0;
            this.NewObjectAddBtn.Text = "Добавить объект";
            this.NewObjectAddBtn.UseVisualStyleBackColor = true;
            this.NewObjectAddBtn.Click += new System.EventHandler(this.NewObjectAddBtn_Click);
            // 
            // txtBox1
            // 
            this.txtBox1.Location = new System.Drawing.Point(12, 17);
            this.txtBox1.Multiline = true;
            this.txtBox1.Name = "txtBox1";
            this.txtBox1.Size = new System.Drawing.Size(185, 19);
            this.txtBox1.TabIndex = 1;
            this.txtBox1.Visible = false;
            // 
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(149, 135);
            this.txtBoxName.Multiline = true;
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(185, 19);
            this.txtBoxName.TabIndex = 2;
            // 
            // attributeForObjectComboBX
            // 
            this.attributeForObjectComboBX.FormattingEnabled = true;
            this.attributeForObjectComboBX.Location = new System.Drawing.Point(213, 16);
            this.attributeForObjectComboBX.Name = "attributeForObjectComboBX";
            this.attributeForObjectComboBX.Size = new System.Drawing.Size(144, 21);
            this.attributeForObjectComboBX.TabIndex = 4;
            this.attributeForObjectComboBX.Visible = false;
            this.attributeForObjectComboBX.SelectedIndexChanged += new System.EventHandler(this.attributeForObjectComboBX_SelectedIndexChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(23, 135);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(83, 13);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "Наименование";
            // 
            // AddObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.attributeForObjectComboBX);
            this.Controls.Add(this.txtBoxName);
            this.Controls.Add(this.txtBox1);
            this.Controls.Add(this.NewObjectAddBtn);
            this.Name = "AddObjectControl";
            this.Size = new System.Drawing.Size(571, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NewObjectAddBtn;
        private System.Windows.Forms.TextBox txtBox1;
        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.ComboBox attributeForObjectComboBX;
        private System.Windows.Forms.Label labelName;
    }
}
