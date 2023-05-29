using QuantoDemoraApp.Models;
using QuantoDemoraApp.Services.Usuarios;
using QuantoDemoraApp.Views.Usuarios;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace QuantoDemoraApp.ViewModels.Usuarios
{
    internal class DadosUsuarioViewModel : BaseViewModel
    {
        UsuarioService uService;
        public ICommand AbrirAlteracaoDadosUsuarioCommand { get; set; }
        public ICommand SalvarCommand { get; set; }
        public ICommand AlterarNomeCommand { get; set; }
        public ICommand AlterarEmailCommand { get; set; }
        public ICommand AlterarSenhaCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand DeletarCommand { get; set; }

        public DadosUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            InicializarCommands();
            _ = CarregarDadosUsuario();
        }

        public void InicializarCommands()
        {
            AbrirAlteracaoDadosUsuarioCommand = new Command(AbrirAlteracaoDadosUsuario);
            SalvarCommand = new Command(AlterarDadosUsuario);
            AlterarNomeCommand = new Command(AlterarNomeUsuario);
            AlterarEmailCommand = new Command(AlterarEmailUsuario);
            AlterarSenhaCommand = new Command(AlterarSenhaUsuario);
            CancelarCommand = new Command(CancelarAlteracao);
            DeletarCommand = new Command(DeletarUsuario);
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

        public async void AbrirAlteracaoDadosUsuario()
        {
            try
            {
                await Shell.Current.GoToAsync("alterarDadosUsuarioView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void AlterarDadosUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.NomeUsuario = this.nome;
                u.Email = this.email;
                u.PasswordString = this.senha;

                if (u.IdUsuario == usuarioId)
                {
                    //if (u.NomeUsuario.Equals(this.nome))
                    //{
                    //    await uService.PutAlterarNomeAsync(u);
                    //}
                    //if (u.Email.Equals(this.email))
                    //{
                    //    await uService.PutAlterarEmailAsync(u);
                    //}
                    //if (u.PasswordString.Equals(this.senha))
                    //{
                    //    await uService.PutAlterarSenhaAsync(u);
                    //}
                    if (u.NomeUsuario.Equals(this.nome) && u.Email.Equals(this.email) && u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarNomeAsync(u);
                        await uService.PutAlterarEmailAsync(u);
                        await uService.PutAlterarSenhaAsync(u);
                    }
                    if (u.NomeUsuario.Equals(this.nome) && u.Email.Equals(this.email) && !u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarNomeAsync(u);
                        await uService.PutAlterarEmailAsync(u);
                    }
                    if (u.NomeUsuario.Equals(this.nome) && !u.Email.Equals(this.email) && u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarNomeAsync(u);
                        await uService.PutAlterarSenhaAsync(u);
                    }
                    if (!u.NomeUsuario.Equals(this.nome) && u.Email.Equals(this.email) && u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarEmailAsync(u);
                        await uService.PutAlterarSenhaAsync(u);
                    }
                    if (u.NomeUsuario.Equals(this.nome) && !u.Email.Equals(this.email) && !u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarNomeAsync(u);
                    }
                    if (!u.NomeUsuario.Equals(this.nome) && u.Email.Equals(this.email) && !u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarEmailAsync(u);
                    }
                    if (!u.NomeUsuario.Equals(this.nome) && !u.Email.Equals(this.email) && u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarSenhaAsync(u);
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

        public async void AlterarNomeUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.NomeUsuario = this.nome;

                if (u.IdUsuario == usuarioId)
                {
                    if (u.NomeUsuario.Equals(this.nome))
                    {
                        await uService.PutAlterarNomeAsync(u);
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

        public async void AlterarEmailUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.Email = this.email;

                if (u.IdUsuario == usuarioId)
                {
                    if (u.Email.Equals(this.email))
                    {
                        await uService.PutAlterarEmailAsync(u);
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

        public async void AlterarSenhaUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                u.PasswordString = this.senha;

                if (u.IdUsuario == usuarioId)
                {
                    if (u.PasswordString.Equals(this.senha))
                    {
                        await uService.PutAlterarSenhaAsync(u);
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

        private async void CancelarAlteracao()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async void DeletarUsuario()
        {
            try
            {
                Usuario usuario = new Usuario();
                var usuarioId = Preferences.Get("UsuarioId", usuario.IdUsuario);

                Usuario u = await
                    uService.GetUsuarioAsync(usuarioId);

                if (await Application.Current.MainPage
                    .DisplayAlert("Confirmação", $"Confirma a remoção do usuário {u.NomeUsuario}?", "Sim", "Não"))
                {
                    await uService.DeletarUsuarioAsync(u.IdUsuario);

                    await Application.Current.MainPage.DisplayAlert("Mensagem",
                            $"Usuário {u.NomeUsuario} removido com sucesso!", "Ok");

                    Application.Current.Quit();
                }

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
                throw;
            }
        }

        #endregion
    }
}
