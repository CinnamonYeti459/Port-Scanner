using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Port_Scanner.Models
{
    public class PortScanner : INotifyPropertyChanged
    {
        public ObservableCollection<PortScanResult> ScanResults { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public Action<int>? ProgressChanged;

        public void AddResult(int port, bool isOpen, string? banner = null)
        {
            ScanResults.Add(new PortScanResult
            {
                Port = port,
                Status = isOpen ? "Open" : "Closed",
                Service = GuessService(port), // Predicts the service based on the port
                Banner = banner ?? "N/A"
            });
        }

        private string GuessService(int port) => port switch
        {
            20 => "FTP Data",
            21 => "FTP Control",
            22 => "SSH",
            23 => "Telnet",
            25 => "SMTP",
            53 => "DNS",
            67 => "DHCP Server",
            68 => "DHCP Client",
            69 => "TFTP",
            80 => "HTTP",
            110 => "POP3",
            119 => "NNTP",
            123 => "NTP",
            137 => "NetBIOS Name Service",
            138 => "NetBIOS Datagram Service",
            139 => "NetBIOS Session Service",
            143 => "IMAP",
            161 => "SNMP",
            162 => "SNMP Trap",
            179 => "BGP",
            194 => "IRC",
            443 => "HTTPS",
            445 => "Microsoft-DS (SMB)",
            465 => "SMTPS",
            514 => "Syslog",
            587 => "SMTP (Submission)",
            636 => "LDAPS",
            993 => "IMAPS",
            995 => "POP3S",
            1080 => "SOCKS Proxy",
            1433 => "Microsoft SQL Server",
            1521 => "Oracle DB",
            1723 => "PPTP VPN",
            2049 => "NFS",
            3306 => "MySQL",
            3389 => "RDP",
            5432 => "PostgreSQL",
            5900 => "VNC",
            6379 => "Redis",
            8080 => "HTTP Proxy",
            8443 => "HTTPS Alternate",
            8888 => "Alternative HTTP",
            _ => "Unknown"
        };

        public async Task ScanPortsAsync(string host, int startPort, int endPort, int timeout = 200)
        {
            // Clear previous results from the data grid
            ScanResults.Clear();

            // Calculate the total number of ports to scan
            int totalPorts = endPort - startPort + 1;
            // Counter for how many ports scanned so far
            int scanned = 0;

            // Loops through each port from start to end
            for (int port = startPort; port <= endPort; port++)
            {
                // Tries to grab the banner
                string? banner = await TryGrabBannerAsync(host, port, timeout);
                // Determines if the port's open based on if the banner was recieved
                bool isOpen = banner != null;
                // Adds the scan results for this port to the data grid
                AddResult(port, isOpen, banner);
                
                // Increments the number of scanned ports
                scanned++;

                // Calculates the current progress for the progress bar on the GUI
                int progress = (int)((double)scanned / totalPorts * 100);
                // Updates the progress for the listeners
                ProgressChanged?.Invoke(progress);
            }
        }

        private async Task<string?> TryGrabBannerAsync(string host, int port, int timeout)
        {
            try
            {
                // Create a new TCP client for connecting to the host and port
                using var client = new TcpClient();
                // Start connecting to the host and port
                var connectTask = client.ConnectAsync(host, port);
                // Create a timeout task that'll complete after a specified time
                var timeoutTask = Task.Delay(timeout);
                // Waits for when the task is completed or timed out
                var completed = await Task.WhenAny(connectTask, timeoutTask);

                // Returns null if it couldn't connect
                if (completed != connectTask || !client.Connected)
                    return null;

                // Sets the time until data's recieved
                client.ReceiveTimeout = timeout;

                // Get the network stream for sending/recieving data
                using var stream = client.GetStream();
                // Buffer to store incoming data, which is the banner
                byte[] buffer = new byte[1024];

                // Give server time to respond with banner
                await Task.Delay(100);

                // Checks if data's available to read from the stream
                if (stream.DataAvailable)
                {
                    // Read the available data
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    // If bytes were read, convert to ASCII string and remove any whitespace and return it as a banner
                    if (bytesRead > 0)
                        return Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                }

                // If no data was recieved, but the connection was successful, return an empty string
                return "";
            }
            catch // Return null if an error occurs
            {
                return null;
            }
        }
    }
}