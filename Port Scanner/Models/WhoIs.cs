using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Port_Scanner.Models
{
    public static class WhoIs
    {
        public static async Task<string> LookupAsync(string host, string whoisServer = "whois.iana.org")
        {
            // Creates a new TCpc lient and connects to the WHOIS server on port 43
            using var client = new TcpClient();
            await client.ConnectAsync(whoisServer, 43);

            // Network stream to send and recieve data
            using var stream = client.GetStream();
            // Turn host to bytes as the network stream works with bytes, not strings
            var query = Encoding.ASCII.GetBytes($"{host}\r\n");
            // Sends the query to the server
            await stream.WriteAsync(query);

            // Buffer to hold bytes from the server
            var buffer = new byte[4096];

            // Get the server response
            var response = new StringBuilder();

            int bytesRead;
            // Read the response in 4096 byte chunks
            while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
            {
                // Converts the recieved bytes to ASCII and adds it to the end of the response
                response.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
            }

            return response.ToString();
        }
    }

}
