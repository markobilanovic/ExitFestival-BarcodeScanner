using ExitBarcodeScanner2016.Common;
using ExitBarcodeScanner2016.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExitBarcodeScanner2016.ViewModels.Pages
{
	public class MainPageViewModel : AbstractViewModel
	{
		private ICommand addUserCommand;
		private MainWindowViewModel mainWindowVM;
		public MainPageViewModel(MainWindowViewModel mainWindowVM)
		{
			this.mainWindowVM = mainWindowVM;
		}

		public ICommand AddUserCommand
		{
			get
			{
				return addUserCommand ?? (addUserCommand = new RelayCommand(param => AddUser(), param => CanAddUserCommandExecute()));
			}
		}

		private void AddUser()
		{
			AddJournalistViewModel addJournalistVM = new AddJournalistViewModel(mainWindowVM);
			AddJournalist addJournalistView = new AddJournalist(addJournalistVM);
			mainWindowVM.ShowPage(addJournalistView);
			mainWindowVM.ActiveWindow = "Add User";
		}

		private bool CanAddUserCommandExecute()
		{
			return true;
		}

	}
}
