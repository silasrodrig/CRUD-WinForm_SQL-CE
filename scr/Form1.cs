using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
// SQL Server CE Necessário importar. 
using System.Data.SqlServerCe;
using System.Data.SqlClient;
//SQLite
using System.Data.SQLite;
// MySQL
using MySql.Data.MySqlClient;

namespace BaseDados
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            #region SQL SERVER CE
            //Criando string com o caminho da base de dados.
            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            //Definição da StringConection que conecta na base.
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            //Objeto do tipo SqlCeEngine usando a string strConection
            SqlCeEngine db = new SqlCeEngine(strConection);

            //Verificação se a base de dados exite
            if (!File.Exists(baseDados))
            {
                db.CreateDatabase();
            }

            db.Dispose();

            // Criando a conexão passando a string conection no construtor.
            SqlCeConnection conexao = new SqlCeConnection(strConection);
            //conexao.ConnectionString = strConection;  (outra maneira de conectar com a base)

            try
            {
                conexao.Open();
                labelResultado.Text = "Conectado SQL Server CE";
            }
            catch (Exception ex)
            {

                labelResultado.Text = "ERRO!!! Inpossível Conectar ao SQL Server CE \n " + ex;
            }
            finally
            {
                conexao.Close();
            }
            #endregion

            #region SQLite
            ////Criando string com o caminho da base de dados.
            //string baseDados = Application.StartupPath + @"\db\DBSQLite.db";
            ////Definição da StringConection que conecta na base.
            //string strConection = @"Data Source = " + baseDados + "; Version = '3'";


            ////Verificação se a base de dados exite
            //if (!File.Exists(baseDados))
            //{
            //    SQLiteConnection.CreateFile(baseDados);
            //}

            //// Criando a conexão passando a string conection no construtor.
            //SQLiteConnection conexao = new SQLiteConnection(strConection);

            ////conexao.ConnectionString = strConection;  (outra maneira de conectar com a base)

            //try
            //{
            //    conexao.Open();
            //    labelResultado.Text = "Conectado Base SQLite";
            //}
            //catch (Exception ex)
            //{

            //    labelResultado.Text = "ERRO!!! Inpossível Conectar ao SQLite\n " + ex;
            //}
            //finally
            //{
            //    conexao.Close();
            //}

            //#endregion

            //#region MySQL



            #endregion
        }

        private void btnCriarTabela_Click(object sender, EventArgs e)
        {
            #region SQLServerCE

            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                conexao.Open();
                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                comando.CommandText = "CREATE TABLE pessoas ( id INT IDENTITY(1,1) PRIMARY KEY, nome NVARCHAR(50), email NVARCHAR(50))";
                comando.ExecuteNonQuery();

                labelResultado.Text = "Tabela Criada Sql Server CE";
                comando.Dispose();
            }
            catch (Exception ex)
            {

                labelResultado.Text = ex.Message;
            }
            finally
            {
                conexao.Close();

            }

            #endregion
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            #region SQLServerCE

            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                string nome = txtNome.Text;
                string email = txtEmail.Text;

                if (nome != "")
                {
                    conexao.Open();

                    SqlCeCommand comando = new SqlCeCommand();
                    comando.Connection = conexao;


                    //comando.CommandText = "INSERT INTO pessoas VALUES ('" + nome + "', '" + email + "')";
                    comando.CommandText = "INSERT INTO pessoas (nome, email) Values (@nome, @email)";

                    comando.Parameters.AddWithValue("nome", nome);
                    comando.Parameters.AddWithValue("email", email);


                    comando.ExecuteNonQuery();

                    labelResultado.Text = "Registro Inserido Sql Server CE";
                    txtNome.Text = "";
                    txtEmail.Text = "";

                    comando.Dispose();
                    #region Comando do botão procurar que já insere e busca o dado inserido.
                    ////Comando de pesquisa no banco que atualiza a lista mesmo comando procurar

                    //lista.Rows.Clear();
                    //txtNome.Text = "";
                    //txtEmail.Text = "";


                    //string query = "SELECT * FROM pessoas";

                    //DataTable dados = new DataTable(); //tabela que recebe os dados do banco

                    //SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConection);
                    ////Essa adaptador recebe o dados do banco


                    //adaptador.Fill(dados); //o adaptador pegou os dados recebidos do banco e salvou no dados

                    //foreach (DataRow linhas in dados.Rows)
                    //{
                    //    lista.Rows.Add(linhas.ItemArray);
                    //}


                    //comando.Dispose();
                    #endregion


                }
                else
                {
                    labelResultado.Text = "Informe um Nome!!!!";
                }
                

            }
            catch (Exception ex)
            {

                labelResultado.Text = ex.Message;
            }
            finally
            {
                conexao.Close();

            }

            #endregion
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {

            labelResultado.Text = "";
            lista.Rows.Clear();

            #region SQLServerCE

            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            try
            {
                #region String de recuperação de dados.
                string query = "SELECT * FROM pessoas";

                if (txtNome.Text != "") // se for diferente de vazio é pra procar o 
                                       //que foi digitado no campo nome
                {
                    query = "SELECT * FROM pessoas WHERE nome LIKE '" + txtNome.Text + "'";
                    //Query pra ir no banco e buscar o nome que foi digitado como esta no IF
                    // se digitar no campo ele busca o nome se não ele vai buscar todos os dados. 
                }
                #endregion

                DataTable dados = new DataTable(); //tabela que recebe os dados do banco

                SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConection);
                //Essa adaptador recebe o dados do banco


                conexao.Open();

                adaptador.Fill(dados); //o adaptador pegou os dados recebidos do banco e salvou no dados

                foreach (DataRow linhas in dados.Rows)
                {
                    lista.Rows.Add(linhas.ItemArray);
                }
                
            }
            catch (Exception ex)
            {
                lista.Rows.Clear();
                labelResultado.Text = ex.Message;
            }
            finally
            {
                conexao.Close();

            }

            #endregion

        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            #region SQLServerCE

            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);

            

            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                int id = (int)lista.SelectedRows[0].Cells[0].Value;
               
                //comando.CommandText = "INSERT INTO pessoas VALUES ('" + nome + "', '" + email + "')";
                comando.CommandText = "DELETE FROM pessoas WHERE id = '" + id + "'";

                comando.ExecuteNonQuery();
                
                labelResultado.Text = "Registro Deletado Sql Server CE";

                comando.Dispose();

                #region Comando do botão procurar, que já busca no banco o dado excluido
                //lista.Rows.Clear();
                //string query = "SELECT * FROM pessoas";

                //DataTable dados = new DataTable(); //tabela que recebe os dados do banco

                //SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConection);
                ////Essa adaptador recebe o dados do banco


                //adaptador.Fill(dados); //o adaptador pegou os dados recebidos do banco e salvou no dados

                //foreach (DataRow linhas in dados.Rows)
                //{
                //    lista.Rows.Add(linhas.ItemArray);
                //}

                //comando.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
             
                labelResultado.Text = ex.Message;
            }
            finally
            {
               
                conexao.Close();

            }

            #endregion

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            #region SQLServerCE

            string baseDados = Application.StartupPath + @"\db\DBSQLServer.sdf";
            string strConection = @"DataSource = " + baseDados + "; Password = '1234'";

            SqlCeConnection conexao = new SqlCeConnection(strConection);



            try
            {
                conexao.Open();

                SqlCeCommand comando = new SqlCeCommand();
                comando.Connection = conexao;

                int id = (int)lista.SelectedRows[0].Cells[0].Value;
                string nome = txtNome.Text;
                string email = txtEmail.Text;

                string consulta = "UPDATE pessoas SET nome = '" + nome + "', email = '" + email + "' WHERE id LIKE '" + id + "'";

                comando.CommandText = consulta;

                comando.ExecuteNonQuery();

                labelResultado.Text = "Registro Alterado Sql Server CE";

                //btnProcurar_Click(sender, e);
                comando.Dispose();

                #region Comando do botão procurar, que já busca no banco o dado excluido
                //lista.Rows.Clear();
                //string query = "SELECT * FROM pessoas";

                //DataTable dados = new DataTable(); //tabela que recebe os dados do banco

                //SqlCeDataAdapter adaptador = new SqlCeDataAdapter(query, strConection);
                ////Essa adaptador recebe o dados do banco


                //adaptador.Fill(dados); //o adaptador pegou os dados recebidos do banco e salvou no dados

                //foreach (DataRow linhas in dados.Rows)
                //{
                //    lista.Rows.Add(linhas.ItemArray);
                //}

                //comando.Dispose();
                #endregion
            }
            catch (Exception ex)
            {

                labelResultado.Text = ex.Message;
            }
            finally
            {

                conexao.Close();

            }

            #endregion
        }
    }
}
