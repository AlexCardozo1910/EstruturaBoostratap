using EstruturaBoostratap.Data.Commun;
using EstruturaBoostratap.Data.Models;
using EstruturaBoostratap.ModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstruturaBoostratap.Controllers
{
    public class ClienteController : BasicController
    {
        public ActionResult Index(int page)
        {
            ClienteModelView dados = new ClienteModelView
            {
                PageAtual = page == 0 ? 1 : page
            };
            dados.ListaBloqueios();
            dados.ListaClientes = dados.ListarClientes();

            return View(dados);
        }

        // POST: Buscar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IFormCollection collection, int page)
        {
            try
            {
                if (collection["LimparBusca"] == "LimparFiltro")
                {
                    ClienteModelView reset = new ClienteModelView
                    {
                        PageAtual = page == 0 ? 1 : page
                    };
                    reset.ListaClientes = reset.ListarClientes();

                    reset.ListaBloqueios();
                    return View(reset);
                }

                ClienteModelView dados = new ClienteModelView
                {
                    PageAtual = page == 0 ? 1 : page,
                    NomeClienteFiltro = collection["NomeClienteFiltro"],
                    EmailClienteFiltro = collection["EmailClienteFiltro"],
                    TelefoneClienteFiltro = collection["TelefoneClienteFiltro"],
                    DataCadastroFiltro = collection["DataCadastroFiltro"],
                    StatusFiltro = collection["StatusFiltro"]
                };

                dados.ListaClientes = dados.ListarClientes();

                dados.ListaBloqueios();
                dados.NomeClienteFiltro = collection["NomeClienteFiltro"];
                dados.EmailClienteFiltro = collection["EmailClienteFiltro"];
                dados.TelefoneClienteFiltro = collection["TelefoneClienteFiltro"];
                dados.DataCadastroFiltro = collection["DataCadastroFiltro"];
                dados.StatusFiltro = collection["StatusFiltro"];

                return View(dados);
            }
            catch (Exception ex)
            {
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return RedirectToAction("Index", "Cliente");
            }
        }

        public ActionResult Create()
        {
            ClienteModelView dados = new ClienteModelView();
            dados.ListaGeneros();

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                ClienteModelView dados = new ClienteModelView();
                dados.ListaGeneros();

                dados.NomeCliente = collection["NomeCliente"];
                dados.EmailCliente = collection["EmailCliente"];
                dados.TelefoneCliente = collection["Telefone"];
                dados.TipoPessoa = collection["TipoPessoa"];
                dados.CpfCnpj = collection["CpfCnpj"];
                dados.RgIe = collection["RgIe"];
                dados.Isento = Convert.ToBoolean(collection["Isento"]);
                dados.Genero = collection["Genero"];
                dados.DataNascimento = collection["DataNascimento"];
                dados.Senha = collection["Senha"];
                dados.ConfirmarSenha = collection["ConfirmarSenha"];
                dados.Status = Convert.ToBoolean(collection["Status"]);

                if ((dados.Senha.Length < 8 || dados.Senha.Length > 15) && (dados.ConfirmarSenha.Length < 8 || dados.ConfirmarSenha.Length > 15))
                {
                    dados.MensagemErro = "A senha tem que conter no mínimo 8 (oito) caracteres e no máximo 15 (quinze) caracteres!";
                    return View(dados);
                }

                if (dados.Senha != dados.ConfirmarSenha)
                {
                    dados.MensagemErro = "As senhas informadas não conferem!";
                    return View(dados);
                }

                if (dados.VerificarCPF() == true)
                {
                    dados.MensagemErro = "Este CPF/ CNPJ já está cadastrado para outro Cliente.";
                    return View(dados);
                }

                if (dados.VerificaEmail() == true)
                {
                    dados.MensagemErro = "Este e-mail já está cadastrado para outro Cliente.";
                    return View(dados);
                }

                if (dados.TipoPessoa == "F" && dados.Isento == false)
                {
                    if (dados.VerificaInscricao() == true)
                    {
                        dados.MensagemErro = "Esta Inscrição Estadual já está cadastrada para outro Cliente.";
                    }
                }

                dados.Senha = HashValue(collection["Senha"]);

                dados.InserirCliente();

                return RedirectToAction("Index", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return RedirectToAction("Create", "Cliente");
            }
        }

        public ActionResult Edit(int id)
        {
            ClienteModelView dados = new ClienteModelView();
            dados.CarregaInformacao(id);
            dados.ListaGeneros();

            return View(dados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                ClienteModelView dados = new ClienteModelView();
                dados.ListaGeneros();

                dados.ClienteID = Convert.ToInt32(collection["ClienteID"]);
                dados.NomeCliente = collection["NomeCliente"];
                dados.EmailCliente = collection["EmailCliente"];
                dados.TelefoneCliente = collection["Telefone"];
                dados.TipoPessoa = collection["TipoPessoa"];
                dados.CpfCnpj = collection["CpfCnpj"];
                dados.RgIe = collection["RgIe"];
                dados.Isento = Convert.ToBoolean(collection["Isento"]);
                dados.Genero = collection["Genero"];
                dados.DataNascimento = collection["DataNascimento"];
                dados.Senha = collection["Senha"];
                dados.ConfirmarSenha = collection["ConfirmarSenha"];
                dados.Status = Convert.ToBoolean(collection["Status"]);

                if (!String.IsNullOrEmpty(dados.Senha) && !String.IsNullOrEmpty(dados.ConfirmarSenha))
                {
                    if (dados.Senha != dados.ConfirmarSenha)
                    {
                        dados.MensagemErro = "As senhas informadas não conferem!";
                        return View(dados);
                    }
                    dados.Senha = HashValue(collection["Senha"]);
                }

                if (dados.VerificarCPF() == true)
                {
                    dados.MensagemErro = "Este CPF/ CNPJ já está cadastrado para outro Cliente.";
                    return View(dados);
                }

                if (dados.VerificaEmail() == true)
                {
                    dados.MensagemErro = "Este e-mail já está cadastrado para outro Cliente.";
                    return View(dados);
                }

                if (dados.TipoPessoa == "F" && dados.Isento == false)
                {
                    if (dados.VerificaInscricao() == true)
                    {
                        dados.MensagemErro = "Esta Inscrição Estadual já está cadastrada para outro Cliente.";
                    }
                }

                dados.AlterarCliente();

                return RedirectToAction("Index", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.msg = DBModel.ViewBagMessage(ex.Message);
                return RedirectToAction("Create", "Cliente");
            }
        }
    }
}
