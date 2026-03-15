using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        // SQLiteAsyncConnection é a classe que representa a conexão assíncrona
        // com o banco de dados SQLite, e a nomeamos como _conn.
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        // O método Insert recebe um objeto do tipo Produto e retorna uma Task<int> que representa a
        // operação assíncrona de inserção no banco de dados.
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }
        // O método Update recebe um objeto do tipo Produto e retorna uma Task<List<Produto>> que substitui
        // o produto existente no banco de dados com os novos valores fornecidos.
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(
            sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }
        // O método Delete recebe um inteiro id e retorna uma Task<int> que representa a operação assíncrona
        // de exclusão do produto com o id especificado no banco de dados.
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        // O método GetAll retorna uma Task<List<Produto>> que traz todos os produtos armazenados no
        // banco de dados, permitindo que o aplicativo exiba a lista completa de produtos para o usuário.
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        // O método Search recebe uma string q e retorna uma Task<List<Produto>> que representa a
        // operação assíncrona de busca no banco de dados, retornando os produtos cuja descrição contenha a string fornecida.
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }
        // Houve uma alteração sendo adicionado "FROM" na string sql, para corrigir o erro de sintaxe na consulta SQL,
        // garantindo que a consulta seja executada corretamente
    }
}
