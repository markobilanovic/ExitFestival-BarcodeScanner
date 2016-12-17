using ExitBarcodeScanner2016.ViewModels.Pages;
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

namespace ExitBarcodeScanner2016.Views.Pages
{
	/// <summary>
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : UserControl
	{
		public MainPage(MainPageViewModel mainPageVM)
		{
			InitializeComponent();

			this.DataContext = mainPageVM;
		}
	}
}
