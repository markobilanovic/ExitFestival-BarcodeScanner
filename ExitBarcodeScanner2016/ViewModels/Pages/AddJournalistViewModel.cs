using ExitBarcodeScanner2016.Common;
using ExitBarcodeScanner2016.Model;
using ExitBarcodeScanner2016.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExitBarcodeScanner2016.ViewModels.Pages
{
	public class AddJournalistViewModel
	{
		private string barcode;
		private string name = "";
		private string company = "";
		private string country = "";

		private ICommand addJournalistCommand;
		private ICommand cancelCommand;
		private ICommand addAndCheckInCommand;
		

		private MainWindowViewModel mainWindowVM;

		public AddJournalistViewModel(MainWindowViewModel mainWindowVM, string barcode = null)
		{
			this.mainWindowVM = mainWindowVM;
			this.barcode = barcode;
		}

		private bool Add()
		{
			if (barcode == null)
			{
				barcode = Guid.NewGuid().ToString();
			}

			Journalist journalist = new Journalist();
			journalist.barcode = barcode;
			journalist.name = name.Trim();
			journalist.company = company.Trim();
			journalist.country = country.Trim();

			return mainWindowVM.Repo.AddJournalist(journalist, true);
		}

		private void AddAndCheckIn()
		{
			if(name.Trim() == "")
			{
				MessageBox.Show("Name is required.");
				return;
			}

			if (!Add())
			{
				MessageBox.Show("Unable to add journalist! Hint: Try with some other name or append a number next to it.");
				return;
			}

			mainWindowVM.BarcodeScanned(barcode);
		}


		private void AddJournalist()
		{
			if (name.Trim() == "")
			{
				MessageBox.Show("Name is required.");
				return;
			}

			if (!Add())
			{
				MessageBox.Show("Unable to add journalist! Hint: Try with some other name or append a number next to it.");
				return;
			}

			mainWindowVM.LoadMainPage();
		}

		private void Cancel()
		{
			mainWindowVM.LoadMainPage();
		}

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}
		public string Company
		{
			get
			{
				return company;
			}
			set
			{
				company = value;
			}
		}
		public string Country
		{
			get
			{
				return country;
			}
			set
			{
				country = value;
			}
		}
		public string Barcode
		{
			get
			{
				return barcode;
			}
			set
			{
				barcode = value;
			}
		}


		public ICommand AddAndCheckInCommand
		{
			get
			{
				return addAndCheckInCommand ?? (addAndCheckInCommand = new RelayCommand(param => AddAndCheckIn(), param => CanAddAndCheckInCommandExecute()));
			}
		}
		private bool CanAddAndCheckInCommandExecute()
		{
			return true;
		}

		public ICommand AddJournalistCommand
		{
			get
			{
				return addJournalistCommand ?? (addJournalistCommand = new RelayCommand(param => AddJournalist(), param => CanAddJournalistCommandExecute()));
			}
		}
		private bool CanAddJournalistCommandExecute()
		{
			return true;
		}

		public ICommand CancelCommand
		{
			get
			{
				return cancelCommand ?? (cancelCommand = new RelayCommand(param => Cancel(), param => CanCancelCommandExecute()));
			}
		}
		private bool CanCancelCommandExecute()
		{
			return true;
		}
		
	}
}
