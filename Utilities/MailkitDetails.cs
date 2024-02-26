using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationPortal.Utilities
{
    public class MailkitDetails
    {
        public string SmtpServer { get; set; }
        public int PortNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool TrustedConnection { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}