using BD___Project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Empregado
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string nif { get; set; }
        public string email { get; set; }
        public string salario { get; set; }
        public int cinema { get; set; }

        public Empregado() { }
        public Empregado(int id, string nome, string nif, string email, string salario, int cinema)
        {
            this.id = id;
            this.Title = nome;
            this.nif = nif;
            this.email = email;
            this.salario = salario;
            this.cinema = cinema;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
