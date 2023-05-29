using QuantoDemoraApp.ViewModels.Usuarios;

namespace QuantoDemoraApp.Views.Usuarios;

public partial class DadosUsuarioView : ContentPage
{
    private DadosUsuarioViewModel dadosUsuarioViewModel;
    public DadosUsuarioView()
    {
        InitializeComponent();

        dadosUsuarioViewModel = new DadosUsuarioViewModel();
        BindingContext = dadosUsuarioViewModel;
        Title = "Dados do Usuário";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = dadosUsuarioViewModel.CarregarDadosUsuario();
    }
}