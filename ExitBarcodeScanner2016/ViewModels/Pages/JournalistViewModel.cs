using ExitBarcodeScanner2016.Common;
using ExitBarcodeScanner2016.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExitBarcodeScanner2016.ViewModels.Pages
{
	public class JournalistViewModel : AbstractViewModel
	{
		private MainWindowViewModel mainWindowVM;

		private string barcode;
		private string numeration;

		private string name;
		private string company;
		private string country;

		private ObservableCollection<ArrivalViewModel> arrivals;
		private ArrivalViewModel lastArrival;

		private string comment;
		private int luggageNumber;

		private ICommand checkInCommand;
		private ICommand checkOutCommand;
		private ICommand updateCommentCommand;

		private Journalist journalist;

		public JournalistViewModel(MainWindowViewModel mainWindowVM, Journalist journalist)
		{
			this.mainWindowVM = mainWindowVM;
			this.journalist = journalist;
			arrivals = new ObservableCollection<ArrivalViewModel>();

			barcode = journalist.barcode;
			numeration = journalist.numeration;
			name = journalist.name;
			company = journalist.company;
			country = journalist.country;

			if(journalist.lastArrival != null && journalist.lastArrival.status == "Check In")
			{
				comment = journalist.lastArrival.comment;
				luggageNumber = Int32.Parse(journalist.lastArrival.luggageNumber);
			}
			else
			{
				luggageNumber = Repositorium.Instance.GetNextNumber();
			}

			for(int i = 0; i < journalist.arrivals.Count; i++)
			{
				Arrival arrival = journalist.arrivals[i];
				if (arrival.status == "Check In")
				{
					ArrivalViewModel arrivalVM = new ArrivalViewModel();
					arrivalVM.ArrivalTime = arrival.datetime;
					arrivalVM.Comment = arrival.comment;
					arrivalVM.JournalistBarcode = arrival.barcode;
					arrivalVM.LuggageNumber = Int32.Parse(arrival.luggageNumber);
					arrivals.Add(arrivalVM);
					lastArrival = arrivalVM;
				}
				else
				{
					ArrivalViewModel arrivalVM  = arrivals.Last();
					arrivalVM.ExitTime = arrival.datetime;
					arrivalVM.Comment = arrival.comment;
					lastArrival = null;
				}
			}


		}

		private Arrival CreateNewArrival()
		{
			Arrival arrival = new Arrival();
			arrival.luggageNumber = luggageNumber.ToString();
			arrival.barcode = barcode;
			arrival.datetime = DateTime.Now.ToString();
			arrival.comment = comment;
			arrival.company = company;
			arrival.country = country;
			arrival.name = name;

			return arrival;
		}

		private void CheckIn()
		{
			Arrival arrival = CreateNewArrival();
			arrival.status = "Check In";

			if(Repositorium.Instance.NewArrival(arrival, true))
				mainWindowVM.LoadMainPage();
		}

		private void CheckOut()
		{
			Arrival arrival = CreateNewArrival();
			arrival.status = "Check Out";

			if (Repositorium.Instance.NewArrival(arrival, true))
				mainWindowVM.LoadMainPage();
		}


		private string oldComment;
		private void UpdateComment()
		{
			oldComment = lastArrival.Comment;
			lastArrival.Comment = comment;
			journalist.lastArrival.comment = comment;
			if(!Repositorium.Instance.SaveArrivalsToExcel())
			{
				journalist.lastArrival.comment = oldComment;
				lastArrival.Comment = oldComment;
				comment = oldComment;
			}

			MessageBox.Show("Comment updated!");
		}

		public bool IsCheckedIn
		{
			get
			{
				if (lastArrival == null)
					return false;
				else
					return true;
			}
		}

		public bool IsCheckedOut
		{
			get
			{
				if (lastArrival == null)
					return true;
				else
					return false;
			}
		}



		public int LuggageNumber
		{
			get
			{
				return luggageNumber;
			}
			set
			{
				luggageNumber = value;
			}
		}

		public string Comment
		{
			get
			{
				return comment;
			}
			set
			{
				comment = value;
			}
		}

		public ObservableCollection<ArrivalViewModel> Arrivals
		{
			get
			{
				return arrivals;
			}
			set
			{
				arrivals = value;
			}
		}

		public ArrivalViewModel LastArrival
		{
			get
			{
				return lastArrival;
			}
			set
			{
				lastArrival = value;
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

		public string Numeration
		{
			get
			{
				return numeration;
			}
			set
			{
				numeration = value;
			}
		}


		public ICommand CheckInCommand
		{
			get
			{
				return checkInCommand ?? (checkInCommand = new RelayCommand(param => CheckIn(), param => CanCheckInCommandExecute()));
			}
		}
		private bool CanCheckInCommandExecute()
		{
			return true;
		}

		public ICommand CheckOutCommand
		{
			get
			{
				return checkOutCommand ?? (checkOutCommand = new RelayCommand(param => CheckOut(), param => CanCheckOutCommandExecute()));
			}
		}
		private bool CanCheckOutCommandExecute()
		{
			return true;
		}

		public ICommand UpdateCommentCommand
		{
			get
			{
				return updateCommentCommand ?? (updateCommentCommand = new RelayCommand(param => UpdateComment(), param => CanUpdateCommentCommandExecute()));
			}
		}

		private bool CanUpdateCommentCommandExecute()
		{
			return true;
		}
	}
}
