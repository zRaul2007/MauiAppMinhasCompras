using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}