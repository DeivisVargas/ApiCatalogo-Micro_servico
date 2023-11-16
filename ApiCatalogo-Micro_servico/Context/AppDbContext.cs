using ApiCatalogo_Micro_servico.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo_Micro_servico.Context
{
    //ponte entre a aplicação e o banco de dados 
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {}

        //mapeamento das entidades para as tabelas do banco de dados 
        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }


        //sobrescrita do metodo na criação das tabelas 
        protected override void OnModelCreating(ModelBuilder mb)
        {
            //configurando a modelagem da tabela categoria 
            mb.Entity<Categoria>().HasKey( c => c.CategoriaId);
            mb.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
            mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();


            //configurando a modelagem da tabela produto
            mb.Entity<Produto>().HasKey(c => c.ProdutoId);
            mb.Entity<Produto>().Property(c => c.Nome).HasMaxLength(100).IsRequired();
            mb.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();
            mb.Entity<Produto>().Property(c => c.Imagem).HasMaxLength(100);
            mb.Entity<Produto>().Property(c => c.Preco).HasPrecision(14,2).IsRequired();

            //relacionamento
            //um produto contém uma categoria e uma categoria contém muitos produtos 
            mb.Entity<Produto>()
                .HasOne<Categoria>(c => c.Categoria)
                 .WithMany(p => p.Produtos)
                  .HasForeignKey(c => c.CategoriaId);

        }
    }
}
