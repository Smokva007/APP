using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PeskovHost.Wpf
{
    public partial class MainWindow : Window
    {
        private const string StartUrl = "https://host.peskov.xyz";
        private readonly string bookmarksPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bookmarks.json");
        private List<string> bookmarks = new();

        public MainWindow()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await WebView.EnsureCoreWebView2Async();
            WebView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;

            LoadBookmarks();
            UpdateBookmarksUI();
        }

        private void CoreWebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            LoadProgress.Value = 0;
            StartProgressUpdater();
        }

        private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            LoadProgress.Value = 1;
            BackButton.IsEnabled = WebView.CanGoBack;
            ForwardButton.IsEnabled = WebView.CanGoForward;
        }

        private async void StartProgressUpdater()
        {
            try
            {
                while (WebView.CoreWebView2 is not null && LoadProgress.Value < 0.95)
                {
                    double prog = await GetEstimatedProgress();
                    LoadProgress.Value = prog;
                    await Task.Delay(100);
                }
            }
            catch { }
        }

        private async Task<double> GetEstimatedProgress()
        {
            try
            {
                // WebView2 doesn't expose estimatedProgress directly; use document readiness as a heuristic
                var result = await WebView.CoreWebView2.ExecuteScriptAsync("document.readyState");
                if (result.Contains("complete")) return 0.99;
                if (result.Contains("interactive")) return 0.6;
            }
            catch { }
            return LoadProgress.Value + 0.05;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => WebView?.CoreWebView2?.GoBack();
        private void ForwardButton_Click(object sender, RoutedEventArgs e) => WebView?.CoreWebView2?.GoForward();
        private void ReloadButton_Click(object sender, RoutedEventArgs e) => WebView?.CoreWebView2?.Reload();
        private void OpenBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = WebView.Source?.ToString() ?? StartUrl;
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }

        private void BookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = WebView.Source?.ToString() ?? StartUrl;
            if (!bookmarks.Contains(uri)) bookmarks.Add(uri);
            SaveBookmarks();
            UpdateBookmarksUI();
        }

        private void BookmarksCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (BookmarksCombo.SelectedItem is string uri)
            {
                WebView.CoreWebView2.Navigate(uri);
            }
        }

        private void LoadBookmarks()
        {
            try
            {
                if (File.Exists(bookmarksPath))
                {
                    var json = File.ReadAllText(bookmarksPath);
                    bookmarks = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                }
            }
            catch { bookmarks = new List<string>(); }
        }

        private void SaveBookmarks()
        {
            try
            {
                var json = JsonSerializer.Serialize(bookmarks);
                File.WriteAllText(bookmarksPath, json);
            }
            catch { }
        }

        private void UpdateBookmarksUI()
        {
            BookmarksCombo.ItemsSource = null;
            BookmarksCombo.ItemsSource = bookmarks;
        }

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            BackButton.IsEnabled = WebView.CanGoBack;
            ForwardButton.IsEnabled = WebView.CanGoForward;
        }
    }
}
