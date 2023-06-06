namespace Lab1.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int? EmployeeId { get; set; }
        public string CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public List<ProductDto>? Products { get; set; }
    }
}
