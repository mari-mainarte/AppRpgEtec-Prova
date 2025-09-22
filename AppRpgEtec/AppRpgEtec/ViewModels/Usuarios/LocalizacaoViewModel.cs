using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Enderecos;
using AppRpgEtec.Services.Usuarios;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;

namespace AppRpgEtec.ViewModels.Usuarios
{
    class LocalizacaoViewModel : BaseViewModel
    {
        private UsuarioService uService;
        private EnderecoService eService;

        public ICommand BuscarEnderecoCommand { get; set; }

        public LocalizacaoViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            eService = new EnderecoService();

            BuscarEnderecoCommand = new Command(async () => await GetEnderecoUsuario());
        }

        private Map meuMapa;

        public Map MeuMapa
        {
            get => meuMapa;
            set
            {
                if (value != null)
                {
                    meuMapa = value;
                    OnPropertyChanged();
                }
            }

     
        }

        private string cep;

        public string Cep
        {
            get => cep;
            set
            {
                if(value != null)
                {
                    cep = value;
                    OnPropertyChanged();
                }
            }
        }

        public async void InicializarMapa()
        {
            try
            {
                Location location = new Location(-23.5200241d, -46.596498d);
                Pin pinEtec = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Etec Horácio",
                    Address = "Rua alcantara, 113, vila Guilherme",
                    Location = location
                };

                Map map = new Map();
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(5));
                map.Pins.Add(pinEtec);
                map.MoveToRegion(mapSpan);

                MeuMapa = map;
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", e.Message, "OK");
            }
        }

        public async void ExibirUsuarioNoMapa()
        {
            try
            {
                ObservableCollection<Usuario> ocUsuarios = await uService.GetUsuariosAsync();
                List<Usuario> listaUsuarios = new List<Usuario>(ocUsuarios);
                Map map = new Map();

                foreach (Usuario u in listaUsuarios)
                {
                    if (u.Latitude != null && u.Longitude != null)
                    {
                        double latitude = (double)u.Latitude;
                        double longitude = (double)u.Longitude;
                        Location location = new Location(latitude, longitude);

                        Pin pinAtual = new Pin()
                        {
                            Type = PinType.Place,
                            Label = u.Username,
                            Address = $"E-mail: {u.Email}",
                            Location = location
                        };
                        map.Pins.Add(pinAtual);
                    }
                }
                MeuMapa = map;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        public async Task GetEnderecoUsuario()
        {
            try
            {
                Endereco endereco = await eService.GetEnderecoAsync(cep);
                Map map = new Map();

                if (endereco.Lat != null && endereco.Lon != null)
                {
                   double latitude = Convert.ToDouble(endereco.Lat);
                   double longitude = Convert.ToDouble(endereco.Lon);

                   Location location = new Location(latitude, longitude);

                   Pin pinEndereco = new Pin()
                   {
                       Type = PinType.Place,
                       Label = endereco.DisplayName,
                       Address = endereco.Name,
                       Location = location
                   };

                   map.Pins.Add(pinEndereco);

                   MeuMapa = map;
                }
            }
            catch (Exception ex) {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

    }
}
