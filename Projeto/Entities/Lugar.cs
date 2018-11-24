using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Lugar
    {
        public int id_cinema { get; set; }
        public int num_sala { get; set; }
        public int fila { get; set; }
        public int num_lugar { get; set; }
        public bool ocupado { get; set; }

        public Lugar() { }
        public Lugar(int id_cinema, int num_sala, int fila, int num_lugar, bool ocupado)
        {
            this.id_cinema = id_cinema;
            this.num_sala = num_sala;
            this.fila = fila;
            this.num_lugar = num_lugar;
            this.ocupado = ocupado;
        }

        public override string ToString()
        {
            return "";
        }
    }
}
