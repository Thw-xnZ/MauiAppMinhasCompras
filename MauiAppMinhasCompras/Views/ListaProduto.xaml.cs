namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	public ListaProduto()
	{
		InitializeComponent();
	}

    // Botão que foi criado para a utilização do ToolbarItem, para navegar para a página de cadastro de produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
        } catch (Exception ex)
		{
			DisplayAlert("Ops", "Ocorreu um erro: {ex.Message}", "OK");
			return;
        }
    }
}