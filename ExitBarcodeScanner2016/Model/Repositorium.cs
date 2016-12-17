using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace ExitBarcodeScanner2016.Model
{
	public class Repositorium
	{
		private string journalistFilePath = Environment.CurrentDirectory + "\\journalists.xlsx";
		private string arrivalsFilePath = Environment.CurrentDirectory + "\\arrivals.xlsx";

		private DataTable journalistsDataTable;
		private DataTable arrivalsDataTable;

		private Dictionary<string, Journalist> journalists = new Dictionary<string, Journalist>();
		private List<Arrival> arrivals = new List<Arrival>();

		private int LuggageCounter = 0;


		static readonly Repositorium _instance = new Repositorium();
		public static Repositorium Instance
		{
			get
			{
				return _instance;
			}
		}

		public bool RepoLoadedCorrectly = false;

		private Repositorium()
		{
			journalistsDataTable = GetDataTableFromExcel(journalistFilePath);
			arrivalsDataTable = GetDataTableFromExcel(arrivalsFilePath);

			if (journalistsDataTable != null && arrivalsDataTable != null)
			{
				FillJournalistDictionary();
				AssignArrivalsToJournalist();
				RepoLoadedCorrectly = true;
			}
		}


		public bool AddJournalist(Journalist journalist, bool save = false)
		{
			if (!string.IsNullOrEmpty(journalist.barcode) && !journalists.ContainsKey(journalist.barcode))
			{
				try
				{
					if (save)
					{
						FileStream stream = File.Open(journalistFilePath, FileMode.Open);
						stream.Close();
						journalists.Add(journalist.barcode, journalist);
						SaveJournalistsToExcel();
					}
					else
					{
						journalists.Add(journalist.barcode, journalist);
					}
					return true;
				}
				catch(Exception e)
				{
					MessageBox.Show("Action not completed. Please close excel document while the program is running.");
					return false;
				}
			}
			return false;
		}


		public bool NewArrival(Arrival newArrival, bool save = false)
		{
			try
			{
				if (save)
				{
					FileStream stream = File.Open(arrivalsFilePath, FileMode.Open);
					stream.Close();
					
					AddNewArrival(newArrival);
					SaveArrivalsToExcel();
				}
				else
				{
					AddNewArrival(newArrival);
				}
				return true;
			}
			catch(Exception e)
			{
				MessageBox.Show("Action not completed. Please close excel document while the program is running.");
				return false;
			}
		}

		private void AddNewArrival(Arrival newArrival)
		{
			Journalist journalist = journalists[newArrival.barcode];
			journalist.arrivals.Add(newArrival);
			arrivals.Add(newArrival);

			if (newArrival.status == "Check In")
			{
				LuggageCounter++;
			}
		}


		public int GetNextNumber()
		{
			return LuggageCounter + 1;
		}

		private void FillJournalistDictionary()
		{
			foreach (DataRow row in journalistsDataTable.Rows)
			{
				Journalist j = new Journalist();
				j.company = row.ItemArray[0].ToString();
				j.country = row.ItemArray[1].ToString();
				j.name = row.ItemArray[2].ToString();
				j.barcode = row.ItemArray[3].ToString();
				j.numeration = row.ItemArray[4].ToString();

				AddJournalist(j);
			}
		}

		private void AssignArrivalsToJournalist()
		{
			string journalistBarcode;
			foreach (DataRow row in arrivalsDataTable.Rows)
			{
				journalistBarcode = row.ItemArray[4].ToString();
				string arrivalStatus = row.ItemArray[1].ToString();

				if (journalists.ContainsKey(journalistBarcode))
				{
					Journalist journalist = journalists[journalistBarcode];
					Arrival newArrival = new Arrival();
					newArrival.barcode = journalist.barcode;
					newArrival.company = journalist.company;
					newArrival.country = journalist.country;
					newArrival.name = journalist.name;
					newArrival.luggageNumber = row.ItemArray[0].ToString();
					newArrival.status = row.ItemArray[1].ToString();
					newArrival.datetime = row.ItemArray[2].ToString();
					newArrival.comment = row.ItemArray[3].ToString();

					NewArrival(newArrival);
				}
			}
		}


		public bool SaveArrivalsToExcel()
		{
			try
			{
				DataTable excelDataTable = new DataTable();
				excelDataTable.Columns.Add("Luggage Number");
				excelDataTable.Columns.Add("Check In/Out");
				excelDataTable.Columns.Add("Date time");
				excelDataTable.Columns.Add("Comment");
				excelDataTable.Columns.Add("Barcode");
				excelDataTable.Columns.Add("Name");
				excelDataTable.Columns.Add("Company");
				excelDataTable.Columns.Add("Country");


				foreach (Arrival arrival in arrivals)
				{
					excelDataTable.Rows.Add(arrival.luggageNumber, arrival.status, arrival.datetime, arrival.comment,
						arrival.barcode, arrival.name, arrival.company, arrival.country);
				}

				if (File.Exists(arrivalsFilePath))
					File.Delete(arrivalsFilePath);

				var newFile = new FileInfo(arrivalsFilePath);
				using (var package = new ExcelPackage(newFile))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Arrivals");
					worksheet.Cells["A1"].LoadFromDataTable(excelDataTable, true, TableStyles.None);
					package.Save();
				}
				return true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Action not completed. Please close excel document while the program is running.");
				return false;
			}
		}

		private void SaveJournalistsToExcel()
		{
			try
			{
				DataTable excelDataTable = new DataTable();
				excelDataTable.Columns.Add("company");
				excelDataTable.Columns.Add("country");
				excelDataTable.Columns.Add("name");
				excelDataTable.Columns.Add("barcode");
				excelDataTable.Columns.Add("numeration");

				foreach(Journalist journalist in journalists.Values)
				{
					excelDataTable.Rows.Add(journalist.company, journalist.country, journalist.name, journalist.barcode, journalist.numeration);
				}

				if (File.Exists(journalistFilePath))
					File.Delete(journalistFilePath);

				var newFile = new FileInfo(journalistFilePath);
				using (var package = new ExcelPackage(newFile))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Journalists");
					worksheet.Cells["A1"].LoadFromDataTable(excelDataTable, true, TableStyles.None);
					package.Save();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("Action not completed. Please close excel document while the program is running.");
			}
		}



		private DataTable GetDataTableFromExcel(string filepath)
		{
			DataTable _excelDB = new DataTable();
			using (var pck = new ExcelPackage())
			{
				try
				{
					using (var stream = File.OpenRead(filepath))
					{
						pck.Load(stream);
					}

					var ws = pck.Workbook.Worksheets.First();
					bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
					foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
					{
						_excelDB.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
					}
					var startRow = hasHeader ? 2 : 1;
					for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
					{
						var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
						var row = _excelDB.NewRow();
						foreach (var cell in wsRow)
						{
							row[cell.Start.Column - 1] = cell.Text;
						}
						_excelDB.Rows.Add(row);
					}
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message, "Error reading database");
					return null;
				}
			}
			return _excelDB;
		}


		public Dictionary<string, Journalist> Journalists
		{
			get
			{
				return journalists;
			}
		}

	}
}
