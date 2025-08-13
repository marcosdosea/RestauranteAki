using System;
using System.Collections.Generic;

namespace Core.Service
{
    public interface ICardapio
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public sbyte Ativo { get; set; }

        public int IdRestaurante { get; set; }

        public Restaurante IdRestauranteNavigation { get; set; }

        public ICollection<Itemcardapio> IdItemCardapios { get; set; }
    }
}
