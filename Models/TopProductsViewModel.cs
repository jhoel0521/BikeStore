namespace BikeStore.Models
{
    public class TopProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<dynamic> TopProducts { get; set; }
        public int CantTop { get; set; }
    }
}
