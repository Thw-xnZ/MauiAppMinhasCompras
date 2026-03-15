using SQLite;

namespace MauiAppMinhasCompras.Models
{   
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }
    }
}
// Adicionado a propriedade Total, que é calculada multiplicando a Qtd pelo Preço, facilitando a exibição do valor