using ConwaysGameOfLife.ViewModel;
using System.Windows;

namespace ConwaysGameOfLife.View
{
    /// <summary>
    /// Interaction logic for ConwaysGameOfLifeView.xaml
    /// </summary>
    public partial class ConwaysGameOfLifeView : Window
    {
        #region Fields
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ConwaysGameOfLifeView()
        {
            InitializeComponent();

            // Create the View Model.
            ConwaysGameOfLifeViewModel viewModel = new ConwaysGameOfLifeViewModel();
            DataContext = viewModel;    // Set the data context for all data binding operations.
        }

        #endregion

        #region Events
        #endregion

        #region Properties
        #endregion

        #region Methods
        #endregion


    }
}
