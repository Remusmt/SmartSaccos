﻿namespace SmartSaccos.MemberPortal.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string[] AllowedOrigins { get; set; }
        public string Subject { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string AdminPhoneNo { get; set; }
        public string AdminEmail { get; set; }
    }
}
