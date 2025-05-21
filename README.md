# Port-Scanner

Port Scanner is built in C# using Avalonia UI for its interface. It pings hosts, performs reverse DNS lookups, scans custom port ranges (e.g., 1-100, 500-800, etc...), detects open/closed ports, detects the service and tries to grab the banner and exports results to Excel, which makes it a flexible and user-friendly network scanning tool.

## How It Works
- Capable of scanning any IP or hostname with custom port ranges with a real-time progress bar and host pinging to verify connectivity along with being able to perform reverse DNS lookups to find the host domain, e.g. 8.8.8.8 equals dns.google.
![Showcase of the port scanner, such as the data grid and input boxes](images/PortScanInProcess.png)

- Capable of performing Who Is queries on the host to identify specific information.
![Showcase of the Who Is query, which requests information about the host](images/PortScanWhoIs.png)

- Capable of exporting scan results including port numbers, status, services, and banners to Excel for easy review.  
![Showcase of the data grid being outputted to an excel spreadsheet, which includes the port, status, service and banner](images/PortScanToExcel.png)


## Coming Soon

- Ability to set the scanning speed to reduce timeouts and minimize detection by the target service.  
- Support for advanced scanning techniques.
- Improved export options, such as JSON, etc...
- Customizable scan profiles to save and reuse scan settings, such as saving the host, port ranges, speed, etc...


## License

This project is open source. Feel free to contribute or report issues!
