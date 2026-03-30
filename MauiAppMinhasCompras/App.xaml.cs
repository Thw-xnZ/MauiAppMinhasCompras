using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        // Nomeamos aqui o database como static para que seja possível acessar de qualquer lugar do projeto e
        // abreviamos como "Db" para facilitar a escrita do código.
        static SQLiteDatabaseHelper _db;

        // Definimos a propriedade "Db" para retornar a instância do banco de dados.
        // Se a instância ainda não existir, ela será criada usando o
        // caminho do arquivo localizado na pasta de dados local do aplicativo.
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData), "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }
                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            MainPage = new NavigationPage(new Views.ListaProduto());
        }

    }
}