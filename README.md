# Logai

Logai is a Windows optimization and cleanup utility focused on keeping PCs lean,
responsive, and ready for gaming. The project is designed to combine practical OS
maintenance, performance tuning, and security-oriented cleanup tools in one
modern desktop app.

The long-term goal is simple: reduce system clutter, remove unnecessary
background overhead, and expose useful Windows controls so the device can spend
more of its resources on the work or game in front of the user.

> Project status: early development. Logai is not yet intended for production
> use, and performance gains will depend on each device, Windows installation,
> hardware configuration, and running software.

## Goals

- Clean temporary files, caches, logs, and other low-value OS clutter.
- Improve day-to-day Windows responsiveness through safe system maintenance.
- Help reduce background activity that can affect game smoothness and FPS
  stability.
- Provide accessible controls for advanced and hidden Windows settings.
- Include responsible adware and malware cleanup helpers.
- Give users clear visibility into what will be changed before applying actions.

## Current Functionality

### Modern App Shell

- Borderless Windows Forms UI with a custom title bar.
- Sidebar navigation for Overview, Cleaner, Performance, Security, Tools, and
  Info pages.
- Runtime page hosting so the design-time tab container stays hidden while the
  app presents a cleaner single-page view.

### Info View

- Live CPU usage, live CPU operating GHz, RAM usage, GPU usage, GPU clocks,
  temperatures, and top resource processes.
- Detailed system specs covering Windows, motherboard/firmware, CPU, RAM, GPU,
  disks, sensors, and hardware notes.
- GPU and process telemetry use Windows performance counters; temperatures and
  GPU clock sensors use LibreHardwareMonitor when the hardware and drivers expose
  those readings.

## Planned Features

### System Cleanup

- Temporary file cleanup
- Windows cache cleanup
- Recycle Bin cleanup
- Browser cache cleanup
- Log and crash dump cleanup
- Storage usage summaries

### Performance Optimization

- Startup app review
- Background process visibility
- Service and scheduled task analysis
- Power plan optimization
- Game-focused performance presets
- System responsiveness tweaks

### Gaming Tools

- Game mode and GPU-related Windows setting checks
- FPS stability recommendations
- Background load reduction before launching games
- Optional optimization profiles for different use cases

### Security Cleanup

- Adware detection helpers
- Suspicious startup entry review
- Browser hijacker and unwanted extension checks
- Quarantine-oriented cleanup flow
- Malware removal assistance that complements, not replaces, trusted antivirus
  software

### Advanced Windows Controls

- Hidden or hard-to-find Windows settings
- Privacy and telemetry controls
- Visual effect tuning
- Context menu and shell cleanup
- Device efficiency recommendations

## Safety Principles

Logai should be powerful without being reckless. Planned cleanup and optimization
features should follow these principles:

- Explain every change before it is applied.
- Prefer reversible actions where possible.
- Avoid deleting personal files.
- Avoid disabling critical Windows services.
- Keep logs of applied changes.
- Make aggressive tuning optional, never automatic.
- Clearly separate safe cleanup from advanced system modification.

## Tech Stack

- C#
- Windows Forms
- .NET Framework 4.8
- System.Management / WMI
- Windows performance counters
- LibreHardwareMonitorLib
- Visual Studio / MSBuild

## Development

Clone the repository:

```powershell
git clone https://github.com/MrMegambo/Logai.git
cd Logai
```

Build with Visual Studio or MSBuild:

```powershell
MSBuild .\Logai.slnx /t:Build /p:Configuration=Debug
```

The debug executable is generated at:

```text
Logai\bin\Debug\Logai.exe
```

### Runtime Dependencies

Logai targets .NET Framework 4.8 and depends on Windows system assemblies for
WinForms, WMI, drawing, networking, XML, and performance counter access. Those
framework assemblies are provided by the installed .NET Framework runtime.

Logai also requires administrator privileges at startup. The application checks
its elevation state before loading the main form and relaunches itself through
Windows UAC when needed. If elevation is denied or cannot be requested, Logai
shows an error message and exits.

The hardware sensor stack also requires these app-local DLL files to stay in the
same folder as `Logai.exe`; copy or publish the complete output folder, not just
the executable.

| DLL | Minimum checked assembly version | Purpose |
| --- | --- | --- |
| `LibreHardwareMonitorLib.dll` | `0.9.6.0` | Hardware sensors, GPU clocks, temperatures, and related telemetry. |
| `BlackSharp.Core.dll` | `1.0.7.0` | LibreHardwareMonitor support dependency. |
| `DiskInfoToolkit.dll` | `1.1.2.0` | Storage hardware information used by the sensor stack. |
| `HidSharp.dll` | `2.6.4.0` | HID device access used by supported hardware sensors. |
| `RAMSPDToolkit-NDD.dll` | `1.4.2.0` | Memory module/SPD support dependency. |
| `System.Buffers.dll` | `4.0.5.0` | Microsoft runtime support library. |
| `System.Memory.dll` | `4.0.5.0` | Microsoft runtime support library. |
| `System.Numerics.Vectors.dll` | `4.1.6.0` | Microsoft vector runtime support library. |
| `System.Runtime.CompilerServices.Unsafe.dll` | `6.0.3.0` | Microsoft runtime support library. |

At startup, Logai validates those local DLLs before loading the main form. The
check verifies that each required DLL exists, has the expected managed assembly
identity, meets the minimum known-good assembly version, and that Logai is
running as a 64-bit process for the LibreHardwareMonitor runtime. Strong-name
public key tokens are also checked for the signed Microsoft runtime assemblies.

## Roadmap

- Expand the core modern Windows desktop UI beyond placeholder pages.
- Add a cleanup scan with itemized results.
- Add safe cleanup execution with logs.
- Add startup app and service review tools.
- Add game performance profiles.
- Add security cleanup helpers for adware and unwanted software.
- Add restore points, backups, or rollback support before advanced changes.

## Disclaimer

Logai is intended as a maintenance and optimization utility. It does not
guarantee FPS increases, does not replace antivirus software, and should not be
used as the only security tool on a system. Any advanced Windows modifications
should be reviewed carefully before applying them.
