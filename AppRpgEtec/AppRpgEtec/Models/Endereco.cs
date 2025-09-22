using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRpgEtec.Models
{
    public class Endereco
    {
        public int PlaceId { get; set; }

        public string? Lat { get; set; }

        public string? Lon { get; set; }

        public string Name { get; set;}

        public string DisplayName { get; set;}
    }
}
