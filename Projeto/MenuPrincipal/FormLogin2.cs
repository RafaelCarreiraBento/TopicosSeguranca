using EI.SI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuPrincipal
{
    public partial class FormLogin2 : Form
    {
        public static string nomeDeUtilizador;  // Variável que vai armazenar o username para os outros forms
        private const int SALTSIZE = 8;
        private const int NUMBER_OF_ITERATIONS = 5000;
      
        
        public FormLogin2()
        {
            InitializeComponent();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            //Obtém o texto das TextBoxs
            String pass = textBoxPassword.Text;
            String username = textBoxUser.Text;
            //Vai verificar se os dados de Login estão corretos
            if (VerifyLogin(username, pass))
            {
                MessageBox.Show("Utilizador válido");
                nomeDeUtilizador = username;
                //Vai abir o form de chat
                FormChat2 chat = new FormChat2();
                this.Hide();
                chat.Show();
            }
            else MessageBox.Show("Utilizador não válido");
        }

        /// <summary>
        /// Verifica se os dados estão base de dados
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>True se houver utilizador e password; False se não encontrar</returns>
        private bool VerifyLogin(string username, string password)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\rafae\OneDrive\Ambiente de Trabalho\TESP\2º semestre\Tópicos de segurança\Projeto\Projeto\MenuPrincipal\baseDadosProjeto.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração do comando SQL
                String sql = "SELECT * FROM Users WHERE Username = @username";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;

                // Declaração dos parâmetros do comando SQL
                SqlParameter param = new SqlParameter("@username", username);

                // Introduzir valor ao parâmentro registado no comando SQL
                cmd.Parameters.Add(param);

                // Associar ligação à Base de Dados ao comando a ser executado
                cmd.Connection = conn;

                // Executar comando SQL
                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Erro ao tentar aceder ao user.");
                }

                // Ler resultado da pesquisa
                reader.Read();

                // Obter Hash (password + salt)
                byte[] saltedPasswordHashStored = (byte[])reader["SaltedPasswordHash"];

                // Obter salt
                byte[] saltStored = (byte[])reader["Salt"];

                conn.Close();

                byte[] hash = GenerateSaltedHash(password, saltStored);
                return saltedPasswordHashStored.SequenceEqual(hash);

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocorreu um erro: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Efetua o registo na base de dados
        /// </summary>
        /// <param name="username"></param>
        /// <param name="saltedPasswordHash"></param>
        /// <param name="salt"></param>
        /// <exception cref="Exception"></exception>
        private void Register(string username, byte[] saltedPasswordHash, byte[] salt)
        {
            SqlConnection conn = null;
            try
            {
                // Configurar ligação à Base de Dados
                conn = new SqlConnection();
                conn.ConnectionString = String.Format(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\rafae\OneDrive\Ambiente de Trabalho\TESP\2º semestre\Tópicos de segurança\Projeto\Projeto\MenuPrincipal\baseDadosProjeto.mdf';Integrated Security=True");

                // Abrir ligação à Base de Dados
                conn.Open();

                // Declaração dos parâmetros do comando SQL
                SqlParameter paramUsername = new SqlParameter("@username", username);
                SqlParameter paramPassHash = new SqlParameter("@saltedPasswordHash", saltedPasswordHash);
                SqlParameter paramSalt = new SqlParameter("@salt", salt);

                // Declaração do comando SQL
                String sql = "INSERT INTO Users (Username, SaltedPasswordHash, Salt) VALUES (@username,@saltedPasswordHash,@salt)";

                // Prepara comando SQL para ser executado na Base de Dados
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Introduzir valores aos parâmentros registados no comando SQL
                cmd.Parameters.Add(paramUsername);
                cmd.Parameters.Add(paramPassHash);
                cmd.Parameters.Add(paramSalt);

                // Executar comando SQL
                int lines = cmd.ExecuteNonQuery();

                // Fechar ligação
                conn.Close();
                if (lines == 0)
                {
                    // Se forem devolvidas 0 linhas alteradas então o não foi executado com sucesso
                    throw new Exception("Erro ao inserir o utilizador");
                }
                MessageBox.Show("Registado com sucesso");
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao registar: " + e.Message);
                throw new Exception("Erro ao inserir o utilizador:" + e.Message);
            }
        }

        /// <summary>
        /// Dá Generetate a um número criptográfico aleatório
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private static byte[] GenerateSalt(int size)
        {
            //Dá generate a um número cryptográfico aleatório.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
        }

        /// <summary>
        /// Dá generate a um SaltedHash
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static byte[] GenerateSaltedHash(string plainText, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(plainText, salt, NUMBER_OF_ITERATIONS);
            return rfc2898.GetBytes(32);
        }

        private void btRegisto_Click(object sender, EventArgs e)
        {
            //Obtém dados das textBoxs
            String pass = textBoxPassword.Text;
            String username = textBoxUser.Text;
            //Se as textBoxs não estiverem vazias
            if (pass != "" && username != "")
            {
                //Criar o Salt, o SaltedHash e efetuar o registo
                byte[] salt = GenerateSalt(SALTSIZE);
                byte[] hash = GenerateSaltedHash(pass, salt);
                Register(username, hash, salt);
            }
        }


    }
}
