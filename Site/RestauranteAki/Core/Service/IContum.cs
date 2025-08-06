using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IContum
    {
        public int Id { get; set; }

        public DateTime DataHoraEncerramento { get; set; }

        public float Valor { get; set; }

        public string FormaPagamento { get; set; }

        public string Status { get; set; }

        public int IdMesa { get; set; }

        public  Mesa IdMesaNavigation { get; set; }

        public  ICollection<Pagamento> Pagamentos { get; set; }

        public  ICollection<Pedido> Pedidos { get; set; }
    }
}
