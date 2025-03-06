using System.Windows;
using PiCalculator.ViewModels;

namespace PiCalculator.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new();
        this.DataContext = mainViewModel;
    }
}