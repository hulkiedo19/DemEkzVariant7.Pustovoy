using DemEkzVariant7.Pustovoy.Models;
using DemEkzVariant7.Pustovoy.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemEkzVariant7.Pustovoy.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private List<Agent> _agents = new List<Agent>();
        private List<Agent> _page = new List<Agent>();
        private List<string> _comboBoxSort = new List<string>();
        private List<string> _comboBoxFilter = new List<string>();
        private List<Button> _buttonList = new List<Button>();
        private int _selectedIndex;
        private int _currentPage = 0;
        private int _pageCountMax = 1;
        private int _pageCountItem = 10;

        public List<Agent> Agents
        {
            get => _page;
            set => Set(ref _page, value, nameof(Agents));
        }
        public List<string> ComboBoxSort
        {
            get => _comboBoxSort;
            set => Set(ref _comboBoxSort, value, nameof(ComboBoxSort));
        }
        public List<string> ComboBoxFilter
        {
            get => _comboBoxFilter;
            set => Set(ref _comboBoxFilter, value, nameof(ComboBoxFilter));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }
        public List<Button> ButtonList
        {
            get => _buttonList;
            set => Set(ref _buttonList, value, nameof(ButtonList));
        }

        public MainWindowViewModel()
        {
            UpdateItems(null);
            InitializeComboBoxes();
            InitializePages();
        }

        // main methods
        public void UpdateItems(string? text)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(text == null)
                {
                    _agents = context.Agents
                        .Include(at => at.AgentType)
                        .Include(ps => ps.ProductSales)
                        .ThenInclude(p => p.Product)
                        .ToList();
                }
                else
                {
                    _agents = context.Agents
                        .Include(p => p.AgentType)
                        .Include(ps => ps.ProductSales)
                        .ThenInclude(p => p.Product)
                        .Where(p => p.Title.ToLower().Contains(text.ToLower()) || 
                            p.Phone.ToLower().Contains(text.ToLower()) || 
                            p.Email.ToLower().Contains(text.ToLower()))
                        .ToList();
                }
            }
        }

        public void InitializeComboBoxes()
        {
            ComboBoxSort.Add("Название >");
            ComboBoxSort.Add("Название <");
            ComboBoxSort.Add("Скидка >");
            ComboBoxSort.Add("Скидка <");
            ComboBoxSort.Add("Приоритет >");
            ComboBoxSort.Add("Приоритет <");

            ComboBoxFilter.Add($"Все типы");
            foreach (var types in new ApplicationDbContext().AgentTypes.ToList())
                ComboBoxFilter.Add($"{types.Title}");
        }

        // page methods
        public void InitializePages()
        {
            _currentPage = 0;
            _pageCountMax = (_agents.Count / _pageCountItem) + ((_agents.Count % _pageCountItem) > 0 ? 1 : 0);
            SetPage();
            InitializeButtons();
        }

        public void SetPage()
        {
            Agents = _agents
                .Skip(_currentPage * _pageCountItem)
                .Take(_pageCountItem)
                .ToList();
        }

        public void InitializeButtons()
        {
            List<Button> buttons = new List<Button>();

            Button leftPage = new Button();
            leftPage.Content = "<";
            leftPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            leftPage.Background = new SolidColorBrush(Colors.White);
            leftPage.BorderBrush = new SolidColorBrush(Colors.White);
            leftPage.BorderThickness = new System.Windows.Thickness(0);
            leftPage.Width = 15;
            leftPage.Click += LeftPage_Click;
            buttons.Add(leftPage);

            for(int i = 0; i < _pageCountMax; i++)
            {
                Button specifiedPage = new Button();
                specifiedPage.Content = $"{i + 1}";
                specifiedPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
                specifiedPage.Background = new SolidColorBrush(Colors.White);
                specifiedPage.BorderBrush = new SolidColorBrush(Colors.White);
                specifiedPage.BorderThickness = new System.Windows.Thickness(0);
                specifiedPage.Width = 15;
                specifiedPage.Click += SpecifiedPage_Click;
                buttons.Add(specifiedPage);
            }

            Button rightPage = new Button();
            rightPage.Content = ">";
            rightPage.Margin = new System.Windows.Thickness(0, 0, 2, 0);
            rightPage.Background = new SolidColorBrush(Colors.White);
            rightPage.BorderBrush = new SolidColorBrush(Colors.White);
            rightPage.BorderThickness = new System.Windows.Thickness(0);
            rightPage.Width = 15;
            rightPage.Click += RightPage_Click;
            buttons.Add(rightPage);

            ButtonList = buttons;
        }

        private void LeftPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_currentPage <= 0)
                return;

            _currentPage--;
            SetPage();
        }

        private void RightPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_currentPage >= _pageCountMax - 1)
                return;

            _currentPage++;
            SetPage();
        }

        private void SpecifiedPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int Index = Convert.ToInt32((sender as Button).Content);

            _currentPage = Index - 1;
            SetPage();
        }

        // called methods
        public void Sort(int Index)
        {
            switch(Index)
            {
                case 0:
                    _agents = _agents.OrderBy(a => a.Title).ToList();
                    break;
                case 1:
                    _agents = _agents.OrderByDescending(a => a.Title).ToList();
                    break;
                case 2:
                    _agents = _agents.OrderByDescending(a => a.Discount).ToList();
                    break;
                case 3:
                    _agents = _agents.OrderBy(a => a.Discount).ToList();
                    break;
                case 4:
                    _agents = _agents.OrderByDescending(a => a.Priority).ToList();
                    break;
                case 5:
                    _agents = _agents.OrderBy(a => a.Priority).ToList();
                    break;
            }

            InitializePages();
        }

        public void Filter(List<string> Types, int Index)
        {
            UpdateItems(null);

            if (Types[Index] != "Все типы")
                _agents = _agents.Where(a => a.AgentType.Title == Types[Index]).ToList();

            InitializePages();
        }

        public void Search(string Text)
        {
            if (Text == "")
                UpdateItems(null);
            else
                UpdateItems(Text);

            InitializePages();
        }

        public void ChangePriority()
        {
            string result = Microsoft.VisualBasic.Interaction.InputBox("Input a priority of agent");

            int new_priority = Convert.ToInt32(result);
            int id = _agents.ElementAt(SelectedIndex).Id;

            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var agent = context.Agents.Where(a => a.Id == id).First();
                agent.Priority = new_priority;

                context.SaveChanges();
            }

            UpdateItems(null);
            InitializePages();
        }

        public void ShowProductWindow(bool Flag)
        {
            int? agentId = (Flag == true) ? Agents.ElementAt(SelectedIndex).Id : null;

            if (new AgentsWindow(agentId).ShowDialog() == false)
                return;

            UpdateItems(null);
            InitializePages();
        }
    }
}
