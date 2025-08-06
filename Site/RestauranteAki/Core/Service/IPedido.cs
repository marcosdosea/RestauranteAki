using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPedido
    {
        public int Id { get; set; }

        /// <summary>
        /// status
        /// P - Pronto
        /// S - Solicitado
        /// E - Entregue
        /// </summary>
        public string? Status { get; set; }

        public int IdConta { get; set; }

        public int IdMesa { get; set; }

        public int IdPersonagem { get; set; }

        public int IdPessoa { get; set; }

        public  Contum IdContaNavigation { get; set; } 

        public  Mesa IdMesaNavigation { get; set; } 

        public  Personagem IdPersonagemNavigation { get; set; } 

        public  Pessoa IdPessoaNavigation { get; set; }

        public  ICollection<PedidoItemcardapio> PedidoItemcardapios { get; set; } 
    }
}
