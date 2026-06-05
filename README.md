# CPU-Y Desktop — .NET Edition

A Windows desktop port of [CPU-Y-Desktop](https://github.com/enzo-quirici/CPU-Y-Desktop) written in **C# / WPF (.NET 8)**.

## Features

| Tab | Info shown |
|-----|-----------|
| CPU | Model name, base clock, physical cores, logical processors |
| GPU | Adapter name, VRAM |
| RAM | Total / used / free with progress bar |
| OS  | Name + version |

## Requirements

- Windows 10 / 11 (x64)
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) or SDK

## Build

```bash
# restore packages + build
dotnet build CpuY.csproj -c Release

# or run directly
dotnet run --project CpuY.csproj
```

## Publish (single-file exe)

```bash
dotnet publish CpuY.csproj -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -o ./dist
```

## Dependencies

| Package | License |
|---------|---------|
| [Hardware.Info](https://github.com/Jinjinov/Hardware.Info) | MIT |

## License

MIT — same as the original Java project.
