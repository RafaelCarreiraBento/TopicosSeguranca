namespace MenuPrincipal
{
    partial class FormChat2
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
            this.btLogout = new System.Windows.Forms.Button();
            this.btEnviar = new System.Windows.Forms.Button();
            this.tbmensagem = new System.Windows.Forms.TextBox();
            this.lbChat = new System.Windows.Forms.Label();
            this.lbUser = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btLogout
            // 
            this.btLogout.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btLogout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btLogout.Location = new System.Drawing.Point(673, 54);
            this.btLogout.Name = "btLogout";
            this.btLogout.Size = new System.Drawing.Size(92, 33);
            this.btLogout.TabIndex = 20;
            this.btLogout.Text = "Logout";
            this.btLogout.UseVisualStyleBackColor = false;
            this.btLogout.Click += new System.EventHandler(this.btLogout_Click);
            // 
            // btEnviar
            // 
            this.btEnviar.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btEnviar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btEnviar.Location = new System.Drawing.Point(673, 368);
            this.btEnviar.Name = "btEnviar";
            this.btEnviar.Size = new System.Drawing.Size(92, 28);
            this.btEnviar.TabIndex = 19;
            this.btEnviar.Text = "Enviar";
            this.btEnviar.UseVisualStyleBackColor = false;
            this.btEnviar.Click += new System.EventHandler(this.btEnviar_Click);
            // 
            // tbmensagem
            // 
            this.tbmensagem.Location = new System.Drawing.Point(41, 368);
            this.tbmensagem.Multiline = true;
            this.tbmensagem.Name = "tbmensagem";
            this.tbmensagem.Size = new System.Drawing.Size(626, 28);
            this.tbmensagem.TabIndex = 18;
            // 
            // lbChat
            // 
            this.lbChat.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lbChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbChat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbChat.Location = new System.Drawing.Point(41, 100);
            this.lbChat.Name = "lbChat";
            this.lbChat.Size = new System.Drawing.Size(724, 256);
            this.lbChat.TabIndex = 17;
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbUser.Location = new System.Drawing.Point(36, 54);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(145, 25);
            this.lbUser.TabIndex = 16;
            this.lbUser.Text = "(NOME USER)";
            // 
            // FormChat2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btLogout);
            this.Controls.Add(this.btEnviar);
            this.Controls.Add(this.tbmensagem);
            this.Controls.Add(this.lbChat);
            this.Controls.Add(this.lbUser);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormChat2";
            this.Text = "FormChat2";
            this.Load += new System.EventHandler(this.FormChat2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLogout;
        private System.Windows.Forms.Button btEnviar;
        private System.Windows.Forms.TextBox tbmensagem;
        private System.Windows.Forms.Label lbChat;
        private System.Windows.Forms.Label lbUser;
    }
}