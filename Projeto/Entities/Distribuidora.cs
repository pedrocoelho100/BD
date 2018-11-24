using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD___Project.Entities
{
    public class Distribuidora
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string precoInicial { get; set; }
        public string comissaoBilhete { get; set; }

        public Distribuidora() { }
        public Distribuidora(int id, string nome, string precoInicial, string comissaoBilhete)
        {
            this.id = id;
            this.Title = nome;
            this.precoInicial = precoInicial;
            this.comissaoBilhete = comissaoBilhete;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
