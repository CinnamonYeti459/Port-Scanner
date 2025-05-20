namespace Port_Scanner.Models
{
    public class PortScanResult
    {
        public int Port { get; set; }
        public string Status { get; set; }  // "Open", "Closed", etc
        public string Service { get; set; } // HTTP, FTP, etc
        public string Banner { get; set; } = "N/A";
    }
}
