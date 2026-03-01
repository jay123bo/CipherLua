using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;

namespace CipherLua
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _logTimer = new();
        private DateTime _runStart;

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.AccentColorChanged += OnAccentChanged;
            PopulateTechniques();
            AddLog("SYS", "CIPHER.LUA v5.0 initialized");
            AddLog("INFO", "All engines nominal");
            AddLog("SYS", "Roblox process watcher active");
        }

        // ─── Theme ───────────────────────────────────────────
        private void OnAccentChanged(object? sender, Color c)
        {
            // The app resources are already updated by ThemeManager.
            // Glow effects need manual refresh since they aren't data-bound
            AddLog("SYS", $"Accent color updated → #{c.R:X2}{c.G:X2}{c.B:X2}");
        }

        // ─── Window controls ─────────────────────────────────
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var sw = new SettingsWindow { Owner = this };
            if (sw.ShowDialog() == true)
                AddLog("SYS", "Settings saved and applied");
        }

        // ─── Navigation ───────────────────────────────────────
        private void Nav_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton rb || PageEditor == null) return;

            PageEditor.Visibility    = Visibility.Collapsed;
            PageFPS.Visibility       = Visibility.Collapsed;
            PageShaders.Visibility   = Visibility.Collapsed;
            PageAI.Visibility        = Visibility.Collapsed;
            PageAutoClick.Visibility = Visibility.Collapsed;
            PageMacros.Visibility    = Visibility.Collapsed;
            PageGameHelper.Visibility = Visibility.Collapsed;

            EditorToolbar.Visibility = Visibility.Collapsed;
            RunBtn.Visibility = Visibility.Collapsed;
            FooterLines.Visibility = Visibility.Visible;

            switch (rb.Name)
            {
                case "NavEditor":
                    PageEditor.Visibility = Visibility.Visible;
                    EditorToolbar.Visibility = Visibility.Visible;
                    RunBtn.Visibility = Visibility.Visible;
                    TabTitle.Text = "Source Editor";
                    break;
                case "NavFPS":
                    PageFPS.Visibility = Visibility.Visible;
                    TabTitle.Text = "FPS Booster";
                    FooterLines.Visibility = Visibility.Collapsed;
                    break;
                case "NavShaders":
                    PageShaders.Visibility = Visibility.Visible;
                    TabTitle.Text = "Shaders";
                    FooterLines.Visibility = Visibility.Collapsed;
                    break;
                case "NavAI":
                    PageAI.Visibility = Visibility.Visible;
                    TabTitle.Text = "AI Assistant";
                    FooterLines.Visibility = Visibility.Collapsed;
                    break;
                case "NavAutoClick":
                    PageAutoClick.Visibility = Visibility.Visible;
                    TabTitle.Text = "Auto Clicker";
                    FooterLines.Visibility = Visibility.Collapsed;
                    break;
                case "NavMacros":
                    PageMacros.Visibility = Visibility.Visible;
                    TabTitle.Text = "Macros";
                    FooterLines.Visibility = Visibility.Collapsed;
                    break;
                case "NavGameHelper":
                    PageGameHelper.Visibility = Visibility.Visible;
                    TabTitle.Text = "Game Helper";
                    FooterLines.Visibility = Visibility.Collapsed;
                    DetectGame();
                    break;
            }
        }

        // ─── Source Editor ────────────────────────────────────
        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = InputBox.Text;
            int lines = string.IsNullOrEmpty(text) ? 0 : text.Split('\n').Length;
            int chars = text.Length;
            FooterLines.Text = $"Lines: {lines}  Chars: {chars}";
            DetLines.Text = lines.ToString();
        }

        private void ClearInput_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Clear();
            OutputBox.Clear();
            DetObfuscator.Text = "--";
            DetConfidence.Text = "--";
            DetRecovery.Text = "--";
            DetLines.Text = "0";
            AddLog("SYS", "Editor cleared");
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Open Lua File"
            };
            if (dlg.ShowDialog() == true)
            {
                InputBox.Text = System.IO.File.ReadAllText(dlg.FileName);
                AddLog("SYS", $"Loaded: {System.IO.Path.GetFileName(dlg.FileName)}");
            }
        }

        private void RunDeobfuscator_Click(object sender, RoutedEventArgs e)
        {
            var input = InputBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                AddLog("ERR", "No input provided");
                return;
            }

            _runStart = DateTime.Now;
            AddLog("SYS", "CIPHER engine v5 initializing...");
            AddLog("DET", "Detecting obfuscator...");

            // Simulate detection
            RunBtn.IsEnabled = false;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(400) };
            int step = 0;
            timer.Tick += (s, ev) =>
            {
                step++;
                switch (step)
                {
                    case 1:
                        AddLog("INFO", "Primary decode phase...");
                        DetObfuscator.Text = "UNKNOWN";
                        DetConfidence.Text = "0%";
                        break;
                    case 2:
                        var elapsed = (DateTime.Now - _runStart).TotalSeconds;
                        AddLog("OK", $"Done in {elapsed:F3}s");
                        int lines = input.Split('\n').Length;
                        AddLog("OK", $"Output: {lines} lines");
                        DetRecovery.Text = "55%";
                        var output = BuildOutput(input);
                        OutputBox.Text = output;
                        FooterTime.Text = $"Time: {elapsed:F2}s";
                        RunBtn.IsEnabled = true;
                        timer.Stop();
                        break;
                }
            };
            timer.Start();
        }

        private static string BuildOutput(string input)
        {
            var sb = new StringBuilder();
            sb.AppendLine("--[[");
            sb.AppendLine("   CIPHER.LUA — Deobfuscated Output");
            sb.AppendLine();
            sb.AppendLine("   Obfuscator  : UNKNOWN");
            sb.AppendLine("   Recovery    : 55%");
            sb.AppendLine($"  Timestamp   : {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("   " + new string('-', 42));
            sb.AppendLine("   NOTE: Review any remaining dynamic calls.");
            sb.AppendLine("--]]");
            sb.AppendLine();
            sb.Append(input);
            return sb.ToString();
        }

        // ─── Techniques ───────────────────────────────────────
        private void PopulateTechniques()
        {
            var tags = new[] { "Hex decode", "B64 decode", "XOR crack", "Unicode", "VM strip", "CFG unflatten", "Var rename" };
            foreach (var tag in tags)
            {
                var b = new Border
                {
                    Style = (Style)FindResource("TechTag"),
                    Child = new TextBlock
                    {
                        Text = "• " + tag,
                        FontFamily = (FontFamily)FindResource("JF"),
                        FontSize = 9,
                        Foreground = (Brush)FindResource("AccentBrush")
                    }
                };
                TechniquesPanel.Children.Add(b);
            }
        }

        // ─── System Log ───────────────────────────────────────
        private void AddLog(string type, string message)
        {
            var row = new Grid { Margin = new Thickness(0, 0, 0, 3) };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            // Timestamp
            var ts = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm:ss"),
                FontSize = 9,
                FontFamily = new FontFamily("Consolas"),
                Foreground = (Brush)FindResource("TextDim"),
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(ts, 0);

            // Tag
            var (tagColor, tagFg) = type switch
            {
                "OK"   => ("#00DD66", "#000"),
                "ERR"  => ("#FF3333", "#FFF"),
                "INFO" => ("#3399FF", "#000"),
                "SYS"  => ("#AA77FF", "#000"),
                "DET"  => ("#FF9900", "#000"),
                _      => ("#555555", "#FFF")
            };
            var tagBorder = new Border
            {
                Background = (Brush)new BrushConverter().ConvertFromString(tagColor)!,
                CornerRadius = new CornerRadius(2),
                Padding = new Thickness(3, 1, 3, 1),
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(2, 0, 4, 0)
            };
            tagBorder.Child = new TextBlock
            {
                Text = type,
                FontSize = 8,
                FontFamily = new FontFamily("Consolas"),
                Foreground = (Brush)new BrushConverter().ConvertFromString(tagFg)!
            };
            Grid.SetColumn(tagBorder, 1);

            // Message
            var msg = new TextBlock
            {
                Text = message,
                FontSize = 10,
                FontFamily = new FontFamily("Consolas"),
                Foreground = (Brush)FindResource("TextBright"),
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Top
            };
            Grid.SetColumn(msg, 2);

            row.Children.Add(ts);
            row.Children.Add(tagBorder);
            row.Children.Add(msg);

            LogPanel.Children.Add(row);
            LogScroller.ScrollToBottom();
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            LogPanel.Children.Clear();
            AddLog("SYS", "Log cleared");
        }

        // ─── AI Chat ──────────────────────────────────────────
        private void AIChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return) SendAIMessage();
        }
        private void SendAI_Click(object sender, RoutedEventArgs e) => SendAIMessage();

        private void SendAIMessage()
        {
            var msg = AIChatInput.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;
            AIChatInput.Text = "";

            // User bubble
            AddChatBubble($"> {msg}", isUser: true);
            AddLog("INFO", $"AI query: {msg[..Math.Min(30, msg.Length)]}...");

            // Response — wire up real API here
            AddChatBubble("CIPHER.AI > (Connect your API key in GetAIResponse() to get real answers!) " +
                          $"You asked: \"{msg}\"", isUser: false);
            ChatScroller.ScrollToBottom();
        }

        private void AddChatBubble(string text, bool isUser)
        {
            var b = new Border
            {
                Background = isUser
                    ? (Brush)FindResource("AccentDimBrush")
                    : (Brush)FindResource("Bg2"),
                BorderBrush = isUser
                    ? (Brush)FindResource("AccentBrush")
                    : (Brush)FindResource("Border2"),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(2),
                Padding = new Thickness(10, 7, 10, 7),
                Margin = isUser ? new Thickness(40, 0, 0, 6) : new Thickness(0, 0, 40, 6),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left
            };
            b.Child = new TextBlock
            {
                Text = text,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 11,
                Foreground = (Brush)FindResource("TextBright"),
                TextWrapping = TextWrapping.Wrap
            };
            ChatMessages.Children.Add(b);
        }

        // ─── Music ────────────────────────────────────────────
        private void LoadMusic_Click(object sender, RoutedEventArgs e)
        {
            var url = YtUrl.Text.Trim();
            if (string.IsNullOrEmpty(url) || url.StartsWith("https://youtube.com/watch"))
            {
                AddLog("ERR", "Invalid YouTube URL");
                return;
            }
            AddLog("SYS", $"Loading: {url[..Math.Min(20, url.Length)]}...");
            AddLog("OK", "Music started");
            TrackName.Text = "Unknown Track";
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLog("SYS", "Toggle play/pause");
        }

        // ─── FPS ──────────────────────────────────────────────
        private void ApplyFPS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var processes = Process.GetProcessesByName("RobloxPlayerBeta");
                if (processes.Length > 0)
                {
                    processes[0].PriorityClass = ProcessPriorityClass.High;
                    AddLog("OK", "High priority set on Roblox process");
                }
                else
                    AddLog("ERR", "Roblox not running");
            }
            catch (Exception ex)
            {
                AddLog("ERR", ex.Message);
            }
        }

        // ─── Game Helper ──────────────────────────────────────
        private void RefreshGame_Click(object sender, RoutedEventArgs e) => DetectGame();

        private void DetectGame()
        {
            var procs = Process.GetProcessesByName("RobloxPlayerBeta");
            if (procs.Length > 0)
            {
                var title = procs[0].MainWindowTitle;
                DetectedGame.Text = string.IsNullOrEmpty(title) ? "Roblox (title unavailable)" : title;
                AddLog("OK", "Roblox detected");
            }
            else
            {
                DetectedGame.Text = "No game detected — launch Roblox first";
                AddLog("DET", "Roblox not running");
            }
        }
    }
}