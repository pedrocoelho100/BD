using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Sessao
    {
        public int id_cinema { get; set; }
        public int dia_semana { get; set; }
        public string hora { get; set; }
        public string desconto { get; set; }

        public Sessao() { }
        public Sessao(int id_cinema, int dia_semana, string hora, string desconto)
        {
            this.id_cinema = id_cinema;
            this.dia_semana = dia_semana;
            this.hora = hora;
            this.desconto = desconto;
        }

        public override string ToString()
        {
            return hora.ToString();
        }
    }
}
