using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSaccos.MemberPortal.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string[] AllowedOrigins { get; set; }
        public string Subject { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
