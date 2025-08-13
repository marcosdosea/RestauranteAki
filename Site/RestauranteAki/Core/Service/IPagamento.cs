using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPagamento
    {
        public int Id { get; set; }

        /// <summary>
        /// C - Cartao
        /// D - Dinheiro
        /// P - Pix
        /// </summary>

        public string TipoPagamento { get; set; } 

        public DateTime DataHora { get; set; }

        public float ValorPagamento { get; set; }

        public int IdConta { get; set; }

        public int IdPersonagem { get; set; }

        public  Contum IdContaNavigation { get; set; } 

        public  Personagem IdPersonagemNavigation { get; set; } 
    }
}
