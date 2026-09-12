using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCategoriaPage : ContentPage
{
    public RelatorioCategoriaPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Define "Todas" como padrão caso nada esteja selecionado
        if (pck_filtro_categoria.SelectedIndex == -1)
        {
            pck_filtro_categoria.SelectedIndex = 0;
        }
        else
        {
            // Se já houver item selecionado, carrega explicitamente
            await CarregarRelatorio();
        }
    }

    private async Task CarregarRelatorio()
    {
        try
        {
            // 1. Busca todos os produtos do SQLite
            List<Produto> lista = await App.Db.GetAll();

            string categoriaSelecionada = pck_filtro_categoria.SelectedItem?.ToString();

            // 2. Filtra ignorando diferenças de maiúsculas/minúsculas e tratando nulos
            if (!string.IsNullOrEmpty(categoriaSelecionada) && categoriaSelecionada != "Todas")
            {
                lista = lista.Where(p => p.Categoria != null &&
                                         p.Categoria.Trim().Equals(categoriaSelecionada.Trim(), StringComparison.OrdinalIgnoreCase))
                             .ToList();
            }

            // 3. Força a atualização da ListView no MAUI
            lst_produtos_categoria.ItemsSource = null;
            lst_produtos_categoria.ItemsSource = lista;

            // 4. Calcula o total acumulado
            double total = lista.Sum(p => p.Total);
            lbl_total_categoria.Text = total.ToString("C");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void pck_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        await CarregarRelatorio();
    }
}