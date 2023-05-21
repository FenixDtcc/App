using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Usuarios;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Usuarios
{
    internal class DadosUsuarioViewModel : BaseViewModel
    {
        UsuarioService uService;
        public ICommand SalvarCommand { get; set; }
        // public ICommand DeletarCommand { get; set; }

        public DadosUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            InicializarCommands();
            _ = CarregarDadosUsuario();
        }

        public void InicializarCommands()
        {
            SalvarCommand = new Command(AlterarDadosUsuario);
            // DeletarCommand = new Command(DeletarUsuario);

        }

        #region AtributosPropriedades

        public int id;
        public string nome;
        public string cpf;
        public string email;
        public string senha;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }

        public string Cpf
        {
            get => cpf;
            set
            {
                cpf = value;
                OnPropertyChanged();
            }
        }

        [EmailAddress]
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Métodos

        public async Task CarregarDadosUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                this.Id = u.IdUsuario;
                this.Cpf = u.Cpf;
                this.Nome = u.NomeUsuario;
                this.Email = u.Email;
                this.Senha = u.PasswordString;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
                throw;
            }
        }

        public async void AlterarDadosUsuario()
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    IdUsuario = this.id,
                    Cpf = this.cpf,
                    NomeUsuario = this.nome,
                    Email = this.email,
                    PasswordString = this.senha
                };

                if (usuario.IdUsuario != 0)
                {
                    if(!usuario.NomeUsuario.Equals(this.nome))
                    {
                        await uService.PutAlterarNomeAsync(usuario);
                    }
                    if(!usuario.Email.Equals(this.email))
                    {
                        await uService.PutAlterarEmailAsync(usuario);
                    }
                    if(!usuario.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarSenhaAsync(usuario);
                    }
                }

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        /*public async Task DeletarUsuario(Usuario u)
        {
            try
            {
                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Você confirma a exclusão do cadastro do usuário {u.Nome}?", "Sim", "Não"))
                {
                    await uService.DeletarUsuarioAsync(u);
                }

                Usuario u = await
                    uService.DeletarUsuarioAsync(usuarioId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
                throw;
            }
        }*/

        #endregion
    }
}
