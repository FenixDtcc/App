using QuantoDemoraApp.ViewModels.Hospitais;
using QuantoDemoraApp.ViewModels;
using QuantoDemoraApp.Views.Hospitais;

namespace QuantoDemoraApp;

public partial class AppShell : Shell
{
    AppShellViewModel viewModel;
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("informacoesHospitalView", typeof(InformacoesHospitalView));

        viewModel = new AppShellViewModel();
        BindingContext = viewModel;

        string login = Preferences.Get("UsuarioNomeUsuario", string.Empty);
        lblLogin.Text = $"Login: {login}";
    }

}
