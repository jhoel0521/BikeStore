using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Models;

public partial class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    [Display(Name = "Codigo Orden")]
    [Required(ErrorMessage = "El codigo la orden es obligatorio")]
    public int OrderId { get; set; }
    [Display(Name = "Codigo Propducto")]
    [Required(ErrorMessage = "El codigo del Propducto es obligatorio")]
    public int ProductId { get; set; }
    [Display(Name = "Cantidad", Prompt = "1")]
    [Required(ErrorMessage = "La cantidad Es Obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad Tiene que ser positiva")]
    public int Quantity { get; set; }
    [Display(Name = "Precio", Prompt = "99.99")]
    [Column(TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage = "El Precio es requerido")]
    [Range(0, 99999999.99, ErrorMessage = "El valor deve estar entre 0 y 99999999.99")]

    public decimal Price { get; set; }
    [Display(Name = "Descuento", Prompt = "99.99")]
    [Column(TypeName = "decimal(4, 2)")]
    [Required(ErrorMessage = "El descuento es requerido")]
    [Range(0,99.99,ErrorMessage ="El valor deve estar entre 0 y 99.99")]
    public decimal Discount { get; set; }
    [Display(Name = "Codigo de la Orden")]
    public virtual Order? Order { get; set; }
    [Display(Name = "Codigo del Producto")]
    public virtual Product? Product { get; set; }
}
