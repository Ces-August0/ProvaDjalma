using System;
using System.ComponentModel.DataAnnotations;

namespace ProvaDjalma.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Preco { get; set; }
        public string Marca { get; set; }
    }
}
