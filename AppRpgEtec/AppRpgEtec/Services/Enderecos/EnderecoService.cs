using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppRpgEtec.Models;

namespace AppRpgEtec.Services.Enderecos
{
    public class EnderecoService
    {
        private readonly Request _request;
        private const string _apiUrlBase = "https://rpgapi3ai2025.azurewebsites.net/Enderecos";
        //Azure: https://rpgapi3ai2025.azurewebsites.net/Enderecos
        //Somee: http://luizfernando.somee.com/RpgApi/Enderecos


        public EnderecoService()
        {
            _request = new Request();
        }

        public async Task<Endereco> GetEnderecoAsync(string cep)
        {
            var endereco = await _request.GetAsync<Endereco>(_apiUrlBase + cep, string.Empty);

            return endereco;
        }

    }
}
