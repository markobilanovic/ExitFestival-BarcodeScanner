using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExitBarcodeScanner2016.Model
{
	public class Journalist
	{
		public string company;
		public string country;
		public string name;
		public string barcode;
		public string numeration;

		public List<Arrival> arrivals = new List<Arrival>();

		public Arrival lastArrival
		{
			get
			{
				if (arrivals.Count > 0)
					return arrivals.Last();
				else
					return null;
			}
		}
	}
}
