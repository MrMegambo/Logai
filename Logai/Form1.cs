using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using Microsoft.Win32;

namespace Logai
{
    public partial class Form1 : Form
    {
        private const int CornerRadius = 18;
        private const int CsDropShadow = 0x00020000;
        private const int HitCaption = 0x2;
        private const int WmNclButtonDown = 0xA1;
        private const int SidebarWidth = 180;
        private const string GpuEngineCategoryName = "GPU Engine";
        private const string GpuEngineUtilizationCounterName = "Utilization Percentage";
        private const string GpuProcessMemoryCategoryName = "GPU Process Memory";
        private const string GpuDedicatedMemoryCounterName = "Dedicated Usage";
        private const string CpuInformationCategoryName = "Processor Information";
        private const string CpuActualFrequencyCounterName = "Actual Frequency";
        private const string CpuBaseFrequencyCounterName = "Processor Frequency";
        private const string CpuPerformanceCounterName = "% Processor Performance";

        private static readonly Color ThemeColor = Color.FromArgb(15, 15, 15);
        private static readonly Color SidebarColor = Color.FromArgb(20, 20, 20);
        private static readonly Color SidebarButtonHoverColor = Color.FromArgb(34, 34, 34);
        private static readonly Color SidebarButtonActiveColor = Color.FromArgb(42, 42, 42);
        private static readonly Color TextColor = Color.FromArgb(240, 240, 240);
        private static readonly Color MutedTextColor = Color.FromArgb(155, 155, 155);
        private static readonly Color AccentBlue = Color.FromArgb(78, 156, 255);
        private static readonly Color AccentGreen = Color.FromArgb(67, 209, 139);
        private static readonly Color AccentPurple = Color.FromArgb(163, 115, 255);
        private static readonly Color AccentOrange = Color.FromArgb(255, 178, 77);
        private static readonly Color AccentRed = Color.FromArgb(255, 96, 96);
        private static readonly Color AccentCyan = Color.FromArgb(80, 220, 230);

        private readonly System.Windows.Forms.Timer infoRefreshTimer = new System.Windows.Forms.Timer();
        private readonly CancellationTokenSource infoRefreshCancellation = new CancellationTokenSource();
        private readonly object cpuCountersSync = new object();
        private readonly object hardwareMonitorSync = new object();
        private readonly object gpuEngineCountersSync = new object();
        private readonly object gpuMemoryCountersSync = new object();
        private readonly HardwareUpdateVisitor hardwareUpdateVisitor = new HardwareUpdateVisitor();
        private readonly Dictionary<int, ProcessSample> processSamples = new Dictionary<int, ProcessSample>();
        private readonly Dictionary<string, PerformanceCounter> cpuActualFrequencyCounters = new Dictionary<string, PerformanceCounter>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, PerformanceCounter> gpuEngineCounters = new Dictionary<string, PerformanceCounter>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, PerformanceCounter> gpuMemoryCounters = new Dictionary<string, PerformanceCounter>(StringComparer.OrdinalIgnoreCase);
        private Button activeNavButton;
        private Panel runtimePageHost;
        private TabPage currentRuntimePage;
        private DateTime lastProcessSampleUtc = DateTime.UtcNow;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter cpuActualFrequencyCounter;
        private PerformanceCounter cpuBaseFrequencyCounter;
        private PerformanceCounter cpuPerformanceCounter;
        private Computer hardwareMonitor;
        private string reportedCpuBaseClockGHz;
        private bool hardwareMonitorOpenFailed;
        private bool infoRefreshInProgress;
        private bool specsLoading;
        private bool specsLoaded;

        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalMemoryStatusEx(ref MemoryStatusEx buffer);

        public Form1()
        {
            InitializeComponent();
            ApplyModernWindowStyle();
            ConfigureRuntimePageHost();
            ConfigureInfoView();
            ConfigureSidebar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= CsDropShadow;
                return createParams;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ApplyRoundedCorners();
            LayoutSidebar();
            LayoutPageHost();
            LayoutRuntimePageHost();
            UpdateMaximizeButtonText();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            infoRefreshCancellation.Cancel();
            infoRefreshTimer.Stop();
            infoRefreshTimer.Dispose();
            cpuCounter?.Dispose();
            cpuActualFrequencyCounter?.Dispose();
            cpuBaseFrequencyCounter?.Dispose();
            cpuPerformanceCounter?.Dispose();
            DisposeCpuActualFrequencyCounters();
            DisposeGpuEngineCounters();
            DisposeGpuMemoryCounters();
            CloseHardwareMonitor();
            infoRefreshCancellation.Dispose();
            base.OnFormClosed(e);
        }

        private void ApplyModernWindowStyle()
        {
            BackColor = ThemeColor;
            ForeColor = TextColor;
            FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            StartPosition = FormStartPosition.CenterScreen;
            appTitle.ForeColor = AccentBlue;
            ApplyRoundedCorners();
        }

        private void ConfigureSidebar()
        {
            ApplyNavAccent(overviewButton, AccentBlue);
            ApplyNavAccent(cleanerButton, AccentGreen);
            ApplyNavAccent(performanceButton, AccentPurple);
            ApplyNavAccent(securityButton, AccentRed);
            ApplyNavAccent(toolsButton, AccentOrange);
            ApplyNavAccent(infoButton, AccentCyan);

            SetActiveNavButton(overviewButton);
            LayoutSidebar();
            LayoutPageHost();
            LayoutRuntimePageHost();
            SelectPage(overviewTabPage);
            titleBar.BringToFront();
        }

        private void ConfigureRuntimePageHost()
        {
            if (IsDesignerHosted())
            {
                return;
            }

            runtimePageHost = new Panel
            {
                BackColor = ThemeColor,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };

            Controls.Add(runtimePageHost);
            LayoutRuntimePageHost();
            pageHost.Visible = false;
            runtimePageHost.BringToFront();
            sidebarPanel.BringToFront();
            titleBar.BringToFront();
        }

        private void ConfigureInfoView()
        {
            infoRefreshTimer.Interval = 1000;
            infoRefreshTimer.Tick += infoRefreshTimer_Tick;
            liveCpuClockLabel.AutoEllipsis = true;
            liveGpuLabel.AutoEllipsis = true;
            liveGpuClockLabel.AutoEllipsis = true;
            liveTempLabel.AutoEllipsis = true;

            if (IsDesignerHosted())
            {
                return;
            }

            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
            }
            catch
            {
                cpuCounter = null;
            }

            cpuActualFrequencyCounter = TryCreatePerformanceCounter(CpuInformationCategoryName, CpuActualFrequencyCounterName, "_Total");
            cpuBaseFrequencyCounter = TryCreatePerformanceCounter(CpuInformationCategoryName, CpuBaseFrequencyCounterName, "_Total");
            cpuPerformanceCounter = TryCreatePerformanceCounter(CpuInformationCategoryName, CpuPerformanceCounterName, "_Total");
        }

        private void ApplyNavAccent(Button button, Color accentColor)
        {
            button.Tag = accentColor;
            button.FlatAppearance.MouseDownBackColor = SidebarButtonActiveColor;
            button.FlatAppearance.MouseOverBackColor = GetTintedColor(accentColor);
        }

        private void LayoutSidebar()
        {
            if (sidebarPanel == null || titleBar == null)
            {
                return;
            }

            sidebarPanel.Location = new Point(0, titleBar.Bottom);
            sidebarPanel.Height = Math.Max(0, ClientSize.Height - titleBar.Height);
            sidebarPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        }

        private void LayoutPageHost()
        {
            if (pageHost == null || titleBar == null)
            {
                return;
            }

            pageHost.Location = new Point(SidebarWidth, titleBar.Bottom);
            pageHost.Size = new Size(
                Math.Max(0, ClientSize.Width - SidebarWidth),
                Math.Max(0, ClientSize.Height - titleBar.Height));
            pageHost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void LayoutRuntimePageHost()
        {
            if (runtimePageHost == null || titleBar == null)
            {
                return;
            }

            runtimePageHost.Location = new Point(SidebarWidth, titleBar.Bottom);
            runtimePageHost.Size = new Size(
                Math.Max(0, ClientSize.Width - SidebarWidth),
                Math.Max(0, ClientSize.Height - titleBar.Height));
            runtimePageHost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void navButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            SetActiveNavButton(button);
            SelectPageForButton(button);
        }

        private void SelectPageForButton(Button button)
        {
            TabPage selectedPage = overviewTabPage;

            if (button == overviewButton)
            {
                selectedPage = overviewTabPage;
            }
            else if (button == cleanerButton)
            {
                selectedPage = cleanerTabPage;
            }
            else if (button == performanceButton)
            {
                selectedPage = performanceTabPage;
            }
            else if (button == securityButton)
            {
                selectedPage = securityTabPage;
            }
            else if (button == toolsButton)
            {
                selectedPage = toolsTabPage;
            }
            else if (button == infoButton)
            {
                selectedPage = infoTabPage;
            }

            SelectPage(selectedPage);
            SetInfoVisible(button == infoButton);
        }

        private void SelectPage(TabPage selectedPage)
        {
            if (selectedPage == null)
            {
                return;
            }

            if (IsDesignerHosted() || runtimePageHost == null)
            {
                pageHost.SelectedTab = selectedPage;
                return;
            }

            if (currentRuntimePage == selectedPage)
            {
                return;
            }

            runtimePageHost.SuspendLayout();

            try
            {
                if (currentRuntimePage != null)
                {
                    while (runtimePageHost.Controls.Count > 0)
                    {
                        Control control = runtimePageHost.Controls[0];
                        runtimePageHost.Controls.Remove(control);
                        currentRuntimePage.Controls.Add(control);
                    }
                }

                pageHost.SelectedTab = selectedPage;
                currentRuntimePage = selectedPage;

                while (selectedPage.Controls.Count > 0)
                {
                    Control control = selectedPage.Controls[0];
                    selectedPage.Controls.Remove(control);
                    runtimePageHost.Controls.Add(control);
                }
            }
            finally
            {
                runtimePageHost.ResumeLayout(true);
            }
        }

        private bool IsInfoPageActive()
        {
            return currentRuntimePage == infoTabPage || pageHost.SelectedTab == infoTabPage;
        }

        private bool IsDesignerHosted()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return true;
            }

