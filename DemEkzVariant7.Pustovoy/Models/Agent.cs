using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemEkzVariant7.Pustovoy.Models
{
    public partial class Agent
    {
        private string? _logo;

        public Agent()
        {
            AgentPriorityHistories = new HashSet<AgentPriorityHistory>();
            ProductSales = new HashSet<ProductSale>();
            Shops = new HashSet<Shop>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AgentTypeId { get; set; }
        public string? Address { get; set; }
        public string Inn { get; set; } = null!;
        public string? Kpp { get; set; }
        public string? DirectorName { get; set; }
        public string Phone { get; set; } = null!;
        public string? Email { get; set; }
        public string? Logo { get => _logo; set => _logo = value; }
        public int Priority { get; set; }

        [NotMapped]
        public string LogoPath
        {
            get => ((_logo == null) || (_logo == String.Empty)) ? $"..\\Resources\\picture.png" : $"..\\Resources{_logo}";
        }

        [NotMapped]
        public string FullTitle => $"{AgentType.Title} | {Title}";

        [NotMapped]
        public int SalesPerYear
        {
            get
            {
                int sales = 0;
                var YearAgoDate = new DateTime(DateTime.Now.Year - 8, DateTime.Now.Month, DateTime.Now.Day);

                foreach(var productSale in ProductSales)
                    if (productSale.SaleDate >= YearAgoDate)
                        sales++;

                return sales;
            }
        }

        [NotMapped]
        public int Discount
        {
            get
            {
                decimal total_cost = 0;
                foreach (var productSale in ProductSales)
                    total_cost += productSale.Product.MinCostForAgent * (decimal)productSale.ProductCount;

                if (total_cost <= 10000)
                    return 0;
                else if (total_cost > 10000 && total_cost <= 50000)
                    return 5;
                else if (total_cost > 50000 && total_cost <= 150000)
                    return 10;
                else if (total_cost > 150000 && total_cost <= 500000)
                    return 20;
                else
                    return 25;
            }
        }

        public virtual AgentType AgentType { get; set; } = null!;
        public virtual ICollection<AgentPriorityHistory> AgentPriorityHistories { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
    }
}
