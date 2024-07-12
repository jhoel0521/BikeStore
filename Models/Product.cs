using System;
using System.Collections.Generic;

namespace BikeStore.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int ModelYear { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
