using AutoMapper;
using Lab1.Dtos;
using Lab1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Prn231DbDemoContext _context;
        private readonly IMapper _mapper;

        public OrderController(Prn231DbDemoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{ordId}")]
        public ActionResult<OrderDto> Detail(int ordId)
        {
            try
            {
                var order = _context.Orders.Where(x => x.OrderId == ordId).Include(x => x.OrderDetails).FirstOrDefault();

                if (order == null || order.OrderDetails == null)
                {
                    return BadRequest();
                }

                var productsDto = new List<ProductDto>();
                foreach (var item in order.OrderDetails)
                {
                    var productDto = new ProductDto()
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.UnitPrice
                    };
                    productsDto.Add(productDto);
                }

                var orderDto = new OrderDto()
                {
                    OrderId = order.OrderId,
                    EmployeeId = order.EmployeeId,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    Products = productsDto
                };
                return Ok(orderDto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{fromDate}/{endDate}")]
        public ActionResult<IEnumerable<OrderDto>> List(string fromDate, string endDate)
        {
            try
            {
                IEnumerable<Order> orders = _context.Orders
                    .Where(x => x.OrderDate <= DateTime.Parse(endDate) && x.OrderDate >= DateTime.Parse(fromDate))
                    .Include(x => x.OrderDetails)
                    .ToList();
                return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<OrderDto> Add(OrderDto orderDto)
        {
            try
            {
                _context.Orders.Add(_mapper.Map<Order>(orderDto));
                _context.SaveChanges();
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
