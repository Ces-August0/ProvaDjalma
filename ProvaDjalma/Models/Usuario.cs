using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProvaDjalma.Models
{   
        public class Usuario
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string UserPass { get; set; }
            public string Nome { get; set; }
            public string Celular { get; set; }
        }
}