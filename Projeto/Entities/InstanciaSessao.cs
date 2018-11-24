using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class InstanciaSessao
    {
        public Sessao sessao { get; set; }
        public string Title { get; set; }
        public string dia { get; set; }
        public int num_sala { get; set; }
        public int id_filme { get; set; }

        public InstanciaSessao() { }
        public InstanciaSessao(Sessao sessao, string dia, int num_sala, int id_filme)
        {
            this.sessao = sessao;
            this.dia = dia;
            this.num_sala = num_sala;
            this.id_filme = id_filme;
            this.Title = sessao.hora;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
