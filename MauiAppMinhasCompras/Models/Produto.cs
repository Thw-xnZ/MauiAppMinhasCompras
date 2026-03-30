using SQLite;

namespace MauiAppMinhasCompras.Models
{   
    // Aqui em produtos, foi adicionado a validação de campos, para evitar que o usuário deixe campos em branco, usando o metodo If
    // e lançando uma throw de Exception para o usuário preencher os campos
    public class Produto
    {
        string _descricao;
        double _quantidade;
        double _preco;
        string _categoria;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
                _descricao = value;
            }
        }
        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value == 0)
                {
                    throw new Exception("Por favor, preencha a Quantidade");
                }
                _quantidade = value;
            }
        }
        public double Preco
        {
            get => _preco;
            set
            {
                if (value == 0)
                {
                    throw new Exception("Por favor, preencha valor do Preço");
                }
                _preco = value;
            }
        }
        public double Total { get => Quantidade * Preco; }

        public string Categoria
        {
            get => _categoria;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha qual a Categoria");
                }
                _categoria = value;
            }
        }
    }
}
// Adicionado a propriedade Total, que é calculada multiplicando a Qtd pelo Preço, facilitando a exibição do valor