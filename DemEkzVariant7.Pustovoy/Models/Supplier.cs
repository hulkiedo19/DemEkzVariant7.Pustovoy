﻿using System;
using System.Collections.Generic;

namespace DemEkzVariant7.Pustovoy.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Inn { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public int? QualityRating { get; set; }
        public string? SupplierType { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
