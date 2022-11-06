using DemEkzVariant7.Pustovoy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemEkzVariant7.Pustovoy.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel? viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = (MainWindowViewModel)DataContext;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.Search(TextBoxInput.Text);
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.Sort(ComboBoxSort.SelectedIndex);
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.Filter(ComboBoxFilter.ItemsSource as List<string>, ComboBoxFilter.SelectedIndex);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (viewModel != null)
                viewModel.ShowProductWindow(true); // edit window
        }

        private void AddAgent_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                viewModel.ShowProductWindow(false); // add window
        }

        private void MenuItemChangePriority_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel != null)
                viewModel.ChangePriority();
        }
    }
}
