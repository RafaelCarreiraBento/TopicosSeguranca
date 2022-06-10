using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuPrincipal
{
    public partial class FormMenu : Form
    {
        public static Aes aes;
        public FormMenu()
        {
            InitializeComponent();
            //Dá generate ao IV e à Key que serão usados para encriptar e desencriptar
            aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();
        }

        private void btEntrar_Click(object sender, EventArgs e)
        {
            //Cria uma instância do Form de Login e abre-o
            FormLogin2 login = new FormLogin2();
            login.Show();
        }
    }
}
