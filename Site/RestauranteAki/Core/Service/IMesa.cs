using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IMesa
    {

        public int Id { get; set; }

        public byte[]? Imagem { get; set; }

        public  ICollection<Contum> Conta { get; set; } 

        public  ICollection<Pedido> Pedidos { get; set; }
    }
}
