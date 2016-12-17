using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ExitBarcodeScanner2016.Common
{
	public class RelayCommand : RelayCommand<object>
	{
		#region Constructors

		public RelayCommand(Action<object> execute)
			: base(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
			: base(execute, canExecute)
		{
		}

		#endregion // Constructors
	}

	public class RelayCommand<T> : ICommand
	{
		#region Fields

		private readonly Action<T> execute;
		private readonly Predicate<T> canExecute;
		private bool isActive;

		//private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		#endregion // Fields

		#region Constructors

		public RelayCommand(Action<T> execute)
			: this(execute, null)
		{
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}
		#endregion // Constructors

		#region Events

		public event EventHandler CanExecuteChanged
		{
			add
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested += value;
				}
			}

			remove
			{
				if (canExecute != null)
				{
					CommandManager.RequerySuggested -= value;
				}
			}
		}

		/// <summary>
		/// Fired if the <see cref="IsActive"/> property changes.
		/// </summary>
		public event EventHandler IsActiveChanged;

		#endregion // Events

		#region ICommand Members

		[DebuggerStepThrough]
		public bool CanExecute(object parameter)
		{
			return this.canExecute == null ? true : this.canExecute((T)parameter);
		}

		public void Execute(object parameter)
		{
			// log.Info(String.Format("Class:{0} invoke execute command with name:{1}", execute.Method.DeclaringType.FullName, execute.Method.Name));
			this.execute((T)parameter);
		}

		#endregion // ICommand Members
	}
}
