using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProvaDjalma.Models;
using MySql.Data.MySqlClient;

namespace ProvaDjalma.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index(Cliente cli)
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var usario = new Usuario
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };

                            lstVendedores.Add(usario);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }

            using (var conexao = new Conexao())
            {

                string strClientes = "SELECT * FROM clientes " +
                "WHERE nome like @nome and " +
                "isExcluido = false;";

                using (var comando = new MySqlCommand(strClientes, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", cli.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstClientes = new List<Cliente>();

                        while (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Telefone = Convert.ToString(dr["telefone"]),
                                EMail = Convert.ToString(dr["email"]),                      
                                DataNasc = Convert.ToDateTime(dr["dataNasc"]).ToString("dd/MM/yyyy"),
                                Vendedor = Convert.ToString(dr["vendedor"])
                            };

                            lstClientes.Add(cliente);
                        }
                        ViewBag.ListaClientes = lstClientes;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoCliente()
        {
            var lstVendedores = new List<Usuario>();
            using (var conexao = new Conexao())
            {
                string strVendedores = "SELECT * FROM usuarios where isExcluido = false order by nome;";
                using (var comando = new MySqlCommand(strVendedores, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            var usario = new Usuario
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };
                            lstVendedores.Add(usario);
                        }
                    ViewBag.ListaVendedores = lstVendedores;
                }
            }
            return View();
        }

        public ActionResult EditarCliente()
        {
            return View();
        }

        public ActionResult SalvarCliente(Cliente cliente)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strInserirCliente = "INSERT INTO clientes (nome, telefone, email, dataNasc) " +
                                               "VALUES (@nome, @telefone, @email, @dataNasc);";

                    using (var comando = new MySqlCommand(strInserirCliente, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", cliente.Nome);
                        comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                        comando.Parameters.AddWithValue("@email", cliente.EMail);
                        comando.Parameters.AddWithValue("@dataNasc", Convert.ToDateTime(cliente.DataNasc));

                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar o cliente: " + ex.Message;
                return View("NovoCliente");
            }
        }

        public ActionResult Excluir(int id)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strExcluir = "UPDATE clientes SET isExcluido = 1 WHERE Id = @Id;";

                    using (var comando = new MySqlCommand(strExcluir, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@Id", id);
                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao excluir o cliente: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Visualizar(int id)
        {
            using (var conexao = new Conexao())
            {
                string strVisualizarCliente = "SELECT * FROM clientes WHERE Id = @Id AND isExcluido = 0;";

                using (var comando = new MySqlCommand(strVisualizarCliente, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        MySqlDataReader dr = comando.ExecuteReader();

                        if (dr.Read())
                        {
                            var cliente = new Cliente
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Telefone = Convert.ToString(dr["telefone"]),
                                EMail = Convert.ToString(dr["email"]),
                                DataNasc = Convert.ToDateTime(dr["dataNasc"]).ToString("dd/MM/yyyy")
                            };

                            return View(cliente);
                        }
                        else
                        {
                            ViewBag.ErroVisualizacao = true;
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Ocorreu um erro ao visualizar o cliente: " + ex.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult SalvarAlteracoesCliente(Cliente cliente)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strAtualizarCliente = "UPDATE clientes SET " +
                                                 "nome = @nome, " +
                                                 "telefone = @telefone, " +
                                                 "email = @email " +
                                                 "WHERE Id = @Id;";

                    using (var comando = new MySqlCommand(strAtualizarCliente, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", cliente.Nome);
                        comando.Parameters.AddWithValue("@telefone", cliente.Telefone);
                        comando.Parameters.AddWithValue("@email", cliente.EMail);
                        comando.Parameters.AddWithValue("@Id", cliente.Id);

                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar as alterações do cliente: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}