            if (Site?.DesignMode == true)
            {
                return true;
            }

            string processName = Process.GetCurrentProcess().ProcessName;
            return processName.Equals("devenv", StringComparison.OrdinalIgnoreCase) ||
                processName.Equals("XDesProc", StringComparison.OrdinalIgnoreCase) ||
                processName.Equals("DesignToolsServer", StringComparison.OrdinalIgnoreCase);
        }

        private void SetActiveNavButton(Button button)
        {
            if (activeNavButton != null)
            {
                activeNavButton.BackColor = SidebarColor;
                activeNavButton.ForeColor = MutedTextColor;
            }

            activeNavButton = button;
            Color accentColor = activeNavButton.Tag is Color color ? color : AccentBlue;
            activeNavButton.BackColor = GetTintedColor(accentColor);
            activeNavButton.ForeColor = TextColor;
        }

        private void SetInfoVisible(bool visible)
        {
            if (visible)
            {
                if (!specsLoaded && !specsLoading)
                {
                    BeginLoadSystemSpecs();
                }

                Task ignored = RefreshInfoAsync();
                infoRefreshTimer.Start();
            }
            else
            {
                infoRefreshTimer.Stop();
            }
        }

        private async void infoRefreshTimer_Tick(object sender, EventArgs e)
        {
            await RefreshInfoAsync();
        }

        private async void BeginLoadSystemSpecs()
        {
            specsLoading = true;
            specsTextBox.Text = "Loading detailed system information...";

            try
            {
                string specs = await Task.Run(() => BuildSystemSpecsText());
                if (!IsDisposed)
                {
                    specsTextBox.Text = specs;
                    specsLoaded = true;
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    specsTextBox.Text = "Unable to load system information." + Environment.NewLine + ex.Message;
                }
            }
            finally
            {
                specsLoading = false;
            }
        }

        private async Task RefreshInfoAsync()
        {
            CancellationToken cancellationToken = infoRefreshCancellation.Token;
            if (infoRefreshInProgress || cancellationToken.IsCancellationRequested || IsDisposed || !IsInfoPageActive())
            {
                return;
            }

            infoRefreshInProgress = true;

            try
            {
                InfoSnapshot snapshot = await Task.Run(() => CollectInfoSnapshot(cancellationToken), cancellationToken);
                if (!cancellationToken.IsCancellationRequested && !IsDisposed && IsInfoPageActive())
                {
                    ApplyInfoSnapshot(snapshot);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                TraceDiagnostic("Live info refresh failed", ex);
                if (!cancellationToken.IsCancellationRequested && !IsDisposed && IsInfoPageActive())
                {
                    liveCpuLabel.Text = "CPU: refresh unavailable";
                    liveCpuClockLabel.Text = "CPU Live GHz: refresh unavailable";
                    liveRamLabel.Text = "RAM: refresh unavailable";
                    liveGpuLabel.Text = "GPU: refresh unavailable";
                    liveGpuClockLabel.Text = "GPU Live GHz: refresh unavailable";
                    liveTempLabel.Text = "Temps: refresh unavailable";
                }
            }
            finally
            {
                infoRefreshInProgress = false;
            }
        }

        private InfoSnapshot CollectInfoSnapshot(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GpuUsageSnapshot gpuUsage = GetGpuUsageByProcess(cancellationToken);
            Dictionary<int, long> gpuMemoryByPid = GetGpuMemoryByProcess(cancellationToken);
            HardwareSensorSnapshot hardwareSensors = GetHardwareSensorSnapshot();
            cancellationToken.ThrowIfCancellationRequested();
            CpuClockSnapshot cpuClock = GetCurrentCpuClockSnapshot();

            return new InfoSnapshot(
                GetCpuUsage(),
                FormatCpuClockSummary(cpuClock),
                GetReportedCpuBaseClockGHz(),
                GetMemoryStatus(),
                FormatGpuSummary(gpuUsage, gpuMemoryByPid),
                hardwareSensors.GpuClockSummary,
                hardwareSensors.TemperatureSummary,
                CollectProcessUsages(gpuUsage.UsageByPid, gpuMemoryByPid, cancellationToken));
        }

        private void ApplyInfoSnapshot(InfoSnapshot snapshot)
        {
            liveCpuLabel.Text = "CPU Usage: " + FormatPercent(snapshot.CpuUsagePercent);
            liveCpuClockLabel.Text = snapshot.CpuClockSummary == "--"
                ? "CPU Live GHz: --"
                : "CPU Live GHz: " + snapshot.CpuClockSummary + FormatCpuClockContext(snapshot.CpuBaseClockGHz);
            liveRamLabel.Text = $"RAM: {FormatBytes(snapshot.Memory.UsedBytes)} / {FormatBytes(snapshot.Memory.TotalBytes)} ({snapshot.Memory.MemoryLoadPercent:0}%)";
            liveGpuLabel.Text = "GPU Usage: " + snapshot.GpuSummary;
            liveGpuClockLabel.Text = string.IsNullOrWhiteSpace(snapshot.GpuClockSummary)
                ? "GPU Live GHz: --"
                : "GPU Live GHz: " + snapshot.GpuClockSummary;
            liveTempLabel.Text = "Temps: " + snapshot.TemperatureSummary;

            topProcessesListView.BeginUpdate();
            topProcessesListView.Items.Clear();

            foreach (ProcessUsage usage in snapshot.TopProcesses)
            {
                ListViewItem row = new ListViewItem(usage.Name);
                row.SubItems.Add(usage.Pid.ToString(CultureInfo.InvariantCulture));
                row.SubItems.Add(FormatPercent(usage.CpuPercent));
                row.SubItems.Add(FormatBytes(usage.MemoryBytes));
                row.SubItems.Add(usage.GpuPercent > 0 ? FormatPercent(usage.GpuPercent) : "--");
                row.SubItems.Add(usage.GpuMemoryBytes > 0 ? FormatBytes(usage.GpuMemoryBytes) : "--");
                topProcessesListView.Items.Add(row);
            }

            topProcessesListView.EndUpdate();
        }

        private string FormatCpuClockContext(string cpuBaseClockGHz)
        {
            return string.IsNullOrWhiteSpace(cpuBaseClockGHz) || cpuBaseClockGHz == "--"
                ? string.Empty
                : " (rated " + cpuBaseClockGHz + ")";
        }

        private string BuildSystemSpecsText()
        {
            StringBuilder specs = new StringBuilder();

            AppendInfoSection(specs, "SYSTEM", GetOperatingSystemInfo());
            AppendInfoSection(specs, "MOTHERBOARD / FIRMWARE",
                GetMotherboardInfo(),
                GetFirmwareSecurityInfo(),
                GetVirtualizationInfo());
            AppendInfoSection(specs, "CPU", GetCpuInfo());
            AppendInfoSection(specs, "RAM", GetMemoryInfo());
            AppendInfoSection(specs, "GPU", GetGpuInfo());
            AppendInfoSection(specs, "DISKS", GetStorageInfo());
            AppendInfoSection(specs, "SENSORS", GetTemperatureInfo());
            AppendInfoSection(specs, "NOTES",
                "GPU clocks, GPU process usage, and disk optimization state depend on Windows counters, drivers, and permissions.",
                "Temperatures are read through LibreHardwareMonitor and depend on hardware sensor exposure.",
                "SSD optimization usually means retrim; HDD optimization usually means defrag.");

            return specs.ToString();
        }

        private void AppendInfoSection(StringBuilder builder, string title, params string[] lines)
        {
            builder.AppendLine("============================================================");
            builder.AppendLine(title);
            builder.AppendLine("============================================================");

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                foreach (string childLine in line.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    builder.Append("  ");
                    builder.AppendLine(childLine);
                }
            }

            builder.AppendLine();
        }

        private Color GetTintedColor(Color accentColor)
        {
            return Color.FromArgb(
                Math.Min(60, 18 + accentColor.R / 5),
                Math.Min(60, 18 + accentColor.G / 5),
                Math.Min(70, 18 + accentColor.B / 5));
        }

        private float GetCpuUsage()
        {
            try
            {
                return cpuCounter?.NextValue() ?? 0F;
            }
            catch
            {
                return 0F;
            }
        }

        private PerformanceCounter TryCreatePerformanceCounter(string categoryName, string counterName, string instanceName)
        {
            try
            {
                PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, instanceName, true);
                counter.NextValue();
                return counter;
            }
            catch
            {
                return null;
            }
        }

        private List<ProcessUsage> CollectProcessUsages(Dictionary<int, double> gpuUsageByPid, Dictionary<int, long> gpuMemoryByPid, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.UtcNow;
            double elapsedSeconds = Math.Max(0.1D, (now - lastProcessSampleUtc).TotalSeconds);
            HashSet<int> livePids = new HashSet<int>();
            List<ProcessUsage> usages = new List<ProcessUsage>();

            foreach (Process process in Process.GetProcesses())
            {
                cancellationToken.ThrowIfCancellationRequested();
                using (process)
                {
                    try
                    {
                        int pid = process.Id;
                        TimeSpan cpuTime = process.TotalProcessorTime;
                        long memoryBytes = process.WorkingSet64;
                        livePids.Add(pid);

                        double cpuPercent = 0D;
                        if (processSamples.TryGetValue(pid, out ProcessSample previous))
                        {
                            double cpuDelta = (cpuTime - previous.CpuTime).TotalSeconds;
                            cpuPercent = Math.Max(0D, cpuDelta / elapsedSeconds / Environment.ProcessorCount * 100D);
                        }

                        processSamples[pid] = new ProcessSample(cpuTime);

                        gpuUsageByPid.TryGetValue(pid, out double gpuPercent);
                        gpuMemoryByPid.TryGetValue(pid, out long gpuBytes);

                        usages.Add(new ProcessUsage(
                            process.ProcessName,
                            pid,
                            cpuPercent,
                            memoryBytes,
                            gpuPercent,
                            gpuBytes));
                    }
                    catch
                    {
                    }
                }
            }

            foreach (int pid in processSamples.Keys.ToList())
            {
                if (!livePids.Contains(pid))
                {
                    processSamples.Remove(pid);
                }
            }

            lastProcessSampleUtc = now;

            return usages
                .OrderByDescending(item => Math.Max(item.CpuPercent, item.GpuPercent))
                .ThenByDescending(item => item.GpuPercent)
                .ThenByDescending(item => item.CpuPercent)
                .ThenByDescending(item => item.MemoryBytes)
                .Take(12)
                .ToList();
        }

        private GpuUsageSnapshot GetGpuUsageByProcess(CancellationToken cancellationToken)
        {
            Dictionary<int, double> usageByPid = new Dictionary<int, double>();
            bool countersAvailable = false;
            bool sampledCounters = false;
            int engineCount = 0;

            try
            {
                if (!PerformanceCounterCategory.Exists(GpuEngineCategoryName))
                {
                    return new GpuUsageSnapshot(usageByPid, false, false, 0);
                }

                PerformanceCounterCategory category = new PerformanceCounterCategory(GpuEngineCategoryName);
                string[] instances = category.GetInstanceNames();
                HashSet<string> liveInstances = new HashSet<string>(instances, StringComparer.OrdinalIgnoreCase);

                lock (gpuEngineCountersSync)
                {
                    RemoveStaleGpuEngineCounters(liveInstances);

                    foreach (string instance in instances)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        int pid = ExtractPid(instance);
                        if (pid <= 0)
                        {
                            continue;
                        }

                        countersAvailable = true;
                        engineCount++;

                        if (!gpuEngineCounters.TryGetValue(instance, out PerformanceCounter counter))
                        {
                            try
                            {
                                counter = new PerformanceCounter(GpuEngineCategoryName, GpuEngineUtilizationCounterName, instance, true);
                                gpuEngineCounters[instance] = counter;
                                counter.NextValue();
                            }
                            catch
                            {
                                counter?.Dispose();
                            }

                            continue;
                        }

                        double value;
                        try
                        {
                            value = Math.Max(0D, counter.NextValue());
                            sampledCounters = true;
                        }
                        catch
                        {
                            gpuEngineCounters.Remove(instance);
                            counter.Dispose();
                            continue;
                        }

                        if (value <= 0D)
                        {
                            continue;
                        }

                        usageByPid[pid] = usageByPid.TryGetValue(pid, out double current)
                            ? Math.Min(100D, current + value)
                            : Math.Min(100D, value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                TraceDiagnostic("GPU usage counters failed", ex);
                DisposeGpuEngineCounters();
                return new GpuUsageSnapshot(usageByPid, false, false, 0);
            }

            return new GpuUsageSnapshot(usageByPid, countersAvailable, countersAvailable && !sampledCounters, engineCount);
        }

        private string FormatGpuSummary(GpuUsageSnapshot gpuUsage, Dictionary<int, long> gpuMemoryByPid)
        {
            if (!gpuUsage.CountersAvailable)
            {
                return "counters unavailable";
            }

            long dedicatedBytes = gpuMemoryByPid.Values.Sum();
            string memorySummary = dedicatedBytes > 0
                ? " | dedicated " + FormatBytes(dedicatedBytes)
                : string.Empty;

            if (gpuUsage.IsSampling)
            {
                return "sampling..." + memorySummary;
            }

            double totalUsage = Math.Min(100D, gpuUsage.UsageByPid.Values.Sum());
            return totalUsage.ToString("0.0", CultureInfo.InvariantCulture) + "% active" + memorySummary;
        }

        private void RemoveStaleGpuEngineCounters(HashSet<string> liveInstances)
        {
            foreach (string instance in gpuEngineCounters.Keys.ToList())
            {
                if (liveInstances.Contains(instance))
                {
                    continue;
                }

                PerformanceCounter counter = gpuEngineCounters[instance];
                gpuEngineCounters.Remove(instance);
                counter.Dispose();
            }
        }

        private void DisposeGpuEngineCounters()
        {
            lock (gpuEngineCountersSync)
            {
                foreach (PerformanceCounter counter in gpuEngineCounters.Values)
                {
                    counter.Dispose();
                }

                gpuEngineCounters.Clear();
            }
        }

        private Dictionary<int, long> GetGpuMemoryByProcess(CancellationToken cancellationToken)
        {
            Dictionary<int, long> memoryByPid = new Dictionary<int, long>();

            try
            {
                if (!PerformanceCounterCategory.Exists(GpuProcessMemoryCategoryName))
                {
                    return memoryByPid;
                }

                PerformanceCounterCategory category = new PerformanceCounterCategory(GpuProcessMemoryCategoryName);
                string[] instances = category.GetInstanceNames();
                HashSet<string> liveInstances = new HashSet<string>(instances, StringComparer.OrdinalIgnoreCase);

                lock (gpuMemoryCountersSync)
                {
                    RemoveStaleGpuMemoryCounters(liveInstances);

                    foreach (string instance in instances)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        int pid = ExtractPid(instance);
                        if (pid <= 0)
                        {
                            continue;
                        }

                        if (!gpuMemoryCounters.TryGetValue(instance, out PerformanceCounter counter))
                        {
                            try
                            {
                                counter = new PerformanceCounter(GpuProcessMemoryCategoryName, GpuDedicatedMemoryCounterName, instance, true);
                                gpuMemoryCounters[instance] = counter;
                            }
                            catch
                            {
                                counter?.Dispose();
                                continue;
                            }
                        }

                        long value;
                        try
                        {
                            value = Math.Max(0L, (long)counter.NextValue());
                        }
                        catch
                        {
                            gpuMemoryCounters.Remove(instance);
                            counter.Dispose();
                            continue;
                        }

                        memoryByPid[pid] = memoryByPid.TryGetValue(pid, out long current)
                            ? current + value
                            : value;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                TraceDiagnostic("GPU memory counters failed", ex);
                DisposeGpuMemoryCounters();
            }

            return memoryByPid;
        }

        private void RemoveStaleGpuMemoryCounters(HashSet<string> liveInstances)
        {
            foreach (string instance in gpuMemoryCounters.Keys.ToList())
            {
                if (liveInstances.Contains(instance))
                {
                    continue;
                }

                PerformanceCounter counter = gpuMemoryCounters[instance];
                gpuMemoryCounters.Remove(instance);
                counter.Dispose();
            }
        }

        private void DisposeGpuMemoryCounters()
        {
            lock (gpuMemoryCountersSync)
            {
                foreach (PerformanceCounter counter in gpuMemoryCounters.Values)
                {
                    counter.Dispose();
                }

                gpuMemoryCounters.Clear();
            }
        }

        private int ExtractPid(string performanceCounterInstanceName)
        {
            const string pidPrefix = "pid_";
            int start = performanceCounterInstanceName.IndexOf(pidPrefix, StringComparison.OrdinalIgnoreCase);
            if (start < 0)
            {
                return -1;
            }

            start += pidPrefix.Length;
            int end = performanceCounterInstanceName.IndexOf('_', start);
            string pidText = end > start
                ? performanceCounterInstanceName.Substring(start, end - start)
                : performanceCounterInstanceName.Substring(start);

            return int.TryParse(pidText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int pid)
                ? pid
                : -1;
        }

        private string GetOperatingSystemInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Caption, Version, BuildNumber, OSArchitecture, LastBootUpTime FROM Win32_OperatingSystem"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject os = results.Cast<ManagementObject>().FirstOrDefault();
                    if (os == null)
                    {
                        return Environment.OSVersion.VersionString;
                    }

                    string bootTime = "--";
                    string rawBootTime = Convert.ToString(os["LastBootUpTime"], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(rawBootTime))
                    {
                        bootTime = ManagementDateTimeConverter.ToDateTime(rawBootTime).ToString("g", CultureInfo.CurrentCulture);
                    }

                    return $"{os["Caption"]} {os["OSArchitecture"]} | Build {os["BuildNumber"]} | Version {os["Version"]} | Boot {bootTime}";
                }
            }
            catch
            {
                return Environment.OSVersion.VersionString;
            }
        }

        private string GetCpuInfo()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name, NumberOfCores, NumberOfLogicalProcessors, MaxClockSpeed FROM Win32_Processor"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject cpu = results.Cast<ManagementObject>().FirstOrDefault();
                    if (cpu == null)
                    {
                        return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown CPU";
                    }

                    return $"{cpu["Name"]} | {cpu["NumberOfCores"]} cores / {cpu["NumberOfLogicalProcessors"]} threads | Windows-reported rated/base {MHzToGHz(cpu["MaxClockSpeed"])} GHz | Live actual shown in CPU Live GHz";
                }
            }
            catch
            {
                return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") ?? "Unknown CPU";
            }
        }

        private string GetMemoryInfo()
        {
            try
            {
                List<ulong> capacities = new List<ulong>();
                List<string> speeds = new List<string>();

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Capacity, Speed, ConfiguredClockSpeed FROM Win32_PhysicalMemory"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    foreach (ManagementObject module in results)
                    {
                        capacities.Add(Convert.ToUInt64(module["Capacity"], CultureInfo.InvariantCulture));
                        string configuredSpeed = Convert.ToString(module["ConfiguredClockSpeed"], CultureInfo.InvariantCulture);
                        string speed = !string.IsNullOrWhiteSpace(configuredSpeed)
                            ? configuredSpeed
                            : Convert.ToString(module["Speed"], CultureInfo.InvariantCulture);

                        if (!string.IsNullOrWhiteSpace(speed))
                        {
                            speeds.Add(speed + " MT/s");
                        }
                    }
                }

                ulong totalBytes = capacities.Aggregate(0UL, (current, next) => current + next);
                string speedText = speeds.Count > 0 ? string.Join(", ", speeds.Distinct()) : "Speed unavailable";

                return $"{FormatBytes((long)totalBytes)} installed | {capacities.Count} slot(s) populated | {speedText}";
            }
            catch
            {
                MemoryStatus memory = GetMemoryStatus();
                return $"{FormatBytes(memory.TotalBytes)} installed | Speed unavailable";
            }
        }

        private string GetGpuInfo()
        {
            try
            {
                List<string> gpus = new List<string>();

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name, AdapterRAM, DriverVersion, CurrentRefreshRate, CurrentHorizontalResolution, CurrentVerticalResolution FROM Win32_VideoController"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    foreach (ManagementObject gpu in results)
                    {
                        string adapterRam = gpu["AdapterRAM"] == null
                            ? "VRAM unavailable"
                            : FormatBytes(Convert.ToInt64(gpu["AdapterRAM"], CultureInfo.InvariantCulture));
                        string resolution = gpu["CurrentHorizontalResolution"] == null
                            ? "No active display"
                            : $"{gpu["CurrentHorizontalResolution"]}x{gpu["CurrentVerticalResolution"]}@{gpu["CurrentRefreshRate"]}Hz";

                        gpus.Add($"{gpu["Name"]} | {adapterRam} | Driver {gpu["DriverVersion"]} | {resolution} | Live clocks shown in GPU Live GHz");
                    }
                }

                return gpus.Count > 0 ? string.Join(Environment.NewLine + "  ", gpus) : "No GPU reported by Windows";
            }
            catch
            {
                return "GPU details unavailable";
            }
        }

        private string GetMotherboardInfo()
        {
            try
            {
                string system = "System unavailable";
                string board = "Motherboard unavailable";
                string bios = "BIOS unavailable";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject item = results.Cast<ManagementObject>().FirstOrDefault();
                    if (item != null)
                    {
                        system = $"{item["Manufacturer"]} {item["Model"]}".Trim();
                    }
                }

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Manufacturer, Product, Version, SerialNumber FROM Win32_BaseBoard"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject item = results.Cast<ManagementObject>().FirstOrDefault();
                    if (item != null)
                    {
                        board = $"{item["Manufacturer"]} {item["Product"]} | Version {item["Version"]} | Serial {item["SerialNumber"]}";
                    }
                }

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Manufacturer, SMBIOSBIOSVersion, ReleaseDate FROM Win32_BIOS"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject item = results.Cast<ManagementObject>().FirstOrDefault();
                    if (item != null)
                    {
                        string releaseDate = "--";
                        string rawReleaseDate = Convert.ToString(item["ReleaseDate"], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrWhiteSpace(rawReleaseDate))
                        {
                            releaseDate = ManagementDateTimeConverter.ToDateTime(rawReleaseDate).ToShortDateString();
                        }

                        bios = $"{item["Manufacturer"]} {item["SMBIOSBIOSVersion"]} | Released {releaseDate}";
                    }
                }

                return $"{system} | {board} | BIOS {bios}";
            }
            catch
            {
                return "Motherboard details unavailable";
            }
        }

        private string GetFirmwareSecurityInfo()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State"))
                {
                    object value = key?.GetValue("UEFISecureBootEnabled");
                    if (value == null)
                    {
                        return "Secure Boot: unavailable or unsupported";
                    }

                    return Convert.ToInt32(value, CultureInfo.InvariantCulture) == 1
                        ? "Secure Boot: enabled"
                        : "Secure Boot: disabled";
                }
            }
            catch
            {
                return "Secure Boot: unavailable";
            }
        }

        private string GetVirtualizationInfo()
        {
            try
            {
                bool virtualizationSupported = false;
                bool virtualizationEnabled = false;
                bool slatSupported = false;
                bool hypervisorPresent = false;

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT VirtualizationFirmwareEnabled, VMMonitorModeExtensions, SecondLevelAddressTranslationExtensions FROM Win32_Processor"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject processor = results.Cast<ManagementObject>().FirstOrDefault();
                    if (processor != null)
                    {
                        virtualizationSupported = ToBoolean(processor["VMMonitorModeExtensions"]);
                        virtualizationEnabled = ToBoolean(processor["VirtualizationFirmwareEnabled"]);
                        slatSupported = ToBoolean(processor["SecondLevelAddressTranslationExtensions"]);
                    }
                }

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT HypervisorPresent FROM Win32_ComputerSystem"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject system = results.Cast<ManagementObject>().FirstOrDefault();
                    if (system != null)
                    {
                        hypervisorPresent = ToBoolean(system["HypervisorPresent"]);
                    }
                }

                return $"Virtualization: CPU support {FormatBool(virtualizationSupported)} | Firmware enabled {FormatBool(virtualizationEnabled)} | SLAT {FormatBool(slatSupported)} | Hypervisor active {FormatBool(hypervisorPresent)}";
            }
            catch
            {
                return "Virtualization: unavailable";
            }
        }

        private string GetStorageInfo()
        {
            try
            {
                StringBuilder storage = new StringBuilder();
                List<string> physicalDisks = new List<string>();
                List<string> volumes = new List<string>();

                try
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\Microsoft\Windows\Storage", "SELECT FriendlyName, MediaType, BusType, Size, HealthStatus, OperationalStatus FROM MSFT_PhysicalDisk"))
                    using (ManagementObjectCollection results = searcher.Get())
                    {
                        foreach (ManagementObject disk in results)
                        {
                            long size = disk["Size"] == null ? 0L : Convert.ToInt64(disk["Size"], CultureInfo.InvariantCulture);
                            physicalDisks.Add(
                                $"Name          : {disk["FriendlyName"]}{Environment.NewLine}" +
                                $"Type          : {GetPhysicalDiskMediaType(disk["MediaType"])}{Environment.NewLine}" +
                                $"Bus           : {GetBusType(disk["BusType"])}{Environment.NewLine}" +
                                $"Capacity      : {FormatBytes(size)}{Environment.NewLine}" +
                                $"Health        : {GetStorageHealthStatus(disk["HealthStatus"])}{Environment.NewLine}" +
                                $"Status        : {GetOperationalStatus(disk["OperationalStatus"])}");
                        }
                    }
                }
                catch
                {
                }

                if (physicalDisks.Count == 0)
                {
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Model, MediaType, InterfaceType, Size, Status FROM Win32_DiskDrive"))
                    using (ManagementObjectCollection results = searcher.Get())
                    {
                        foreach (ManagementObject disk in results)
                        {
                            long size = disk["Size"] == null ? 0L : Convert.ToInt64(disk["Size"], CultureInfo.InvariantCulture);
                            string model = Convert.ToString(disk["Model"], CultureInfo.InvariantCulture);
                            string mediaType = InferDiskMediaType(model, Convert.ToString(disk["MediaType"], CultureInfo.InvariantCulture), Convert.ToString(disk["InterfaceType"], CultureInfo.InvariantCulture));
                            physicalDisks.Add(
                                $"Name          : {model}{Environment.NewLine}" +
                                $"Type          : {mediaType}{Environment.NewLine}" +
                                $"Interface     : {disk["InterfaceType"]}{Environment.NewLine}" +
                                $"Capacity      : {FormatBytes(size)}{Environment.NewLine}" +
                                $"Status        : {disk["Status"]}");
                        }
                    }
                }

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT DeviceID, VolumeName, FileSystem, FreeSpace, Size FROM Win32_LogicalDisk WHERE DriveType = 3"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    foreach (ManagementObject drive in results)
                    {
                        string driveId = Convert.ToString(drive["DeviceID"], CultureInfo.InvariantCulture);
                        string fileSystem = Convert.ToString(drive["FileSystem"], CultureInfo.InvariantCulture);
                        long size = drive["Size"] == null ? 0L : Convert.ToInt64(drive["Size"], CultureInfo.InvariantCulture);
                        long free = drive["FreeSpace"] == null ? 0L : Convert.ToInt64(drive["FreeSpace"], CultureInfo.InvariantCulture);
                        long used = Math.Max(0L, size - free);
                        double usedPercent = size > 0L ? used * 100D / size : 0D;
                        string volumeName = string.IsNullOrWhiteSpace(Convert.ToString(drive["VolumeName"], CultureInfo.InvariantCulture))
                            ? "Local Disk"
                            : Convert.ToString(drive["VolumeName"], CultureInfo.InvariantCulture);

                        volumes.Add(
                            $"Volume        : {driveId} {volumeName}{Environment.NewLine}" +
                            $"File system   : {fileSystem}{Environment.NewLine}" +
                            $"Usage         : {FormatBytes(used)} / {FormatBytes(size)} ({usedPercent:0.0}%) used, {FormatBytes(free)} free{Environment.NewLine}" +
                            $"Optimization  : {GetDefragStatus(driveId, fileSystem)}");
                    }
                }

                storage.AppendLine("Physical disks");
                storage.AppendLine(physicalDisks.Count > 0 ? IndentBlock(string.Join(Environment.NewLine + Environment.NewLine, physicalDisks), 2) : "  Unavailable");
                storage.AppendLine();
                storage.AppendLine("Volumes");
                storage.AppendLine(volumes.Count > 0 ? IndentBlock(string.Join(Environment.NewLine + Environment.NewLine, volumes), 2) : "  No fixed volumes reported");

                return storage.ToString().TrimEnd();
            }
            catch
            {
                return "Storage details unavailable";
            }
        }

        private string GetDefragStatus(string driveLetter, string fileSystem)
        {
            if (string.IsNullOrWhiteSpace(driveLetter) || string.IsNullOrWhiteSpace(fileSystem))
            {
                return "analysis unavailable";
            }

            if (!fileSystem.Equals("NTFS", StringComparison.OrdinalIgnoreCase) &&
                !fileSystem.Equals("ReFS", StringComparison.OrdinalIgnoreCase))
            {
                return "not applicable for " + fileSystem;
            }

            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT DriveLetter FROM Win32_Volume WHERE DriveLetter = '{driveLetter}'"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject volume = results.Cast<ManagementObject>().FirstOrDefault();
                    if (volume == null)
                    {
                        return GetDefragStatusFromCommand(driveLetter);
                    }

                    using (volume)
                    {
                        ManagementBaseObject output = volume.InvokeMethod("DefragAnalysis", null, null);
                        uint returnValue = output?["ReturnValue"] == null
                            ? 1U
                            : Convert.ToUInt32(output["ReturnValue"], CultureInfo.InvariantCulture);

                        if (returnValue != 0U)
                        {
                            return GetDefragStatusFromCommand(driveLetter);
                        }

                        ManagementBaseObject analysis = output["DefragAnalysis"] as ManagementBaseObject;
                        if (analysis == null)
                        {
                            return "analysis complete";
                        }

                        bool recommended = ToBoolean(GetWmiProperty(analysis, "DefragRecommended"));
                        object fragmentation = GetWmiProperty(analysis, "TotalPercentFragmentation");
                        string fragmentationText = fragmentation == null
                            ? "fragmentation unavailable"
                            : Convert.ToDouble(fragmentation, CultureInfo.InvariantCulture).ToString("0.0", CultureInfo.InvariantCulture) + "% fragmented";

                        return recommended
                            ? "defrag recommended, " + fragmentationText
                            : "no defrag needed, " + fragmentationText;
                    }
                }
            }
            catch
            {
                return GetDefragStatusFromCommand(driveLetter);
            }
        }

        private string GetDefragStatusFromCommand(string driveLetter)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = "defrag.exe",
                        Arguments = driveLetter + " /A /U",
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    };

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (!process.WaitForExit(15000))
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                        }

                        return "analysis timed out";
                    }

                    string combinedOutput = (output + Environment.NewLine + error).Trim();
                    if (string.IsNullOrWhiteSpace(combinedOutput))
                    {
                        return "analysis unavailable";
                    }

                    return ParseDefragOutput(combinedOutput);
                }
            }
            catch
            {
                return "analysis unavailable";
            }
        }

        private string ParseDefragOutput(string output)
        {
            string normalized = output.ToLowerInvariant();
            string fragmentation = ExtractDefragOutputValue(output, "Total fragmented space");

            if (normalized.Contains("you should defragment") ||
                normalized.Contains("needs optimization") ||
                normalized.Contains("optimization is recommended"))
            {
                return AppendFragmentation("defrag recommended", fragmentation);
            }

            if (normalized.Contains("you do not need to defragment") ||
                normalized.Contains("does not need to be optimized") ||
                normalized.Contains("no optimization is required"))
            {
                return AppendFragmentation("no defrag needed", fragmentation);
            }

            if (normalized.Contains("retrim"))
            {
                return AppendFragmentation("SSD retrim analysis complete", fragmentation);
            }

            if (normalized.Contains("analysis complete") || normalized.Contains("post defragmentation report"))
            {
                return AppendFragmentation("analysis complete", fragmentation);
            }

            string firstUsefulLine = output
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .FirstOrDefault(line => line.Length > 0 && !line.StartsWith("Microsoft", StringComparison.OrdinalIgnoreCase));

            return string.IsNullOrWhiteSpace(firstUsefulLine) ? "analysis unavailable" : firstUsefulLine;
        }

        private string AppendFragmentation(string status, string fragmentation)
        {
            return string.IsNullOrWhiteSpace(fragmentation)
                ? status
                : status + ", " + fragmentation + " fragmented";
        }

        private string ExtractDefragOutputValue(string output, string label)
        {
            foreach (string line in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int index = line.IndexOf(label, StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                {
                    continue;
                }

                int separator = line.IndexOf('=', index);
                if (separator < 0)
                {
                    separator = line.IndexOf(':', index);
                }

                if (separator >= 0 && separator < line.Length - 1)
                {
                    return line.Substring(separator + 1).Trim();
                }
            }

            return string.Empty;
        }

        private string GetPhysicalDiskMediaType(object mediaType)
        {
            if (mediaType == null)
            {
                return "Unknown";
            }

            switch (Convert.ToUInt16(mediaType, CultureInfo.InvariantCulture))
            {
                case 3:
                    return "HDD";
                case 4:
                    return "SSD";
                case 5:
                    return "SCM";
                default:
                    return "Unspecified";
            }
        }

        private string GetStorageHealthStatus(object healthStatus)
        {
            if (healthStatus == null)
            {
                return "Unknown";
            }

            switch (Convert.ToUInt16(healthStatus, CultureInfo.InvariantCulture))
            {
                case 0:
                    return "Healthy";
                case 1:
                    return "Warning";
                case 2:
                    return "Unhealthy";
                case 5:
                    return "Unknown";
                default:
                    return "Status " + Convert.ToString(healthStatus, CultureInfo.InvariantCulture);
            }
        }

        private string GetBusType(object busType)
        {
            if (busType == null)
            {
                return "Unknown";
            }

            switch (Convert.ToUInt16(busType, CultureInfo.InvariantCulture))
            {
                case 1:
                    return "SCSI";
                case 2:
                    return "ATAPI";
                case 3:
                    return "ATA";
                case 4:
                    return "IEEE 1394";
                case 5:
                    return "SSA";
                case 6:
                    return "Fibre Channel";
                case 7:
                    return "USB";
                case 8:
                    return "RAID";
                case 9:
                    return "iSCSI";
                case 10:
                    return "SAS";
                case 11:
                    return "SATA";
                case 12:
                    return "SD";
                case 13:
                    return "MMC";
                case 16:
                    return "File Backed Virtual";
                case 17:
                    return "Storage Spaces";
                case 18:
                    return "NVMe";
                case 19:
                    return "Microsoft Reserved";
                default:
                    return "Bus " + Convert.ToString(busType, CultureInfo.InvariantCulture);
            }
        }

        private string GetOperationalStatus(object status)
        {
            if (status == null)
            {
                return "Unknown";
            }

            if (status is Array values)
            {
                List<string> statuses = new List<string>();
                foreach (object value in values)
                {
                    statuses.Add(GetOperationalStatusValue(value));
                }

                return statuses.Count > 0 ? string.Join(", ", statuses.Distinct()) : "Unknown";
            }

            return GetOperationalStatusValue(status);
        }

        private string GetOperationalStatusValue(object status)
        {
            if (status == null)
            {
                return "Unknown";
            }

            switch (Convert.ToUInt16(status, CultureInfo.InvariantCulture))
            {
                case 0:
                    return "Unknown";
                case 1:
                    return "Other";
                case 2:
                    return "OK";
                case 3:
                    return "Degraded";
                case 4:
                    return "Stressed";
                case 5:
                    return "Predictive Failure";
                case 6:
                    return "Error";
                case 7:
                    return "Non-Recoverable Error";
                case 8:
                    return "Starting";
                case 9:
                    return "Stopping";
                case 10:
                    return "Stopped";
                case 11:
                    return "In Service";
                case 12:
                    return "No Contact";
                case 13:
                    return "Lost Communication";
                case 14:
                    return "Aborted";
                case 15:
                    return "Dormant";
                case 16:
                    return "Supporting Entity in Error";
                case 17:
                    return "Completed";
                case 18:
                    return "Power Mode";
                case 19:
                    return "Relocating";
                default:
                    return "Status " + Convert.ToString(status, CultureInfo.InvariantCulture);
            }
        }

        private string InferDiskMediaType(string model, string mediaType, string interfaceType)
        {
            string combined = (model + " " + mediaType + " " + interfaceType).ToUpperInvariant();

            if (combined.Contains("SSD") || combined.Contains("NVME"))
            {
                return "SSD";
            }

            if (combined.Contains("HDD") || combined.Contains("SCSI") || combined.Contains("IDE"))
            {
                return "HDD";
            }

            return string.IsNullOrWhiteSpace(mediaType) ? "Unknown" : mediaType;
        }

        private object GetWmiProperty(ManagementBaseObject source, string propertyName)
        {
            foreach (PropertyData property in source.Properties)
            {
                if (property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return property.Value;
                }
            }

            return null;
        }

        private bool ToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }

            return value is bool boolValue
                ? boolValue
                : Convert.ToBoolean(value, CultureInfo.InvariantCulture);
        }

        private string FormatBool(bool value)
        {
            return value ? "yes" : "no";
        }

        [Conditional("DEBUG")]
        private void TraceDiagnostic(string message, Exception exception)
        {
            Debug.WriteLine($"{nameof(Form1)}: {message}: {exception}");
        }

        private string GetTemperatureInfo()
        {
            string temperatureSummary = GetHardwareSensorSnapshot().TemperatureSummary;
            return string.IsNullOrWhiteSpace(temperatureSummary)
                ? "Unavailable"
                : temperatureSummary;
        }

        private HardwareSensorSnapshot GetHardwareSensorSnapshot()
        {
            try
            {
                List<ClockReading> clockReadings = new List<ClockReading>();
                List<TemperatureReading> temperatureReadings = new List<TemperatureReading>();

                lock (hardwareMonitorSync)
                {
                    if (!TryEnsureHardwareMonitor())
                    {
                        return new HardwareSensorSnapshot(string.Empty, string.Empty);
                    }

                    hardwareMonitor.Accept(hardwareUpdateVisitor);

                    foreach (IHardware hardware in hardwareMonitor.Hardware)
                    {
                        CollectHardwareClockReadings(hardware, clockReadings);
                        CollectHardwareTemperatures(hardware, temperatureReadings);
                    }
                }

                return new HardwareSensorSnapshot(
                    BuildGpuClockSummary(clockReadings),
                    BuildTemperatureSummary(temperatureReadings));
            }
            catch (Exception ex)
            {
                TraceDiagnostic("Hardware sensor refresh failed", ex);
                return new HardwareSensorSnapshot(string.Empty, string.Empty);
            }
        }

        private bool TryEnsureHardwareMonitor()
        {
            if (hardwareMonitor != null)
            {
                return true;
            }

            if (hardwareMonitorOpenFailed || IsDesignerHosted())
            {
                return false;
            }

            try
            {
                hardwareMonitor = new Computer
                {
                    IsBatteryEnabled = true,
                    IsControllerEnabled = true,
                    IsCpuEnabled = true,
                    IsGpuEnabled = true,
                    IsMemoryEnabled = true,
                    IsMotherboardEnabled = true,
                    IsPowerMonitorEnabled = true,
                    IsPsuEnabled = true,
                    IsStorageEnabled = true
                };

                hardwareMonitor.Open();
                return true;
            }
            catch
            {
                try
                {
                    hardwareMonitor?.Close();
                }
                catch
                {
                }

                hardwareMonitor = null;
                hardwareMonitorOpenFailed = true;
                return false;
            }
        }

        private void CloseHardwareMonitor()
        {
            lock (hardwareMonitorSync)
            {
                if (hardwareMonitor == null)
                {
                    return;
                }

                try
                {
                    hardwareMonitor.Close();
                }
                catch
                {
                }
                finally
                {
                    hardwareMonitor = null;
                }
            }
        }

        private void CollectHardwareClockReadings(IHardware hardware, List<ClockReading> readings)
        {
            if (hardware == null)
            {
                return;
            }

            if (IsGpuHardware(hardware.HardwareType))
            {
                foreach (ISensor sensor in hardware.Sensors ?? Enumerable.Empty<ISensor>())
                {
                    if (sensor.SensorType != SensorType.Clock || !sensor.Value.HasValue)
                    {
                        continue;
                    }

                    double megahertz = sensor.Value.Value;
                    if (megahertz > 0D && megahertz < 50000D)
                    {
                        readings.Add(new ClockReading(hardware.Name, sensor.Name, megahertz));
                    }
                }
            }

            foreach (IHardware childHardware in hardware.SubHardware ?? Enumerable.Empty<IHardware>())
            {
                CollectHardwareClockReadings(childHardware, readings);
            }
        }

        private void CollectHardwareTemperatures(IHardware hardware, List<TemperatureReading> readings)
        {
            if (hardware == null)
            {
                return;
            }

            foreach (ISensor sensor in hardware.Sensors ?? Enumerable.Empty<ISensor>())
            {
                if (sensor.SensorType != SensorType.Temperature || !sensor.Value.HasValue)
                {
                    continue;
                }

                double celsius = sensor.Value.Value;
                if (celsius > 0D && celsius < 150D)
                {
                    readings.Add(new TemperatureReading(
                        GetTemperatureGroupLabel(hardware.HardwareType),
                        hardware.Name,
                        sensor.Name,
                        celsius,
                        GetTemperatureGroupSortOrder(hardware.HardwareType)));
                }
            }

            foreach (IHardware childHardware in hardware.SubHardware ?? Enumerable.Empty<IHardware>())
            {
                CollectHardwareTemperatures(childHardware, readings);
            }
        }

        private string BuildGpuClockSummary(List<ClockReading> readings)
        {
            if (readings.Count == 0)
            {
                return string.Empty;
            }

            List<IGrouping<string, ClockReading>> groups = readings
                .GroupBy(reading => reading.HardwareName ?? string.Empty)
                .OrderBy(group => group.Key)
                .ToList();

            return string.Join(" | ", groups
                .Select(group => FormatGpuClockGroup(group.ToList(), groups.Count > 1 ? CleanTemperatureSensorName(group.Key) : string.Empty))
                .Where(summary => !string.IsNullOrWhiteSpace(summary)));
        }

        private string FormatGpuClockGroup(List<ClockReading> readings, string hardwareName)
        {
            List<string> parts = new List<string>();
            ClockReading coreClock;
            ClockReading memoryClock;

            if (TrySelectClock(readings, out coreClock, "core", "graphics"))
            {
                parts.Add("core " + FormatClockGHz(coreClock.Megahertz));
            }

            if (TrySelectClock(readings, out memoryClock, "memory", "vram"))
            {
                parts.Add("mem " + FormatClockGHz(memoryClock.Megahertz));
            }

            if (parts.Count == 0)
            {
                foreach (ClockReading reading in readings
                    .OrderByDescending(item => item.Megahertz)
                    .Take(2))
                {
                    parts.Add(CleanTemperatureSensorName(reading.SensorName).ToLowerInvariant() + " " + FormatClockGHz(reading.Megahertz));
                }
            }

            if (parts.Count == 0)
            {
                return string.Empty;
            }

            string summary = string.Join(", ", parts);
            return string.IsNullOrWhiteSpace(hardwareName)
                ? summary
                : hardwareName + ": " + summary;
        }

        private bool TrySelectClock(List<ClockReading> readings, out ClockReading selected, params string[] tokens)
        {
            selected = default(ClockReading);

            foreach (ClockReading reading in readings
                .Where(reading => ClockNameContainsAny(reading.SensorName, tokens))
                .OrderByDescending(reading => GetClockSensorPriority(reading.SensorName))
                .ThenByDescending(reading => reading.Megahertz))
            {
                selected = reading;
                return true;
            }

            return false;
        }

        private bool ClockNameContainsAny(string sensorName, params string[] tokens)
        {
            string normalized = (sensorName ?? string.Empty).ToUpperInvariant();
            foreach (string token in tokens)
            {
                if (normalized.Contains(token.ToUpperInvariant()))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetClockSensorPriority(string sensorName)
        {
            string normalized = (sensorName ?? string.Empty).ToUpperInvariant();

            if (normalized.Contains("GPU CORE") || normalized.Contains("GPU MEMORY"))
            {
                return 3;
            }

            if (normalized.Contains("CORE") || normalized.Contains("MEMORY"))
            {
                return 2;
            }

            return 1;
        }

        private bool IsGpuHardware(HardwareType hardwareType)
        {
            return hardwareType == HardwareType.GpuAmd ||
                hardwareType == HardwareType.GpuIntel ||
                hardwareType == HardwareType.GpuNvidia;
        }

        private string BuildTemperatureSummary(List<TemperatureReading> readings)
        {
            if (readings.Count == 0)
            {
                return string.Empty;
            }

            return string.Join(" | ", readings
                .GroupBy(reading => reading.GroupLabel, StringComparer.OrdinalIgnoreCase)
                .OrderBy(group => group.Min(reading => reading.GroupSortOrder))
                .ThenBy(group => group.Key)
                .Select(group => FormatTemperatureGroup(group.Key, group.ToList()))
                .Where(summary => !string.IsNullOrWhiteSpace(summary))
                .Take(6));
        }

        private string FormatTemperatureGroup(string label, List<TemperatureReading> readings)
        {
            if (readings.Count == 0)
            {
                return string.Empty;
            }

            TemperatureReading reading = readings
                .OrderByDescending(item => GetTemperatureSensorPriority(item.SensorName))
                .ThenByDescending(item => item.Celsius)
                .First();

            string sensorName = GetTemperatureDisplayName(reading);
            string temperature = reading.Celsius.ToString("0.0", CultureInfo.InvariantCulture) + " C";
            return string.IsNullOrWhiteSpace(sensorName)
                ? label + ": " + temperature
                : label + ": " + temperature + " (" + sensorName + ")";
        }

        private string GetTemperatureGroupLabel(HardwareType hardwareType)
        {
            switch (hardwareType)
            {
                case HardwareType.Cpu:
                    return "CPU";
                case HardwareType.GpuAmd:
                case HardwareType.GpuIntel:
                case HardwareType.GpuNvidia:
                    return "GPU";
                case HardwareType.Storage:
                    return "Drive";
                case HardwareType.Memory:
                    return "Memory";
                case HardwareType.Motherboard:
                case HardwareType.SuperIO:
                case HardwareType.EmbeddedController:
                    return "Board";
                case HardwareType.Battery:
                    return "Battery";
                case HardwareType.Psu:
                    return "PSU";
                case HardwareType.Cooler:
                    return "Cooler";
                case HardwareType.PowerMonitor:
                    return "Power";
                default:
                    return "Sensor";
            }
        }

        private int GetTemperatureGroupSortOrder(HardwareType hardwareType)
        {
            switch (hardwareType)
            {
                case HardwareType.Cpu:
                    return 0;
                case HardwareType.GpuAmd:
                case HardwareType.GpuIntel:
                case HardwareType.GpuNvidia:
                    return 1;
                case HardwareType.Storage:
                    return 2;
                case HardwareType.Memory:
                    return 3;
                case HardwareType.Motherboard:
                case HardwareType.SuperIO:
                case HardwareType.EmbeddedController:
                    return 4;
                case HardwareType.Battery:
                    return 5;
                case HardwareType.Psu:
                    return 6;
                case HardwareType.Cooler:
                case HardwareType.PowerMonitor:
                    return 7;
                default:
                    return 9;
            }
        }

        private int GetTemperatureSensorPriority(string sensorName)
        {
            string normalized = (sensorName ?? string.Empty).ToUpperInvariant();

            if (normalized.Contains("PACKAGE") ||
                normalized.Contains("TCTL") ||
                normalized.Contains("TDIE") ||
                normalized.Contains("GPU CORE"))
            {
                return 4;
            }

            if (normalized.Contains("CORE") ||
                normalized.Contains("JUNCTION") ||
                normalized.Contains("HOT SPOT"))
            {
                return 3;
            }

            if (normalized.Contains("DIODE") ||
                normalized.Contains("TEMPERATURE"))
            {
                return 2;
            }

            return 1;
        }

        private string CleanTemperatureSensorName(string sensorName)
        {
            if (string.IsNullOrWhiteSpace(sensorName))
            {
                return string.Empty;
            }

            return sensorName.Trim();
        }

        private string GetTemperatureDisplayName(TemperatureReading reading)
        {
            string sensorName = CleanTemperatureSensorName(reading.SensorName);
            string hardwareName = CleanTemperatureSensorName(reading.HardwareName);

            if (string.IsNullOrWhiteSpace(sensorName) ||
                sensorName.Equals("Temperature", StringComparison.OrdinalIgnoreCase) ||
                sensorName.Equals("Temperature 1", StringComparison.OrdinalIgnoreCase))
            {
                return hardwareName;
            }

            if (reading.GroupLabel == "CPU" || reading.GroupLabel == "GPU")
            {
                return sensorName;
            }

            if (!string.IsNullOrWhiteSpace(hardwareName) &&
                !sensorName.StartsWith(hardwareName, StringComparison.OrdinalIgnoreCase))
            {
                return hardwareName + " " + sensorName;
            }

            return sensorName;
        }

        private string GetReportedCpuBaseClockGHz()
        {
            if (!string.IsNullOrWhiteSpace(reportedCpuBaseClockGHz))
            {
                return reportedCpuBaseClockGHz;
            }

            lock (cpuCountersSync)
            {
                double baseFrequency = GetPerformanceCounterValue(cpuBaseFrequencyCounter);
                if (baseFrequency > 0D)
                {
                    reportedCpuBaseClockGHz = FormatNullableClockGHz(baseFrequency);
                    return reportedCpuBaseClockGHz;
                }
            }

            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT MaxClockSpeed FROM Win32_Processor"))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    object maxClock = results.Cast<ManagementObject>().FirstOrDefault()?["MaxClockSpeed"];
                    reportedCpuBaseClockGHz = MHzToGHz(maxClock);
                    return reportedCpuBaseClockGHz;
                }
            }
            catch
            {
                reportedCpuBaseClockGHz = "--";
                return reportedCpuBaseClockGHz;
            }
        }

        private CpuClockSnapshot GetCurrentCpuClockSnapshot()
        {
            lock (cpuCountersSync)
            {
                CpuClockSnapshot perCoreSnapshot = GetPerCoreCpuClockSnapshot();
                if (perCoreSnapshot.IsAvailable)
                {
                    return perCoreSnapshot;
                }

                double actualFrequency = GetPerformanceCounterValue(cpuActualFrequencyCounter);
                if (actualFrequency > 0D)
                {
                    return new CpuClockSnapshot(actualFrequency, actualFrequency, actualFrequency, 1);
                }

                double baseFrequency = GetPerformanceCounterValue(cpuBaseFrequencyCounter);
                double performancePercent = GetPerformanceCounterValue(cpuPerformanceCounter);
                if (baseFrequency > 0D && performancePercent > 0D)
                {
                    double estimatedFrequency = baseFrequency * performancePercent / 100D;
                    return new CpuClockSnapshot(estimatedFrequency, estimatedFrequency, estimatedFrequency, 1);
                }
            }

            return new CpuClockSnapshot(0D, 0D, 0D, 0);
        }

        private CpuClockSnapshot GetPerCoreCpuClockSnapshot()
        {
            try
            {
                if (!PerformanceCounterCategory.Exists(CpuInformationCategoryName))
                {
                    return new CpuClockSnapshot(0D, 0D, 0D, 0);
                }

                PerformanceCounterCategory category = new PerformanceCounterCategory(CpuInformationCategoryName);
                string[] instances = category.GetInstanceNames()
                    .Where(IsLogicalProcessorInstance)
                    .ToArray();
                HashSet<string> liveInstances = new HashSet<string>(instances, StringComparer.OrdinalIgnoreCase);
                List<double> values = new List<double>();

                RemoveStaleCpuActualFrequencyCounters(liveInstances);

                foreach (string instance in instances)
                {
                    if (!cpuActualFrequencyCounters.TryGetValue(instance, out PerformanceCounter counter))
                    {
                        try
                        {
                            counter = new PerformanceCounter(CpuInformationCategoryName, CpuActualFrequencyCounterName, instance, true);
                            cpuActualFrequencyCounters[instance] = counter;
                            counter.NextValue();
                        }
                        catch
                        {
                            counter?.Dispose();
                        }

                        continue;
                    }

                    double value;
                    try
                    {
                        value = Math.Max(0D, counter.NextValue());
                    }
                    catch
                    {
                        cpuActualFrequencyCounters.Remove(instance);
                        counter.Dispose();
                        continue;
                    }

                    if (value > 0D)
                    {
                        values.Add(value);
                    }
                }

                return values.Count == 0
                    ? new CpuClockSnapshot(0D, 0D, 0D, 0)
                    : new CpuClockSnapshot(values.Average(), values.Min(), values.Max(), values.Count);
            }
            catch
            {
                DisposeCpuActualFrequencyCounters();
                return new CpuClockSnapshot(0D, 0D, 0D, 0);
            }
        }

        private bool IsLogicalProcessorInstance(string instanceName)
        {
            if (string.IsNullOrWhiteSpace(instanceName) ||
                instanceName.Equals("_Total", StringComparison.OrdinalIgnoreCase) ||
                instanceName.EndsWith("_Total", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string[] parts = instanceName.Split(',');
            if (parts.Length == 1)
            {
                return int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
            }

            return parts.Length == 2 &&
                int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out _) &&
                int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
        }

        private void RemoveStaleCpuActualFrequencyCounters(HashSet<string> liveInstances)
        {
            foreach (string instance in cpuActualFrequencyCounters.Keys.ToList())
            {
                if (liveInstances.Contains(instance))
                {
                    continue;
                }

                PerformanceCounter counter = cpuActualFrequencyCounters[instance];
                cpuActualFrequencyCounters.Remove(instance);
                counter.Dispose();
            }
        }

        private void DisposeCpuActualFrequencyCounters()
        {
            lock (cpuCountersSync)
            {
                foreach (PerformanceCounter counter in cpuActualFrequencyCounters.Values)
                {
                    counter.Dispose();
                }

                cpuActualFrequencyCounters.Clear();
            }
        }

        private double GetPerformanceCounterValue(PerformanceCounter counter)
        {
            if (counter == null)
            {
                return 0D;
            }

            try
            {
                return Math.Max(0D, counter.NextValue());
            }
            catch
            {
                return 0D;
            }
        }

        private MemoryStatus GetMemoryStatus()
        {
            MemoryStatusEx memoryStatus = new MemoryStatusEx();
            memoryStatus.Length = (uint)Marshal.SizeOf(typeof(MemoryStatusEx));

            if (!GlobalMemoryStatusEx(ref memoryStatus))
            {
                return new MemoryStatus(0L, 0L, 0D);
            }

            long totalBytes = unchecked((long)memoryStatus.TotalPhys);
            long availableBytes = unchecked((long)memoryStatus.AvailPhys);
            return new MemoryStatus(totalBytes, Math.Max(0L, totalBytes - availableBytes), memoryStatus.MemoryLoad);
        }

        private string MHzToGHz(object megahertz)
        {
            if (megahertz == null)
            {
                return "--";
            }

            double mhz = Convert.ToDouble(megahertz, CultureInfo.InvariantCulture);
            return (mhz / 1000D).ToString("0.00", CultureInfo.InvariantCulture);
        }

        private string FormatClockGHz(double megahertz)
        {
            return FormatNullableClockGHz(megahertz) + " GHz";
        }

        private string FormatCpuClockSummary(CpuClockSnapshot snapshot)
        {
            if (!snapshot.IsAvailable)
            {
                return "--";
            }

            string average = FormatCpuClockGHz(snapshot.AverageMHz);

            if (snapshot.LogicalProcessorCount <= 1 || Math.Abs(snapshot.MaximumMHz - snapshot.MinimumMHz) < 25D)
            {
                return average;
            }

            return average + " avg (" + FormatCpuClockGHz(snapshot.MinimumMHz) + "-" + FormatCpuClockGHz(snapshot.MaximumMHz) + ")";
        }

        private string FormatCpuClockGHz(double megahertz)
        {
            return megahertz > 0D
                ? (megahertz / 1000D).ToString("0.000", CultureInfo.InvariantCulture)
                : "--";
        }

        private string FormatNullableClockGHz(double megahertz)
        {
            return megahertz > 0D
                ? (megahertz / 1000D).ToString("0.00", CultureInfo.InvariantCulture)
                : "--";
        }

        private string FormatPercent(double value)
        {
            return value.ToString("0.0", CultureInfo.InvariantCulture) + "%";
        }

        private string FormatBytes(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            double value = bytes;
            int unit = 0;

            while (value >= 1024D && unit < units.Length - 1)
            {
                value /= 1024D;
                unit++;
            }

            return value.ToString(unit == 0 ? "0" : "0.0", CultureInfo.InvariantCulture) + " " + units[unit];
        }

        private string IndentBlock(string text, int spaces)
        {
            string indent = new string(' ', spaces);
            return string.Join(
                Environment.NewLine,
                text.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                    .Select(line => string.IsNullOrEmpty(line) ? line : indent + line));
        }

        private void ApplyRoundedCorners()
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            if (WindowState == FormWindowState.Maximized)
            {
                Region previousRegion = Region;
                Region = null;
                previousRegion?.Dispose();
                return;
            }

            IntPtr regionHandle = CreateRoundRectRgn(
                0,
                0,
                Width + 1,
                Height + 1,
                CornerRadius,
                CornerRadius);

            if (regionHandle == IntPtr.Zero)
            {
                return;
            }

            try
            {
                Region roundedRegion = Region.FromHrgn(regionHandle);
                Region previousRegion = Region;

                Region = roundedRegion;
                previousRegion?.Dispose();
            }
            finally
            {
                DeleteObject(regionHandle);
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void maximizeButton_Click(object sender, EventArgs e)
        {
            WindowState = WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;

            UpdateMaximizeButtonText();
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, WmNclButtonDown, (IntPtr)HitCaption, IntPtr.Zero);
        }

        private void UpdateMaximizeButtonText()
        {
            if (maximizeButton != null)
            {
                maximizeButton.Text = WindowState == FormWindowState.Maximized ? "<>" : "[]";
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MemoryStatusEx
        {
            public uint Length;
            public uint MemoryLoad;
            public ulong TotalPhys;
            public ulong AvailPhys;
            public ulong TotalPageFile;
            public ulong AvailPageFile;
            public ulong TotalVirtual;
            public ulong AvailVirtual;
            public ulong AvailExtendedVirtual;
        }

        private struct TemperatureReading
        {
            public TemperatureReading(string groupLabel, string hardwareName, string sensorName, double celsius, int groupSortOrder)
            {
                GroupLabel = groupLabel;
                HardwareName = hardwareName;
                SensorName = sensorName;
                Celsius = celsius;
                GroupSortOrder = groupSortOrder;
            }

            public string GroupLabel { get; }

            public string HardwareName { get; }

            public string SensorName { get; }

            public double Celsius { get; }

            public int GroupSortOrder { get; }
        }

        private struct ClockReading
        {
            public ClockReading(string hardwareName, string sensorName, double megahertz)
            {
                HardwareName = hardwareName;
                SensorName = sensorName;
                Megahertz = megahertz;
            }

            public string HardwareName { get; }

            public string SensorName { get; }

            public double Megahertz { get; }
        }

        private struct CpuClockSnapshot
        {
            public CpuClockSnapshot(double averageMHz, double minimumMHz, double maximumMHz, int logicalProcessorCount)
            {
                AverageMHz = averageMHz;
                MinimumMHz = minimumMHz;
                MaximumMHz = maximumMHz;
                LogicalProcessorCount = logicalProcessorCount;
            }

            public double AverageMHz { get; }

            public double MinimumMHz { get; }

            public double MaximumMHz { get; }

            public int LogicalProcessorCount { get; }

            public bool IsAvailable => LogicalProcessorCount > 0 && AverageMHz > 0D;
        }

        private struct HardwareSensorSnapshot
        {
            public HardwareSensorSnapshot(string gpuClockSummary, string temperatureSummary)
            {
                GpuClockSummary = gpuClockSummary;
                TemperatureSummary = temperatureSummary;
            }

            public string GpuClockSummary { get; }

            public string TemperatureSummary { get; }
        }

        private struct InfoSnapshot
        {
            public InfoSnapshot(float cpuUsagePercent, string cpuClockSummary, string cpuBaseClockGHz, MemoryStatus memory, string gpuSummary, string gpuClockSummary, string temperatureSummary, List<ProcessUsage> topProcesses)
            {
                CpuUsagePercent = cpuUsagePercent;
                CpuClockSummary = cpuClockSummary;
                CpuBaseClockGHz = string.IsNullOrWhiteSpace(cpuBaseClockGHz) ? "--" : cpuBaseClockGHz;
                Memory = memory;
                GpuSummary = gpuSummary;
                GpuClockSummary = gpuClockSummary;
                TemperatureSummary = temperatureSummary;
                TopProcesses = topProcesses;
            }

            public float CpuUsagePercent { get; }

            public string CpuClockSummary { get; }

            public string CpuBaseClockGHz { get; }

            public MemoryStatus Memory { get; }

            public string GpuSummary { get; }

            public string GpuClockSummary { get; }

            public string TemperatureSummary { get; }

            public List<ProcessUsage> TopProcesses { get; }
        }

        private struct GpuUsageSnapshot
        {
            public GpuUsageSnapshot(Dictionary<int, double> usageByPid, bool countersAvailable, bool isSampling, int engineCount)
            {
                UsageByPid = usageByPid;
                CountersAvailable = countersAvailable;
                IsSampling = isSampling;
                EngineCount = engineCount;
            }

            public Dictionary<int, double> UsageByPid { get; }

            public bool CountersAvailable { get; }

            public bool IsSampling { get; }

            public int EngineCount { get; }
        }

        private struct MemoryStatus
        {
            public MemoryStatus(long totalBytes, long usedBytes, double memoryLoadPercent)
            {
                TotalBytes = totalBytes;
                UsedBytes = usedBytes;
                MemoryLoadPercent = memoryLoadPercent;
            }

            public long TotalBytes { get; }

            public long UsedBytes { get; }

            public double MemoryLoadPercent { get; }
        }

        private struct ProcessSample
        {
            public ProcessSample(TimeSpan cpuTime)
            {
                CpuTime = cpuTime;
            }

            public TimeSpan CpuTime { get; }
        }

        private struct ProcessUsage
        {
            public ProcessUsage(string name, int pid, double cpuPercent, long memoryBytes, double gpuPercent, long gpuMemoryBytes)
            {
                Name = name;
                Pid = pid;
                CpuPercent = cpuPercent;
                MemoryBytes = memoryBytes;
                GpuPercent = gpuPercent;
                GpuMemoryBytes = gpuMemoryBytes;
            }

            public string Name { get; }

            public int Pid { get; }

            public double CpuPercent { get; }

            public long MemoryBytes { get; }

            public double GpuPercent { get; }

            public long GpuMemoryBytes { get; }
        }

        private sealed class HardwareUpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }

            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();

                foreach (IHardware childHardware in hardware.SubHardware)
                {
                    childHardware.Accept(this);
                }
            }

            public void VisitSensor(ISensor sensor)
            {
            }

            public void VisitParameter(IParameter parameter)
            {
            }
        }
    }
}
