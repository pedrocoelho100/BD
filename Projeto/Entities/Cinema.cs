using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Cinema
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string morada { get; set; }
        public string telefone { get; set; }
        public int gerente { get; set; } = -1;

        public Cinema() { }
        public Cinema(int id, string nome, string morada, string telefone, int gerente)
        {
            this.id = id;
            this.Title = nome;
            this.morada = morada;
            this.telefone = telefone;
            this.gerente = gerente;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
