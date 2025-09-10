using Microsoft.EntityFrameworkCore;

namespace Core;

public partial class RestauranteAkiContext : DbContext
{
    public RestauranteAkiContext()
    {
    }

    public RestauranteAkiContext(DbContextOptions<RestauranteAkiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cardapio> Cardapios { get; set; }

    public virtual DbSet<Contum> Conta { get; set; }

    public virtual DbSet<Garcom> Garcoms { get; set; }

    public virtual DbSet<Itemcardapio> Itemcardapios { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoItemcardapio> PedidoItemcardapios { get; set; }

    public virtual DbSet<Personagem> Personagems { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Restaurante> Restaurantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=123456;database=restaurantedb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cardapio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cardapio");

            entity.HasIndex(e => e.IdRestaurante, "fk_cardapio_restaurante1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ativo)
                .HasDefaultValueSql("'1'")
                .HasColumnName("ativo");
            entity.Property(e => e.DataFim)
                .HasColumnType("date")
                .HasColumnName("dataFim");
            entity.Property(e => e.DataInicio)
                .HasColumnType("date")
                .HasColumnName("dataInicio");
            entity.Property(e => e.IdRestaurante).HasColumnName("idRestaurante");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdRestauranteNavigation).WithMany(p => p.Cardapios)
                .HasForeignKey(d => d.IdRestaurante)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_cardapio_restaurante1");

            entity.HasMany(d => d.IdItemCardapios).WithMany(p => p.IdCardapios)
                .UsingEntity<Dictionary<string, object>>(
                    "CardapioItemcardapio",
                    r => r.HasOne<Itemcardapio>().WithMany()
                        .HasForeignKey("IdItemCardapio")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_cardapio__itemcardapio_itemcardapio1"),
                    l => l.HasOne<Cardapio>().WithMany()
                        .HasForeignKey("IdCardapio")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("fk_cardapio__itemcardapio_cardapio1"),
                    j =>
                    {
                        j.HasKey("IdCardapio", "IdItemCardapio").HasName("PRIMARY");
                        j.ToTable("cardapio__itemcardapio");
                        j.HasIndex(new[] { "IdCardapio" }, "fk_cardapio__itemcardapio_cardapio1_idx");
                        j.HasIndex(new[] { "IdItemCardapio" }, "fk_cardapio__itemcardapio_itemcardapio1_idx");
                        j.IndexerProperty<int>("IdCardapio").HasColumnName("idCardapio");
                        j.IndexerProperty<int>("IdItemCardapio").HasColumnName("idItemCardapio");
                    });
        });

        modelBuilder.Entity<Contum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("conta");

            entity.HasIndex(e => e.IdMesa, "fk_conta_mesa1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataHoraEncerramento)
                .HasColumnType("datetime")
                .HasColumnName("dataHoraEncerramento");
            entity.Property(e => e.FormaPagamento)
                .HasMaxLength(50)
                .HasColumnName("formaPagamento");
            entity.Property(e => e.IdMesa).HasColumnName("idMesa");
            entity.Property(e => e.Status)
                .HasColumnType("enum('F','A')")
                .HasColumnName("status");
            entity.Property(e => e.Valor).HasColumnName("valor");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Conta)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_conta_mesa1");
        });

        modelBuilder.Entity<Garcom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("garcom");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<Itemcardapio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("itemcardapio");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Categoria).HasColumnName("categoria");
            entity.Property(e => e.Descricao)
                .HasColumnType("text")
                .HasColumnName("descricao");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(15)
                .HasColumnName("diaSemana");
            entity.Property(e => e.Imagem).HasColumnName("imagem");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.Porcao).HasColumnName("porcao");
            entity.Property(e => e.PrecoUnitario).HasColumnName("precoUnitario");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mesa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Imagem).HasColumnName("imagem");
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pagamento");

            entity.HasIndex(e => e.IdConta, "fk_pagamento_conta1_idx");

            entity.HasIndex(e => e.IdPersonagem, "fk_pagamento_personagem1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataHora)
                .HasColumnType("datetime")
                .HasColumnName("dataHora");
            entity.Property(e => e.IdConta).HasColumnName("idConta");
            entity.Property(e => e.IdPersonagem).HasColumnName("idPersonagem");
            entity.Property(e => e.TipoPagamento)
                .HasComment("C - Cartao\nD - Dinheiro\nP - Pix")
                .HasColumnType("enum('C','D','P')")
                .HasColumnName("tipoPagamento");
            entity.Property(e => e.ValorPagamento).HasColumnName("valorPagamento");

            entity.HasOne(d => d.IdContaNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdConta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pagamento_conta1");

            entity.HasOne(d => d.IdPersonagemNavigation).WithMany(p => p.Pagamentos)
                .HasForeignKey(d => d.IdPersonagem)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pagamento_personagem1");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pedido");

            entity.HasIndex(e => e.IdConta, "fk_pedido_conta_idx");

            entity.HasIndex(e => e.IdMesa, "fk_pedido_mesa1_idx");

            entity.HasIndex(e => e.IdPersonagem, "fk_pedido_personagem1_idx");

            entity.HasIndex(e => e.IdPessoa, "fk_pedido_pessoa1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdConta).HasColumnName("idConta");
            entity.Property(e => e.IdMesa).HasColumnName("idMesa");
            entity.Property(e => e.IdPersonagem).HasColumnName("idPersonagem");
            entity.Property(e => e.IdPessoa).HasColumnName("idPessoa");
            entity.Property(e => e.Status)
                .HasComment("status\nP - Pronto\nS - Solicitado\nE - Entregue")
                .HasColumnType("enum('P','S','E')")
                .HasColumnName("status");

            entity.HasOne(d => d.IdContaNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdConta)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido_conta");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido_mesa1");

            entity.HasOne(d => d.IdPersonagemNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdPersonagem)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido_personagem1");

            entity.HasOne(d => d.IdPessoaNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdPessoa)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido_pessoa1");
        });

        modelBuilder.Entity<PedidoItemcardapio>(entity =>
        {
            entity.HasKey(e => new { e.IdPedido, e.IdItemCardapio }).HasName("PRIMARY");

            entity.ToTable("pedido__itemcardapio");

            entity.HasIndex(e => e.IdItemCardapio, "fk_pedido__itemcardapio_itemcardapio1_idx");

            entity.HasIndex(e => e.IdPedido, "fk_pedido__itemcardapio_pedido1_idx");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.IdItemCardapio).HasColumnName("idItemCardapio");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.IdItemCardapioNavigation).WithMany(p => p.PedidoItemcardapios)
                .HasForeignKey(d => d.IdItemCardapio)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido__itemcardapio_itemcardapio1");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.PedidoItemcardapios)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pedido__itemcardapio_pedido1");
        });

        modelBuilder.Entity<Personagem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("personagem");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataAtualizacao)
                .HasColumnType("datetime")
                .HasColumnName("dataAtualizacao");
            entity.Property(e => e.DataCriacao)
                .HasColumnType("datetime")
                .HasColumnName("dataCriacao");
            entity.Property(e => e.IdentificadorCor)
                .HasMaxLength(50)
                .HasColumnName("identificadorCor");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pessoa");

            entity.HasIndex(e => e.IdRestaurante, "fk_pessoa_restaurante1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(20)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Cpf)
                .HasMaxLength(14)
                .HasColumnName("cpf");
            entity.Property(e => e.DataNascimento)
                .HasColumnType("date")
                .HasColumnName("dataNascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.IdRestaurante).HasColumnName("idRestaurante");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(50)
                .HasColumnName("logradouro");
            entity.Property(e => e.NomeCompleto)
                .HasMaxLength(50)
                .HasColumnName("nomeCompleto");
            entity.Property(e => e.Telefone)
                .HasMaxLength(50)
                .HasColumnName("telefone");
            entity.Property(e => e.TipoPessoa)
                .HasComment("F - FUNCIONARIO\nG - GESTOR")
                .HasColumnType("enum('F','G')")
                .HasColumnName("tipoPessoa");

            entity.HasOne(d => d.IdRestauranteNavigation).WithMany(p => p.Pessoas)
                .HasForeignKey(d => d.IdRestaurante)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_pessoa_restaurante1");
        });

        modelBuilder.Entity<Restaurante>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("restaurante");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep)
                .HasMaxLength(20)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade");
            entity.Property(e => e.Cnpj)
                .HasMaxLength(20)
                .HasColumnName("cnpj");
            entity.Property(e => e.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasColumnName("estado");
            entity.Property(e => e.Logradouro)
                .HasMaxLength(50)
                .HasColumnName("logradouro");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome");
            entity.Property(e => e.NomeFantasia)
                .HasMaxLength(50)
                .HasColumnName("nomeFantasia");
            entity.Property(e => e.Numero).HasColumnName("numero");
        });

        OnModelCreatingPartial(modelBuilder);


    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
