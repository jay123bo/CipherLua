using System;
using System.Windows;
using System.Windows.Media;

namespace CipherLua
{
    public static class ThemeManager
    {
        // Accent color — changeable by user
        public static Color AccentColor { get; private set; } = Color.FromRgb(0xC8, 0xFF, 0x00); // neon yellow-green default

        public static void SetAccent(Color color)
        {
            AccentColor = color;
            var app = Application.Current;
            if (app == null) return;

            app.Resources["AccentBrush"]      = new SolidColorBrush(color);
            app.Resources["AccentDimBrush"]   = new SolidColorBrush(Color.FromArgb(60, color.R, color.G, color.B));
            app.Resources["AccentTextBrush"]  = new SolidColorBrush(color);

            // Also update glow color string for effects
            AccentColorChanged?.Invoke(null, color);
        }

        public static event EventHandler<Color>? AccentColorChanged;
    }
}