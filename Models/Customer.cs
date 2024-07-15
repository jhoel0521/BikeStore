using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BikeStore.Models;

public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }
    [Display(Name = "Nombre(s)",Prompt ="Juan")]
    [Required(ErrorMessage = "El Nombre es requerido")]
    [StringLength(255, ErrorMessage = "El Nombre no puede exeder los 255 caracteres")]
    public string? FirstName { get; set; }
    [Display(Name = "Apellido(s)",Prompt = "Perez")]
    [Required(ErrorMessage = "El Apellido es requerido")]
    [StringLength(255, ErrorMessage = "El Apellido no puede exeder los 255 caracteres")]
    public string? LastName { get; set; }
    [Display(Name = "Telefono", Prompt = "59177045885")]
    [StringLength(25, ErrorMessage = "El Telefono no puede exeder los 25 caracteres")]
    public string? Phone { get; set; }
    [Display(Name = "Correo Electronico",Prompt ="correo@dominio.com")]
    [StringLength(255, ErrorMessage = "El Telefono no puede exeder los 255 caracteres")]
    [Required(ErrorMessage = "Es requerido un Correo Electronico")]
    [EmailAddress(ErrorMessage = "El campo tiene que ser un Correo Electronico valido")]
    public string? Email { get; set; }
    [Display(Name = "Calle/Direccion", Prompt = "Calle #9")]
    [StringLength(50, ErrorMessage = "La Calle/Direccion no puede exeder los 50 caracteres")]
    public string? Street { get; set; }
    [Display(Name = "Ciudad", Prompt = "Porongo")]
    [StringLength(50, ErrorMessage = "La ciudad no puede exeder los 50 caracteres")]
    public string? City { get; set; }
    [Display(Name = "Estado/Departamento", Prompt = "Santa Cruz")]
    [StringLength(50, ErrorMessage = "El Estado/Departamento no puede exeder los 50 caracteres")]
    public string? State { get; set; }
    [Display(Name = "Codigo Postal", Prompt = "0000")]
    [StringLength(5, ErrorMessage = "El Codigo Postal no puede exeder los 5 caracteres")]
    public string? ZipCode { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
