using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class TipoBilhete
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string restricoes { get; set; }
        public string custo { get; set; }

        public TipoBilhete() { }
        public TipoBilhete(int id, string nome, string restricoes, string custo)
        {
            this.id = id;
            this.Title = nome;
            this.restricoes = restricoes;
            this.custo = custo;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
