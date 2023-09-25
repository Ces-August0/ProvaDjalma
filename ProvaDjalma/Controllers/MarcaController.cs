using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProvaDjalma.Models;
using MySql.Data.MySqlClient;

namespace ProvaDjalma.Controllers
{
    public class MarcaController : Controller
    {
        public ActionResult Index()
        {
            var lstMarcas = new List<Marca>();
            using (var conexao = new Conexao())
            {
                string strMarcas = "SELECT * FROM marcas order by nome;";
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
                }
            }

            ViewBag.ListaMarcas = lstMarcas;
            return View(lstMarcas);
        }

        public ActionResult NovaMarca()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (var conexao = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas " +
                                    "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strMarca, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var marca = new Marca
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"])
                        };
                        return View(marca);
                    }
                    else
                    {
                        ViewBag.ErroEdicao = true;
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult SalvarMarca(Marca marca)
        {
            try
            {
                using (var conexao = new Conexao())
                {
                    string strInserirMarca = "INSERT INTO marcas (nome) " +
                                               "VALUES (@nome);";

                    using (var comando = new MySqlCommand(strInserirMarca, conexao.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", marca.Nome);

                        comando.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao salvar a marca: " + ex.Message;
                return View("NovaMarca");
            }
        }

        public ActionResult Excluir(int id)
        {
            using (var conexao = new Conexao())
            {
                string strMarca = "SELECT * FROM marcas " +
                                    "WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strMarca, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    MySqlDataReader dr = comando.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        var marca = new Marca
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nome = Convert.ToString(dr["nome"])
                        };
                        return View(marca);
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
        public ActionResult Excluir(Marca marca)
        {
            using (var conexao = new Conexao())
            {
                string strExcluir = "DELETE FROM marcas WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strExcluir, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", marca.Id);
                    comando.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Visualizar(int id)
        {
            using (var conexao = new Conexao())
            {
                string strVisualizarMarca = "SELECT * FROM marcas WHERE Id = @Id;";

                using (var comando = new MySqlCommand(strVisualizarMarca, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@Id", id);

                    try
                    {
                        MySqlDataReader dr = comando.ExecuteReader();

                        if (dr.Read())
                        {
                            var marca = new Marca
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nome = Convert.ToString(dr["nome"])
                            };

                            return View(marca);
                        }
                        else
                        {
                            ViewBag.ErroVisualizacao = true;
                            return RedirectToAction("Index");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Ocorreu um erro ao visualizar a marca: " + ex.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
        }
    }
}
