using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Port_Scanner.Models;
using Port_Scanner.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace Port_Scanner.Views;

public partial class MainView : UserControl
{
    private readonly MainViewModel mainViewModel;

    public MainView()
    {
        InitializeComponent();
        mainViewModel = new MainViewModel();
        DataContext = mainViewModel;
    }

    private async void PortScanButton_Click(object sender, RoutedEventArgs e)
    {
        await mainViewModel.StartScanAsync(HostBox.Text, int.Parse(PortScanBoxStart.Text), int.Parse(PortScanBoxEnd.Text));
    }

    private async void PingButton_Click(object sender, RoutedEventArgs e)
    {
        using var ping = new Ping();
        PingReply reply;
        try
        {
            reply = await ping.SendPingAsync(HostBox.Text, 2000);
            if (reply.Status == IPStatus.Success)
            {
                PingMessage.Text = $"Ping successful ({reply.RoundtripTime} ms)";
            }
            else
            {
                PingMessage.Text = $"Ping failed: {reply.Status}";
            }
        }
        catch (Exception ex)
        {
            reply = null;
            PingMessage.Text = $"Ping error: {ex.Message}";
        }
    }

    private async void ReverseDNSButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            IPAddress ip = IPAddress.Parse(HostBox.Text); // Parses string to an IP address
            IPHostEntry entry = await Dns.GetHostEntryAsync(ip); // Performs the reverse DNS lookup
            ReverseDNSMessage.Text = entry.HostName;
        }
        catch (Exception ex)
        {
            ReverseDNSMessage.Text = $"Error: {ex.Message}";
        }
    }

    private async void ClearReverseDNSButton_Click(object sender, RoutedEventArgs e)
    {
        ReverseDNSMessage.Text = "";
    }

    private async void WhoIsButton_Click(object sender, RoutedEventArgs e)
    {
        string whoIsInfo = await WhoIs.LookupAsync(HostBox.Text);
        if (whoIsInfo != null)
        {
            WhoIsMessage.Text = whoIsInfo;
        }
        else
        {
            WhoIsMessage.Text = "Error: Failed to fetch Who Is";
        }
    }

    private async void ClearWhoIsButton_Click(object sender, RoutedEventArgs e)
    {
        WhoIsMessage.Text = "";
    }

    private async void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
    {
        var exporter = new ExportToExcel();
        exporter.ExportDataGridToExcel(PortDataGrid);
    }

    private void GitHubLink_Click(object sender, PointerPressedEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "https://github.com/CinnamonYeti459",
            UseShellExecute = true
        });
    }
}
