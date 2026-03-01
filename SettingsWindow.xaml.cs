using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CipherLua
{
    public partial class SettingsWindow : Window
    {
        private Color _selectedColor = ThemeManager.AccentColor;

        private readonly (string Name, Color Color)[] _presets = new[]
        {
            ("Neon Green",   Color.FromRgb(0xC8, 0xFF, 0x00)),
            ("Cyan",         Color.FromRgb(0x00, 0xFF, 0xFF)),
            ("Hot Pink",     Color.FromRgb(0xFF, 0x00, 0x88)),
            ("Electric Blue",Color.FromRgb(0x00, 0x88, 0xFF)),
            ("Orange",       Color.FromRgb(0xFF, 0x77, 0x00)),
            ("Purple",       Color.FromRgb(0xAA, 0x00, 0xFF)),
            ("Red",          Color.FromRgb(0xFF, 0x22, 0x22)),
            ("White",        Color.FromRgb(0xFF, 0xFF, 0xFF)),
        };

        public SettingsWindow()
        {
            InitializeComponent();
            BuildSwatches();
            UpdatePreview(_selectedColor);
        }

        private void BuildSwatches()
        {
            foreach (var (name, color) in _presets)
            {
                var swatch = new Border
                {
                    Width = 36, Height = 36,
                    CornerRadius = new CornerRadius(4),
                    Background = new SolidColorBrush(color),
                    Margin = new Thickness(0, 0, 8, 8),
                    Cursor = Cursors.Hand,
                    ToolTip = name,
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Colors.Transparent)
                };
                var c = color;
                swatch.MouseLeftButtonDown += (s, e) =>
                {
                    _selectedColor = c;
                    HexInput.Text = $"#{c.R:X2}{c.G:X2}{c.B:X2}";
                    UpdatePreview(c);
                    // Highlight selected
                    foreach (Border b in ColorSwatches.Children)
                        b.BorderBrush = new SolidColorBrush(Colors.Transparent);
                    swatch.BorderBrush = new SolidColorBrush(Colors.White);
                };
                swatch.Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 8, ShadowDepth = 0,
                    Color = color, Opacity = 0.6
                };
                ColorSwatches.Children.Add(swatch);
            }
        }

        private void UpdatePreview(Color c)
        {
            PreviewText.Foreground = new SolidColorBrush(c);
            PreviewBorder.BorderBrush = new SolidColorBrush(c);
            SaveBtn.Background = new SolidColorBrush(c);
            SaveBtn.Foreground = IsDark(c) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);
            ApplyHexBtn.Background = new SolidColorBrush(c);
            ApplyHexBtn.Foreground = IsDark(c) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);
        }

        private static bool IsDark(Color c) => (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) > 128;

        private void ApplyHex_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hex = HexInput.Text.Trim().TrimStart('#');
                if (hex.Length == 6)
                {
                    var r = Convert.ToByte(hex[..2], 16);
                    var g = Convert.ToByte(hex[2..4], 16);
                    var b = Convert.ToByte(hex[4..6], 16);
                    _selectedColor = Color.FromRgb(r, g, b);
                    UpdatePreview(_selectedColor);
                }
            }
            catch { }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.SetAccent(_selectedColor);
            DialogResult = true;
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}