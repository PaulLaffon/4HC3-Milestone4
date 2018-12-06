using System.Collections.Generic;
using System.Linq;
using Milestone4.Model;

namespace Milestone4.ViewModel
{
    public class FilterViewModel : ViewModel
    {
        #region Static filter values
        public static readonly List<string> SalaryFilterValues = new List<string> {
            "Less than $20,000",
            "$20,000 - $40,000",
            "$40,000 - $60,000",
            "$60,000 - $80,000",
            "$80,000 - $100,000",
            "Over $100,000"
        };

        public static readonly List<string> CompanyFilterValues = new List<string>(Job.Companies);

        public static readonly List<string> LevelFilterValues = new List<string>(Job.Levels);

        public static readonly List<string> LocationFilterValues = new List<string>(Job.Cities);

        public static readonly List<string> CategoryFilterValues = new List<string> {
            "Finance",
            "Human Resources",
            "Research",
            "Engineering",
            "Maintenance"
        };

        public static readonly List<string> TypeFilterValues = new List<string>(Job.Types.Distinct());
        #endregion

        private MainWindowViewModel mainVM;
        private bool isSelected = false;

        public string Type { get; private set; }
        public string Value { get; private set; }

        public bool IsSelectable { get; private set; }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
                if (Value == "All")
                    mainVM.SelectAllChanged(Type, isSelected);
                else
                    mainVM.ManageSelectAllStatus(Type);
                mainVM.Search();
            }
        }

        public bool IsSelectedWithoutEvent
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public FilterViewModel(MainWindowViewModel mainVM, string type, string value, bool isSelectable = true)
        {
            this.mainVM = mainVM;
            IsSelectable = isSelectable;
            Type = type;
            Value = value;
            isSelected = true;
        }
    }
}
