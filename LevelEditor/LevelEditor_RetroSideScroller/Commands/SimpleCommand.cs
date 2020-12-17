using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetJeuxVideo_LevelEditor_Metroidvania.Commands
{
    public class SimpleCommand : ICommand
    {

        public Action<object> ExecuteFunc
        {
            get;
            set;
        }

        public SimpleCommand(Action<object> execute)
        {
            ExecuteFunc = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public void Execute(object parameter)
        {
            ExecuteFunc(parameter);
        }

    }
}
