using DemEkzVariant7.Pustovoy.Models;
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
    /// Interaction logic for AgentsWindow.xaml
    /// </summary>
    public partial class AgentsWindow : Window
    {
        bool IsChanged = false;
        int? _agentId;

        public AgentsWindow(int? agentId)
        {
            InitializeComponent();
            _agentId = agentId;

            if (agentId == null)
            {
                Height = 440;
                Title = "Добавить агента";
                DeleteButton.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                Height = 470;
                Title = "Редактировать агента";
                AddButton.Visibility = Visibility.Collapsed;
            }

            SetTextBoxes();
            SetAgentTypeComboBox();
        }

        // initialize window
        private void SetTextBoxes()
        {
            if (_agentId == null)
                return;

            Agent agent = new ApplicationDbContext().Agents.Where(a => a.Id == _agentId).Single();

            TB_Title.Text = agent.Title;
            TB_Address.Text = agent.Address;
            TB_Inn.Text = agent.Inn;
            TB_Kpp.Text = agent.Kpp;
            TB_Director.Text = agent.DirectorName;
            TB_Phone.Text = agent.Phone;
            TB_Email.Text = agent.Email;
            TB_Logo.Text = agent.Logo;
            TB_Priority.Text = Convert.ToString(agent.Priority);
        }

        private void SetAgentTypeComboBox()
        {
            foreach (var AgentType in new ApplicationDbContext().AgentTypes.ToList())
                CB_AgentType.Items.Add(AgentType.Title);

            if (_agentId != null)
            {
                int agent_type_id = new ApplicationDbContext().Agents.Where(a => a.Id == _agentId).Single().AgentTypeId;
                string agent_type_title = new ApplicationDbContext().AgentTypes.Where(at => at.Id == agent_type_id).Single().Title;
                CB_AgentType.SelectedIndex = CB_AgentType.Items.IndexOf($"{agent_type_title}");
            }
        }

        // get/set methods
        private Agent GetAgentFromTextBoxes()
        {
            Agent agent = new Agent();
            agent.Title = TB_Title.Text;
            agent.Address = TB_Address.Text;
            agent.Inn = TB_Inn.Text;
            agent.Kpp = TB_Kpp.Text;
            agent.DirectorName = TB_Director.Text;
            agent.Phone = TB_Phone.Text;
            agent.Email = TB_Email.Text;
            agent.Logo = TB_Logo.Text;
            agent.Priority = Convert.ToInt32(TB_Priority.Text);

            var agent_type_title = CB_AgentType.Items[CB_AgentType.SelectedIndex].ToString();
            agent.AgentTypeId = new ApplicationDbContext().AgentTypes.Where(a => a.Title.Equals(agent_type_title)).Single().Id;

            return agent;
        }

        private void AddAgent(Agent agent)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Agents.Add(agent);
                context.SaveChanges();
            }
        }

        private void SaveAgent(Agent agent)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Agent _agentOld = context.Agents.Where(a => a.Id == _agentId).Single();

                _agentOld.Title = agent.Title;
                _agentOld.AgentTypeId = agent.AgentTypeId;
                _agentOld.Address = agent.Address;
                _agentOld.Inn = agent.Inn;
                _agentOld.Kpp = agent.Kpp;
                _agentOld.DirectorName = agent.DirectorName;
                _agentOld.Phone = agent.Phone;
                _agentOld.Email = agent.Email;
                _agentOld.Logo = agent.Logo;
                _agentOld.Priority = agent.Priority;

                context.SaveChanges();
            }
        }

        private void DeleteAgent(int? agentId)
        {
            if (agentId == null)
                return;

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var productSales = context.ProductSales.Where(ps => ps.AgentId == agentId).ToList();

                if(productSales.Count > 0)
                {
                    foreach (var productSale in productSales)
                        context.ProductSales.Remove(productSale);

                    context.SaveChanges();
                }

                Agent agent = context.Agents.Where(a => a.Id == agentId).Single();
                context.Agents.Remove(agent);
                context.SaveChanges();
            }
        }

        // click methods
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAgent(GetAgentFromTextBoxes());
            IsChanged = true;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveAgent(GetAgentFromTextBoxes());
            IsChanged = true;
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteAgent(_agentId);
            IsChanged = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = IsChanged;
        }
    }
}
