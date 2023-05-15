using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.Hospitais
{
    public class HospitalService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://quantodemora.somee.com/api/hospitais";

        private string _token;
        public HospitalService(string token)
        {
            _request = new Request();
            _token = token;
        }


        public async Task<int> PostHospitaisAsync(Hospital h)
        {
            return await _request.PostReturnIntTokenAsync(apiUrlBase, h, _token);
        }

        public async Task<ObservableCollection<Hospital>> GetHospitaisAsync()
        {
            string urlComplementar = string.Format("{0}", "/Listar");
            ObservableCollection<Models.Hospital> listaHospitais = await
            _request.GetAsync<ObservableCollection<Models.Hospital>>(apiUrlBase + urlComplementar,
            _token);
            return listaHospitais;
        }

        public async Task<Hospital> GetHospitalAsync(int IdHospital)
        {
            string urlComplementar = string.Format("/{0}", IdHospital);
            var hospital = await _request.GetAsync<Models.Hospital>(apiUrlBase +
            urlComplementar, _token);
            return hospital;
        }

        public async Task<int> PutHospitalAsync(Hospital h)
        {
            var result = await _request.PutAsync(apiUrlBase, h, _token);
            return result;
        }
    }
}