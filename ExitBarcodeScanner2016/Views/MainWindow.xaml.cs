using ExitBarcodeScanner2016.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ExitBarcodeScanner2016
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainWindowViewModel vm;

		private DispatcherTimer timer;
		public MainWindow()
		{
			InitializeComponent();
			 
			vm = new MainWindowViewModel(this, pageTransitionControl);
			this.DataContext = vm;

			timer = new System.Windows.Threading.DispatcherTimer();
			timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = new TimeSpan(0, 0, 1);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			timer.Stop();
			if (barcode.Contains("\r"))
				barcode = barcode.Replace("\r", "");
			vm.BarcodeScanned(barcode);
			barcode = "";
		}

		private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			String activeUserControl = pageTransitionControl.CurrentPage.ToString();

			if (e.Key == Key.Escape)
			{
				if (activeUserControl.Equals("ExitBarcodeScanner2016.Views.Pages.MainPage"))
				{
					this.Close();
				}
				else
				{
					vm.LoadMainPage();
				}
			}
		}

		string barcode = "";
		private void Window_TextInput(object sender, TextCompositionEventArgs e)
		{
			String activeUserControl = pageTransitionControl.CurrentPage.ToString();
			if (activeUserControl.Equals("ExitBarcodeScanner2016.Views.Pages.MainPage"))
			{
				if (barcode.Length == 0)
					timer.Start();
				barcode += e.Text;
			}
		}



	}
}
