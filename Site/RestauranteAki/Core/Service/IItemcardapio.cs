using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IItemcardapio
    {
        public int Id { get; set; }

        public string Nome { get; set; } 

        public string? Descricao { get; set; }

        public float PrecoUnitario { get; set; }

        public int Porcao { get; set; }

        public string DiaSemana { get; set; } 

        public bool Status { get; set; }

        public byte[] Imagem { get; set; } 

        public int Categoria { get; set; }

        public  ICollection<PedidoItemcardapio> PedidoItemcardapios { get; set; }

        public  ICollection<Cardapio> IdCardapios { get; set; }
    
    }
}
