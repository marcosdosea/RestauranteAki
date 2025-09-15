using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class NovoPedidoDto
    {
        public int IdMesa { get; set; }
        public string EmailPessoa { get; set; }
        public int IdPersonagem { get; set; }
        public List<ItemCardapioQuantidadeDto> ItensCardapios { get; set; } = [];
    }

    public class ItemCardapioQuantidadeDto
    {
        public int ItemCardapioId { get; set; }
        public int Quantidade { get; set; }
    }
}
