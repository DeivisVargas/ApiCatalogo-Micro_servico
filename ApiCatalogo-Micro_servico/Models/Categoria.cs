using System.Text.Json.Serialization;

namespace ApiCatalogo_Micro_servico.Models
{
    public class Categoria
    {

        public int? CategoriaId { get; set; }

        public string? Nome { get; set; }

        public string? Descricao { get; set; }


        //uma categoria pode ter vários produtos 

        //para omitir essa lista de produtos no cadastro da categoria pois ela e so uma propriedade 
        //de navegação 
        [JsonIgnore]
        public ICollection<Produto>? Produtos { get; set; }

    }
}
