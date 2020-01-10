using System;
using System.Collections.Generic;
using System.Text;

namespace AureportLogScanner.Models
{
    class AuthenticationLogsModels
    {
        public string Hostname { get; set; }
        public string Timestamp { get; set; }
        public string User { get; set; }
        public int Entries { get; set; }
    }
}
