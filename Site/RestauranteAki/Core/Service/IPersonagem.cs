using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPersonagem
    {
        public int Id { get; set; }

        public string IdentificadorCor { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; } 

        public ICollection<Pedido> Pedidos { get; set; } 
    }
}
