using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Filme
    {
        public int id { get; set; }
        public int dist { get; set; }
        public string Title { get; set; }
        public int idadeMin { get; set; }
        public int duracao { get; set; }
        public string estreia { get; set; }
        public string idioma { get; set; }

        public Filme() { }
        public Filme(int id, int dist, string nome, int idadeMin, int duracao, string estreia, string idioma)
        {
            this.id = id;
            this.dist = dist;
            this.Title = nome;
            this.idadeMin = idadeMin;
            this.duracao = duracao;
            this.estreia = estreia;
            this.idioma = idioma;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
