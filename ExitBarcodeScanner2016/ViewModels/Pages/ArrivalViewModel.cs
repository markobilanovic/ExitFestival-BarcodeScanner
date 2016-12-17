using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitBarcodeScanner2016.ViewModels.Pages
{
	public class ArrivalViewModel : AbstractViewModel
	{
		private string journalistBarcode;

		private int luggageNumber;
		private string comment;

		private string arrivalTime;
		private string exitTime;




		public string JournalistBarcode
		{
			get
			{
				return journalistBarcode;
			}
			set
			{
				journalistBarcode = value;
			}
		}

		public string ArrivalTime
		{
			get
			{
				return arrivalTime;
			}
			set
			{
				arrivalTime = value;
			}
		}
		public string ExitTime
		{
			get
			{
				return exitTime;
			}
			set
			{
				exitTime = value;
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
	}
}
