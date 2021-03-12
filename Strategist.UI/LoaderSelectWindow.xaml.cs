using Strategist.UI.ViewModels;

namespace Strategist.UI
{
    public partial class LoaderSelectWindow
    {
        public LoaderSelectWindow()
        {
            InitializeComponent();
            DataContext = new LoaderSelectWindowViewModel();
        }
    }
}