using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Sala
    {
        public int id_cinema { get; set; }
        public int num_sala { get; set; }
        public int num_filas { get; set; }
        public int num_lugares_fila { get; set; }
        public DataTable lugares { get; set; }

        public Sala() { }
        public Sala(int id_cinema, int num_sala, int num_filas, int num_lugares_fila, DataTable lugares)
        {
            this.id_cinema = id_cinema;
            this.num_sala = num_sala;
            this.num_filas = num_filas;
            this.num_lugares_fila = num_lugares_fila;
            this.lugares = lugares;
        }

        public override string ToString()
        {
            return num_sala.ToString();
        }
    }
}
