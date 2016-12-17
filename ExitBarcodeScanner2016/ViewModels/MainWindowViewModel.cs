using ExitBarcodeScanner2016.Common;
using ExitBarcodeScanner2016.Model;
using ExitBarcodeScanner2016.ViewModels.Pages;
using ExitBarcodeScanner2016.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPageTransitions;

namespace ExitBarcodeScanner2016.ViewModels
{
	public class MainWindowViewModel : AbstractViewModel
	{
		private ICommand homeCommand;
		private ICommand searchCommand;

		private string activeWindowTitle;
		private PageTransition pageTransitionControl;
		private MainPage mainPage;


		private bool isTopMost = true;
		

		
		private Repositorium repo;
		private MainWindow view;
		public MainWindowViewModel(MainWindow view, PageTransition pageTransitionControl)
		{
			this.pageTransitionControl = pageTransitionControl;
			this.view = view;
			MainPageViewModel mainPageVM = new MainPageViewModel(this);
			mainPage = new MainPage(mainPageVM);
			LoadMainPage();

			repo = Repositorium.Instance;
			if(!repo.RepoLoadedCorrectly)
			{
				MessageBox.Show("Excel files not loaded. Please close all excel files and restart the program.");
				view.Close();
			}
		}


		private void Search()
		{
			SearchViewModel searchVM = new SearchViewModel(this);
			SearchView searchView = new SearchView(searchVM);
			ShowPage(searchView);
			this.ActiveWindow = "Search Journalist";
		}

		private void Home()
		{
			LoadMainPage();
			FocusManager.SetFocusedElement(view, view);
		}


		public void BarcodeScanned(string barcode)
		{
			if (repo.Journalists.ContainsKey(barcode))
			{
				Journalist journalist = repo.Journalists[barcode];
				JournalistViewModel journalistVM = new JournalistViewModel(this, journalist);
				JournalistView journalistView = new JournalistView(journalistVM);
				ShowPage(journalistView);
				this.ActiveWindow = "Check In / Check Out";
			}
			else
			{
				if (MessageBox.Show("Unknown barcode. Do you want to add new journalist with this barcode?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
				{
					AddJournalistViewModel addJournalistVM = new AddJournalistViewModel(this, barcode);
					AddJournalist addJournalistView = new AddJournalist(addJournalistVM);
					this.ShowPage(addJournalistView);
					this.ActiveWindow = "Add User";
				}
			}
		}



		

		public void ShowPage(UserControl userControl)
		{
			pageTransitionControl.ShowPage(userControl);
		}

		public void LoadMainPage()
		{
			ShowPage(mainPage);
			this.ActiveWindow = "Waiting for barcode";
		}




		#region Properties 
		public bool IsTopMost
		{
			get
			{
				return isTopMost;
			}
			set
			{
				isTopMost = value;
				OnPropertyChanged("IsTopMost");
			}
		}

		public string ActiveWindow
		{
			get
			{
				return activeWindowTitle;
			}
			set
			{
				activeWindowTitle = value;
				OnPropertyChanged("ActiveWindow");
			}
		}

		public Repositorium Repo
		{
			get
			{
				return repo;
			}
		}

		#endregion


		#region Command junk
		public ICommand HomeCommand
		{
			get
			{
				return homeCommand ?? (homeCommand = new RelayCommand(param => Home(), param => CanHomeCommandExecute()));
			}
		}
		private bool CanHomeCommandExecute()
		{
			return true;
		}

		public ICommand SearchCommand
		{
			get
			{
				return searchCommand ?? (searchCommand = new RelayCommand(param => Search(), param => CanSearchCommandExecute()));
			}
		}
		private bool CanSearchCommandExecute()
		{
			return true;
		}



		#endregion
	}
}
