using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FancyPaint.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public List<int> Size
        {
            get { return new List<int>() {5, 10, 15, 20, 25, 30 }; }
        }


        
        private string _title = "Tp 4 IHM";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
