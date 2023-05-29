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

        public async Task<ObservableCollection<Hospital>> GetHospitaisAsync()
        {
            string urlComplementar = string.Format("{0}", "/Listar");
            ObservableCollection<Models.Hospital> listaHospitais = await
            _request.GetAsync<ObservableCollection<Models.Hospital>>(apiUrlBase + urlComplementar, _token);
            return listaHospitais;
        }

        public async Task<Hospital> GetHospitalAsync(int hospitalId)
        {
            string urlComplementar = string.Format("/{0}", hospitalId);
            var hospital = await _request.GetAsync<Models.Hospital>(apiUrlBase + urlComplementar, _token);
            return hospital;
        }

        public async Task<bool> GetLocalizacaoAsync(double latitude, double longitude, string placeIdGoogleMaps)
        {
            string mapaUrlBase = 
                $"http://www.google.com/maps/search/?api=1&query={latitude.ToString().Replace(",", ".")}%2C{longitude.ToString().Replace(",", ".")}&query_place_id={placeIdGoogleMaps}";
            var mapaHospital = await _request.OpenUrlAsync<Hospital>(mapaUrlBase);
            return mapaHospital;
        }
    }
}