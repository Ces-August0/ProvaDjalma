using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProvaDjalma.Models;
using MySql.Data.MySqlClient;

namespace ProvaDjalma.Controllers
{
    public class ProdutoController : Controller
    {
        // GET: Produto
        public ActionResult Index(Produto produto)
        {
            var lstMarcas = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM produtos order by nome;";
                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };
                            lstMarcas.Add(marca);
                        }
                    }
                    ViewBag.ListaMarcas = lstMarcas;
                }
            }

            using (var conexao = new Conexao())
            {
                string strProdutos = "SELECT * FROM produtos " +
                "WHERE nome like @nome;";

                using (var comando = new MySqlCommand(strProdutos, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@nome", produto.Nome + "%");

                    MySqlDataReader dr = comando.ExecuteReader();

                    if (dr.HasRows)
                    {
                        var lstProdutos = new List<Produto>();

                        while (dr.Read())
                        {
                            var prod = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Preco = Convert.ToInt32(dr["preco"]),
                                Marca = Convert.ToString(dr["marca"])
                            };
                            lstProdutos.Add(prod);
                        }
                        ViewBag.ListaProdutos = lstProdutos;
                        return View();
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }

        public ActionResult NovoProduto()
        {
            var lstMarcas = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM produtos order by nome;";
                using (var comando = new MySqlCommand(strMarcas, conexao.conn))
                {
                    MySqlDataReader dr = comando.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };
                            lstMarcas.Add(marca);
                        }
                    }
                    ViewBag.ListaMarcas = lstMarcas;
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (var conexao = new Conexao())
            {
                string strProduto = "SELECT * FROM produtos " +
                                    "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strProduto, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Preco = Convert.ToInt32(dr["preco"]),
                            Marca = Convert.ToString(dr["marca"])
                        };
                        return View(produto);
                    }
                    else
                    {
                        ViewBag.ErroEdicao = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        public ActionResult SalvarProduto(Produto produto)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strInserirProduto = "INSERT INTO produtos (nome, preco, marca) " +
                                               "VALUES (@nome, @preco, @marca);";

                    using (var comando = new MySqlCommand(strInserirProduto, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", produto.Nome);
                        comando.Parameters.AddWithValue("@preco", produto.Preco);
                        comando.Parameters.AddWithValue("@marca", produto.Marca);

                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar o produto: " + ex.Message;
                return View("NovoProduto");
            }
        }

        public ActionResult Excluir(int id)
        {
            using (var conexao = new Conexao())
            {
                string strProduto = "SELECT * FROM produtos " +
                                    "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strProduto, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var produto = new Produto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"]),
                            Preco = Convert.ToInt32(dr["preco"]),
                            Marca = Convert.ToString(dr["marca"])
                        };
                        return View(produto);
                    }
                    else
                    {
                        ViewBag.ErroExclusao = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Excluir(Produto produto)
        {
            using (var conexao = new Conexao())
            {
                string strExcluir = "DELETE FROM produtos WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strExcluir, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", produto.Id);
                    comando.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult SalvarAlteracoesProduto(Produto produto)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strAtualizarProduto = "UPDATE produtos " +
                                               "SET nome = @nome, preco = @preco, marca = @marca " +
                                               "WHERE Id = @Id;";

                    using (var comando = new MySqlCommand(strAtualizarProduto, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", produto.Nome);
                        comando.Parameters.AddWithValue("@preco", produto.Preco);
                        comando.Parameters.AddWithValue("@marca", produto.Marca);
                        comando.Parameters.AddWithValue("@Id", produto.Id);

                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar as alterações do produto: " + ex.Message;
                return View("Edit", produto); 
            }
        }

        public ActionResult Visualizar(int id)
        {
            using (var conexao = new Conexao())
            {
                string strVisualizarProduto = "SELECT * FROM produtos WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strVisualizarProduto, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        MySqlDataReader dr = comando.ExecuteReader();

                        if (dr.Read())
                        {
                            var produto = new Produto
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"]),
                                Preco = Convert.ToInt32(dr["preco"]),
                                Marca = Convert.ToString(dr["marca"])
                            };

                            return View(produto);
                        }
                        else
                        {
                            ViewBag.ErroVisualizacao = true;
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Ocorreu um erro ao visualizar o produto: " + ex.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

    }
}
