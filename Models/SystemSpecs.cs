using System.Runtime.InteropServices;
using Hardware.Info;

namespace CPU_Y.Models;

public class SystemSpecs
{
    // ── CPU ────────────────────────────────────────────────────────
    public string CpuName      { get; init; } = "Unknown";
    public int    CpuCores     { get; init; }
    public int    CpuThreads   { get; init; }
    public double CpuSpeedGhz  { get; init; }

    // ── GPU ────────────────────────────────────────────────────────
    public string GpuName      { get; init; } = "Unknown";
    public ulong  GpuVramMb    { get; init; }

    // ── RAM ────────────────────────────────────────────────────────
    public ulong  RamTotalMb   { get; init; }
    public ulong  RamUsedMb    { get; init; }
    public ulong  RamFreeMb    { get; init; }

    // ── OS ─────────────────────────────────────────────────────────
    public string OsName       { get; init; } = "Unknown";
    public string OsVersion    { get; init; } = "Unknown";

    // ── Factory ────────────────────────────────────────────────────
    public static SystemSpecs Gather()
    {
        var hw = new HardwareInfo();
        hw.RefreshCPUList(includePercentProcessorTime: false);
        hw.RefreshVideoControllerList();
        hw.RefreshMemoryStatus();
        hw.RefreshMemoryList();
        hw.RefreshOperatingSystem();

        // CPU
        var cpu       = hw.CpuList.FirstOrDefault();
        var cpuName   = cpu?.Name?.Trim() ?? "Unknown";
        var cpuCores  = (int)(cpu?.NumberOfCores ?? 0);
        var cpuThread = (int)(cpu?.NumberOfLogicalProcessors ?? 0);
        var cpuSpeed  = Math.Round((cpu?.CurrentClockSpeed ?? 0) / 1000.0, 2);

        // GPU
        var gpu      = hw.VideoControllerList.FirstOrDefault();
        var gpuName  = gpu?.Name?.Trim() ?? "Unknown";
        var gpuVram  = (gpu?.AdapterRAM ?? 0) / 1024 / 1024;

        // RAM
        var totalBytes = hw.MemoryStatus.TotalPhysical;
        var freeBytes  = hw.MemoryStatus.AvailablePhysical;
        var usedBytes  = totalBytes - freeBytes;

        // OS
        var os = hw.OperatingSystem;
        var osName = string.IsNullOrWhiteSpace(os?.Name) 
            ? RuntimeInformation.OSDescription 
            : os.Name.Trim();

        var osVersion = os?.Version?.ToString() 
            ?? Environment.OSVersion.VersionString;

        return new SystemSpecs
        {
            CpuName     = cpuName,
            CpuCores    = cpuCores,
            CpuThreads  = cpuThread,
            CpuSpeedGhz = cpuSpeed,
            GpuName     = gpuName,
            GpuVramMb   = gpuVram,
            RamTotalMb  = totalBytes / 1024 / 1024,
            RamUsedMb   = usedBytes  / 1024 / 1024,
            RamFreeMb   = freeBytes  / 1024 / 1024,
            OsName      = osName,
            OsVersion   = osVersion,
        };
    }
}