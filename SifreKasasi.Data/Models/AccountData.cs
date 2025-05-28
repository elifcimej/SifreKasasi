using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SifreKasasi.Data.Models
{
    public class AccountData
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = null!;
        public string SiteUsername { get; set; } = null!;
        public string EncryptedPassword { get; set; } = null!;

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
    }
}
