using System.Windows;
using CPU_Y.Models;

namespace CPU_Y;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => LoadSpecs();
    }

    private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadSpecs();

    private async void LoadSpecs()
    {
        TxtStatus.Text = "Loading hardware information…";
        BtnRefresh.IsEnabled = false;

        SystemSpecs specs;
        try
        {
            specs = await Task.Run(SystemSpecs.Gather);
        }
        catch (Exception ex)
        {
            TxtStatus.Text = $"Error: {ex.Message}";
            BtnRefresh.IsEnabled = true;
            return;
        }

        // ── CPU ──────────────────────────────────────────────────
        TxtCpuName.Text    = specs.CpuName;
        TxtCpuCores.Text   = specs.CpuCores   > 0 ? specs.CpuCores.ToString()   : "N/A";
        TxtCpuThreads.Text = specs.CpuThreads > 0 ? specs.CpuThreads.ToString() : "N/A";
        TxtCpuSpeed.Text   = specs.CpuSpeedGhz > 0
            ? $"{specs.CpuSpeedGhz:F2} GHz"
            : "N/A";

        // ── GPU ──────────────────────────────────────────────────
        TxtGpuName.Text = specs.GpuName;
        TxtGpuVram.Text = specs.GpuVramMb > 0
            ? FormatMb(specs.GpuVramMb)
            : "N/A";

        // ── RAM ──────────────────────────────────────────────────
        TxtRamTotal.Text = FormatMb(specs.RamTotalMb);
        TxtRamUsed.Text  = FormatMb(specs.RamUsedMb);
        TxtRamFree.Text  = FormatMb(specs.RamFreeMb);

        double pct = specs.RamTotalMb > 0
            ? (double)specs.RamUsedMb / specs.RamTotalMb * 100.0
            : 0;
        PbRam.Value        = pct;
        TxtRamPercent.Text = $"{pct:F1} %";

        // ── OS ───────────────────────────────────────────────────
        TxtOsName.Text    = specs.OsName;
        TxtOsVersion.Text = specs.OsVersion;

        TxtStatus.Text = $"Last refreshed: {DateTime.Now:HH:mm:ss}";
        BtnRefresh.IsEnabled = true;
    }

    // ── Helpers ──────────────────────────────────────────────────────
    private static string FormatMb(ulong mb)
    {
        if (mb >= 1024) return $"{mb / 1024.0:F2} GB  ({mb:N0} MB)";
        return $"{mb} MB";
    }
}
