using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.Logradouros
{
    public class LogradouroService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://quantodemora.somee.com/api/logradouros";

        private string _token;
        public LogradouroService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Logradouro>> GetLogradourosAsync()
        {
            string urlComplementar = string.Format("{0}", "/Listar");
            ObservableCollection<Models.Logradouro> listaLogradouros = await
            _request.GetAsync<ObservableCollection<Models.Logradouro>>(apiUrlBase + urlComplementar, _token);
            return listaLogradouros;
        }

        public async Task<Logradouro> GetLogradouroAsync(int logradouroId)
        {
            string urlComplementar = string.Format("/{0}", logradouroId);
            var logradouro = await _request.GetAsync<Models.Logradouro>(apiUrlBase + urlComplementar, _token);
            return logradouro;
        }
    }
}

