using Dapper;
using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstruturaBoostratap.ModelViews
{
    public class ClienteModelView : OptionServices
    {
        #region *** COMPONENTES ***
        public int ClienteID { get; set; }
        public string NomeCliente { get; set; }
        public string EmailCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public string DataCadastro { get; set; } = DateTime.Now.ToString("dd'/'MM'/'yyyy");
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string RgIe { get; set; }
        public bool Isento { get; set; }
        public string Genero { get; set; }
        public string DataNascimento { get; set; }
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
        public bool Status { get; set; }
        public string AtivoBloque { get; set; }
        public string MensagemErro { get; set; }
        public string MensagemOK { get; set; }
        public string NomeClienteFiltro { get; set; }
        public string EmailClienteFiltro { get; set; }
        public string TelefoneClienteFiltro { get; set; }
        public string DataCadastroFiltro { get; set; }
        public string StatusFiltro { get; set; }
        public List<ClienteModelView> ListaClientes { get; set; }
        public List<SelectListItem> ListaGenero { get; set; }
        public List<SelectListItem> ListaBloqueio { get; set; }
        #endregion

        #region *** METODOS ***
        public List<ClienteModelView> ListarClientes()
        {
            ListaClientes = new List<ClienteModelView>();

            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_cliente AS ClienteID, ");
                sql.AppendLine("nome_cliente AS NomeCliente, ");
                sql.AppendLine("email_cliente AS EmailCliente, ");
                sql.AppendLine("telefone_cliente AS TelefoneCliente, ");
                sql.AppendLine("data_cadastro AS DataCadastro, ");
                sql.AppendLine("status AS Status ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendLine("1 = 1 ");

                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(NomeClienteFiltro))
                    sql.AppendFormat("AND upper(nome_cliente) LIKE upper('%{0}%') ", NomeClienteFiltro);
                if (!String.IsNullOrEmpty(EmailClienteFiltro))
                    sql.AppendFormat("AND upper(email_cliente) LIKE upper('%{0}%') ", EmailClienteFiltro);
                if (!String.IsNullOrEmpty(TelefoneClienteFiltro))
                    sql.AppendFormat("AND upper(telefone_cliente) LIKE upper('%{0}%') ", TelefoneClienteFiltro);
                if (!String.IsNullOrEmpty(DataCadastroFiltro))
                    sql.AppendFormat("AND data_cadastro = '{0}' ", FormatarData("bd", DataCadastroFiltro));
                if (!String.IsNullOrEmpty(StatusFiltro))
                    sql.AppendFormat("AND status = {0} ", StatusFiltro);
                /*Fim dos filtros*/

                sql.AppendLine("ORDER BY id_cliente ");
                sql.AppendFormat("LIMIT {0} , {1} ", (PageAtual - 1) * RPP, RPP);

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).ToList();

                TotalRegistro();

                foreach (var item in Dados)
                {
                    ClienteModelView oModelo = new ClienteModelView
                    {
                        ClienteID = item.ClienteID,
                        NomeCliente = item.NomeCliente,
                        EmailCliente = item.EmailCliente,
                        TelefoneCliente = item.TelefoneCliente,
                        DataCadastro = FormatarData("user3", item.DataCadastro),
                        AtivoBloque = (item.Status == true ? "Ativo" : "Bloqueado")
                    };

                    ListaClientes.Add(oModelo);
                }

                return ListaClientes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void TotalRegistro()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(id_cliente) AS QTDRegistros ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendLine("1 = 1 ");

                /*Filtros de pesquisa*/
                if (!String.IsNullOrEmpty(NomeClienteFiltro))
                    sql.AppendFormat("AND upper(nome_cliente) LIKE upper('%{0}%') ", NomeClienteFiltro);
                if (!String.IsNullOrEmpty(EmailClienteFiltro))
                    sql.AppendFormat("AND upper(email_cliente) LIKE upper('%{0}%') ", EmailClienteFiltro);
                if (!String.IsNullOrEmpty(TelefoneClienteFiltro))
                    sql.AppendFormat("AND upper(telefone_cliente) LIKE upper('%{0}%') ", TelefoneClienteFiltro);
                if (!String.IsNullOrEmpty(DataCadastroFiltro))
                    sql.AppendFormat("AND data_cadastro = '{0}' ", FormatarData("bd", DataCadastroFiltro));
                if (!String.IsNullOrEmpty(StatusFiltro))
                    sql.AppendFormat("AND status = {0} ", StatusFiltro);
                /*Fim dos filtros*/

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).SingleOrDefault();

                QTDRegistros = Dados.QTDRegistros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void CarregaInformacao(int id)
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("id_cliente AS ClienteID, ");
                sql.AppendLine("nome_cliente AS NomeCliente, ");
                sql.AppendLine("email_cliente AS EmailCliente, ");
                sql.AppendLine("telefone_cliente AS TelefoneCliente,  ");
                sql.AppendLine("data_cadastro AS DataCadastro,  ");
                sql.AppendLine("tipo_pessoa AS TipoPessoa,  ");
                sql.AppendLine("cpf_cnpj AS CpfCnpj,  ");
                sql.AppendLine("rg_ie AS RgIe,  ");
                sql.AppendLine("isento AS Isento,  ");
                sql.AppendLine("genero AS Genero,  ");
                sql.AppendLine("data_nascimento AS DataNascimento,  ");
                sql.AppendLine("status AS Status   ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("id_cliente = {0} ", id);

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).SingleOrDefault();

                ClienteID = Dados.ClienteID;
                NomeCliente = Dados.NomeCliente;
                EmailCliente = Dados.EmailCliente;
                TelefoneCliente = Dados.TelefoneCliente;
                DataCadastro = FormatarData("user3", Dados.DataCadastro);
                TipoPessoa = Dados.TipoPessoa;
                CpfCnpj = Dados.CpfCnpj;
                RgIe = Dados.RgIe;
                Isento = Dados.Isento;
                Genero = Dados.Genero;
                DataNascimento = FormatarData("user3", Dados.DataNascimento);
                Status = Dados.Status;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ListaBloqueios()
        {
            ListaBloqueio = new List<SelectListItem>();

            try
            {

                var dados = GetBloqueios();

                ListaBloqueio.Add(new SelectListItem() { Text = "", Value = "" });

                foreach (var item in dados)
                {
                    ListaBloqueio.Add(new SelectListItem() { Text = item.Texto.ToString(), Value = item.Valor.ToString() });
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void ListaGeneros()
        {
            ListaGenero = new List<SelectListItem>();

            try
            {
                var dados = GetListaGenero();

                ListaGenero.Add(new SelectListItem() { Text = "", Value = "" });

                foreach (var item in dados)
                {
                    ListaGenero.Add(new SelectListItem() { Text = item.NomeGenero.ToString(), Value = item.CodigoGenero.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool VerificarCPF()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(cpf_cnpj) AS QTDRegistros ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("cpf_cnpj = '{0}'", CpfCnpj);

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).SingleOrDefault();

                if (Dados.QTDRegistros > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public bool VerificaEmail()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(email_cliente) AS QTDRegistros ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("email_cliente = '{0}'", EmailCliente);

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).SingleOrDefault();

                if (Dados.QTDRegistros > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool VerificaInscricao()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("COUNT(rg_ie) AS QTDRegistros ");
                sql.AppendLine("FROM ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("WHERE ");
                sql.AppendFormat("rg_ie = '{0}'", RgIe);

                var Dados = conexao.Query<ClienteModelView>(sql.ToString()).SingleOrDefault();

                if (Dados.QTDRegistros > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void InserirCliente()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("INSERT INTO ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("(nome_cliente, email_cliente, telefone_cliente, data_cadastro, tipo_pessoa, cpf_cnpj, rg_ie, isento, genero, data_nascimento, senha, status) ");
                sql.AppendLine("VALUES ");
                sql.AppendLine("( ");
                sql.AppendFormat("'{0}',", NomeCliente);
                sql.AppendFormat("'{0}',", EmailCliente);
                sql.AppendFormat("'{0}',", TelefoneCliente);
                sql.AppendFormat("'{0}',", FormatarData("bd", DataCadastro));
                sql.AppendFormat("'{0}',", TipoPessoa);
                sql.AppendFormat("'{0}',", CpfCnpj);
                sql.AppendFormat("'{0}',", RgIe);
                sql.AppendFormat("{0},", Isento);
                sql.AppendFormat("'{0}',", Genero);
                sql.AppendFormat("'{0}',", FormatarData("bd", DataNascimento));
                sql.AppendFormat("'{0}',", Senha);
                sql.AppendFormat("{0}", Status);
                sql.AppendLine(")");

                MySqlCommand comando = new MySqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void AlterarCliente()
        {
            MySqlConnection conexao = new MySqlConnection(DBModel.strConn);

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("UPDATE ");
                sql.AppendLine("Clientes ");
                sql.AppendLine("SET ");
                sql.AppendFormat("nome_cliente = '{0}',", NomeCliente);
                sql.AppendFormat("email_cliente = '{0}',", EmailCliente);
                sql.AppendFormat("telefone_cliente = '{0}',", TelefoneCliente);
                sql.AppendFormat("data_cadastro = '{0}',", FormatarData("bd", DataCadastro));
                sql.AppendFormat("tipo_pessoa = '{0}',", TipoPessoa);
                sql.AppendFormat("cpf_cnpj = '{0}',", CpfCnpj);
                sql.AppendFormat("rg_ie = '{0}',", RgIe);
                sql.AppendFormat("isento = {0},", Isento);
                sql.AppendFormat("genero = '{0}',", Genero);
                sql.AppendFormat("data_nascimento = '{0}',", FormatarData("bd", DataNascimento));
                if(!String.IsNullOrEmpty(Senha))
                    sql.AppendFormat("senha = '{0}',", Senha);
                sql.AppendFormat("status = {0} ", Status);
                sql.AppendLine("WHERE ");
                sql.AppendFormat("id_cliente = {0}", ClienteID);

                MySqlCommand comando = new MySqlCommand(sql.ToString(), conexao);
                conexao.Open();
                comando.ExecuteReader();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion
    }
}
