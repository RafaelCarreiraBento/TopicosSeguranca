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
            this.lbUser = new System.Windows.Forms.Label();
            this.lbChat = new System.Windows.Forms.Label();
            this.tbmensagem = new System.Windows.Forms.TextBox();
            this.btEnviar = new System.Windows.Forms.Button();
            this.btLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbUser.Location = new System.Drawing.Point(12, 20);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(145, 25);
            this.lbUser.TabIndex = 10;
            this.lbUser.Text = "(NOME USER)";
            // 
            // lbChat
            // 
            this.lbChat.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbChat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbChat.Location = new System.Drawing.Point(17, 66);
            this.lbChat.Name = "lbChat";
            this.lbChat.Size = new System.Drawing.Size(724, 256);
            this.lbChat.TabIndex = 11;
            this.lbChat.Text = "label1";
            // 
            // tbmensagem
            // 
            this.tbmensagem.Location = new System.Drawing.Point(17, 334);
            this.tbmensagem.Multiline = true;
            this.tbmensagem.Name = "tbmensagem";
            this.tbmensagem.Size = new System.Drawing.Size(626, 28);
            this.tbmensagem.TabIndex = 13;
            // 
            // btEnviar
            // 
            this.btEnviar.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btEnviar.Location = new System.Drawing.Point(649, 334);
            this.btEnviar.Name = "btEnviar";
            this.btEnviar.Size = new System.Drawing.Size(92, 28);
            this.btEnviar.TabIndex = 14;
            this.btEnviar.Text = "Enviar";
            this.btEnviar.UseVisualStyleBackColor = false;
            this.btEnviar.Click += new System.EventHandler(this.btEnviar_Click);
            // 
            // btLogout
            // 
            this.btLogout.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btLogout.Location = new System.Drawing.Point(649, 20);
            this.btLogout.Name = "btLogout";
            this.btLogout.Size = new System.Drawing.Size(92, 28);
            this.btLogout.TabIndex = 15;
            this.btLogout.Text = "Logout";
            this.btLogout.UseVisualStyleBackColor = false;
            this.btLogout.Click += new System.EventHandler(this.btLogout_Click);
            // 
            // FormChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(771, 393);
            this.Controls.Add(this.btLogout);
            this.Controls.Add(this.btEnviar);
            this.Controls.Add(this.tbmensagem);
            this.Controls.Add(this.lbChat);
            this.Controls.Add(this.lbUser);
            this.Name = "FormChat";
            this.Text = "FormChat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Label lbChat;
        private System.Windows.Forms.TextBox tbmensagem;
        private System.Windows.Forms.Button btEnviar;
        private System.Windows.Forms.Button btLogout;
    }
}