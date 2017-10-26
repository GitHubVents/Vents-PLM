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
            this.txtBoxName = new System.Windows.Forms.TextBox();
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
            // txtBoxName
            // 
            this.txtBoxName.Location = new System.Drawing.Point(156, 135);
            this.txtBoxName.Multiline = true;
            this.txtBoxName.Name = "txtBoxName";
            this.txtBoxName.Size = new System.Drawing.Size(185, 19);
            this.txtBoxName.TabIndex = 2;
            this.txtBoxName.Tag = "Наименование";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(30, 135);
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
            this.Controls.Add(this.txtBoxName);
            this.Controls.Add(this.NewObjectAddBtn);
            this.Name = "AddObjectControl";
            this.Size = new System.Drawing.Size(571, 290);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NewObjectAddBtn;
        private System.Windows.Forms.TextBox txtBoxName;
        private System.Windows.Forms.Label labelName;
    }
}
