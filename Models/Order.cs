using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeStore.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }
    [Display(Name = "Codigo Cliente: Nombre(s) y Apellido(s)")]
    [Required(ErrorMessage ="Se requiere Un cliente para asignar la orden")]
    public int CustomerId { get; set; }
    [Display(Name = "Fecha De La Orden")]
    [Required(ErrorMessage ="La orden tiene que tener una fecha Valida")]
    [DataType(DataType.Date,ErrorMessage ="Eliga una fecha valida")]
    public DateOnly OrderDate { get; set; }
    [Display(Name = "Cliente: Nombre(s) y Apellido(s)")]
    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
