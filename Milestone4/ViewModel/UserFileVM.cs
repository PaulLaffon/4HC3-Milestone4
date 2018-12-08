using System;
using System.Windows.Input;

namespace Milestone4.ViewModel
{
    public class UserFileVM : ViewModel
    {
        private Action onDelete;
        private Action onOpen;
        public string FileName { get; private set; }

        public ICommand DeleteFile { get { return new ButtonCommand(onDelete); } }
        public ICommand SeeFile { get { return new ButtonCommand(onOpen); } }

        public UserFileVM(Action onDelete, Action onOpen, string fileName)
        {
            FileName = fileName;
            this.onDelete = onDelete;
            this.onOpen = onOpen;
        }
    }
}
