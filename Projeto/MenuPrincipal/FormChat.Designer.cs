namespace MenuPrincipal
{
    partial class FormChat
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
            this.labelUser = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEscrever = new System.Windows.Forms.TextBox();
            this.buttonRegistar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUser.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelUser.Location = new System.Drawing.Point(12, 20);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(145, 25);
            this.labelUser.TabIndex = 10;
            this.labelUser.Text = "(NOME USER)";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(17, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(724, 256);
            this.label1.TabIndex = 11;
            this.label1.Text = "label1";
            // 
            // textBoxEscrever
            // 
            this.textBoxEscrever.Location = new System.Drawing.Point(17, 334);
            this.textBoxEscrever.Multiline = true;
            this.textBoxEscrever.Name = "textBoxEscrever";
            this.textBoxEscrever.Size = new System.Drawing.Size(626, 28);
            this.textBoxEscrever.TabIndex = 13;
            // 
            // buttonRegistar
            // 
            this.buttonRegistar.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.buttonRegistar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonRegistar.Location = new System.Drawing.Point(649, 334);
            this.buttonRegistar.Name = "buttonRegistar";
            this.buttonRegistar.Size = new System.Drawing.Size(92, 28);
            this.buttonRegistar.TabIndex = 14;
            this.buttonRegistar.Text = "Enviar";
            this.buttonRegistar.UseVisualStyleBackColor = false;
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(771, 393);
            this.Controls.Add(this.buttonRegistar);
            this.Controls.Add(this.textBoxEscrever);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelUser);
            this.Name = "FormChat";
            this.Text = "FormChat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEscrever;
        private System.Windows.Forms.Button buttonRegistar;
    }
}