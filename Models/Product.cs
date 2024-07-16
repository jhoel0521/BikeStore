using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Models;

public partial class Product
{
    public int ProductId { get; set; }
    [Display(Name = "Nombre del producto", Prompt = "Bicicleta turbo 3000")]
    [Required(ErrorMessage = "El Nombre del producto es requedio")]
    [StringLength(200, ErrorMessage = "El nombre del producto  no puede exeder los 50 caracteres")]
    public string? ProductName { get; set; }
    [Display(Name = "Año del producto", Prompt = "2024")]
    [Required(ErrorMessage = "El año del modelo del producto es requedido")]
    public int ModelYear { get; set; }
    [Display(Name ="Precio",Prompt ="99.99")]
    [Column(TypeName = "decimal(10, 2)")]
    [Required(ErrorMessage ="El Precio es requerido")]
    public decimal? Price { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
