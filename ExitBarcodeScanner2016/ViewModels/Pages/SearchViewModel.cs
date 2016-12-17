using ExitBarcodeScanner2016.Common;
using ExitBarcodeScanner2016.Model;
using ExitBarcodeScanner2016.Views.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace ExitBarcodeScanner2016.ViewModels.Pages
{
	public class SearchViewModel : AbstractViewModel
	{
		private ObservableCollection<JournalistViewModel> journalists;
		private JournalistViewModel selectedJournalist;

		private ICommand checkInOutCommand;
		private string searchValue = "";

		private ICollectionView itemlist;
		private MainWindowViewModel mainWindowVM;

		public SearchViewModel(MainWindowViewModel mainWindowVM)
		{
			this.mainWindowVM = mainWindowVM;
			journalists = new ObservableCollection<JournalistViewModel>();

			foreach(Journalist j in Repositorium.Instance.Journalists.Values)
			{
				JournalistViewModel journalistVM = new JournalistViewModel(mainWindowVM, j);
				journalists.Add(journalistVM);
			}

			var _itemSourceList = new CollectionViewSource() { Source = journalists };

			itemlist = _itemSourceList.View;
			itemlist.Filter = new Predicate<object>(Filter);
		}

		private void CheckInOut()
		{
			JournalistView journalistView = new JournalistView(SelectedJournalist);
			mainWindowVM.ShowPage(journalistView);
			mainWindowVM.ActiveWindow = "Check In / Check Out";
		}

		public bool Filter(object obj)
		{
			var data = obj as JournalistViewModel;
			if (data != null)
			{
				if (!string.IsNullOrEmpty(SearchValue))
				{
					return Contains(data.Name, SearchValue, StringComparison.OrdinalIgnoreCase) ||
						Contains(data.Company, SearchValue, StringComparison.OrdinalIgnoreCase) ||
						Contains(data.Country, SearchValue, StringComparison.OrdinalIgnoreCase);
				}
				return true;
			}
			return false;
		}


		private void FilterCollection()
		{
			if (itemlist != null)
			{
				itemlist.Refresh();
			}
		}

		public bool Contains(string source, string toCheck, StringComparison comp)
		{
			return source != null && toCheck != null && source.IndexOf(toCheck, comp) >= 0;
		}


		#region Properties
		public JournalistViewModel SelectedJournalist
		{
			get
			{
				return selectedJournalist;
			}
			set
			{
				selectedJournalist = value;
			}
		}
		public string SearchValue
		{
			get
			{
				return searchValue;
			}
			set
			{
				searchValue = value;
				FilterCollection();
			}
		}
		public ObservableCollection<JournalistViewModel> Journalists
		{
			get
			{
				return journalists;
			}
		}
		public ICollectionView Itemlist
		{
			get
			{
				return itemlist;
			}
		}

		#endregion

		public ICommand CheckInOutCommand
		{
			get
			{
				return checkInOutCommand ?? (checkInOutCommand = new RelayCommand(param => CheckInOut(), param => CanCheckInOutCommandExecute()));
			}
		}
		private bool CanCheckInOutCommandExecute()
		{
			if (SelectedJournalist != null)
				return true;
			else
				return false;
		}
	}
}
