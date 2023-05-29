using QuantoDemoraApp.ViewModels.Hospitais;
using QuantoDemoraApp.ViewModels;
using QuantoDemoraApp.Views.Hospitais;
using QuantoDemoraApp.Views.Usuarios;

namespace QuantoDemoraApp;

public partial class AppShell : Shell
{
    AppShellViewModel viewModel;
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("informacoesHospitalView", typeof(InformacoesHospitalView));
        Routing.RegisterRoute("alterarDadosUsuarioView", typeof(AlterarDadosUsuarioView));

        viewModel = new AppShellViewModel();
        BindingContext = viewModel;

        string login = Preferences.Get("UsuarioUsername", string.Empty);
        lblLogin.Text = $"Login: {login}";
    }

}
