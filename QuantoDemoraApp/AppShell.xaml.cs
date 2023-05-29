using QuantoDemoraApp.Views.Hospitais;
using QuantoDemoraApp.Views.Usuarios;

namespace QuantoDemoraApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("informacoesHospitalView", typeof(InformacoesHospitalView));
        Routing.RegisterRoute("alterarDadosUsuarioView", typeof(AlterarDadosUsuarioView));

        string login = Preferences.Get("UsuarioUsername", string.Empty);
        lblLogin.Text = $"Login: {login}";
    }

}
