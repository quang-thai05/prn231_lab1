using Lab1.Dtos;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace Lab1.Formatter
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add("text/csv");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (type == typeof(OrderDto))
            {
                return true;
            }
            else
            {
                Type enumerableType = typeof(IEnumerable<OrderDto>);
                return enumerableType.IsAssignableFrom(type);
            }
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;
            var buffer = new StringBuilder();

            buffer.Append($"OrderId,EmployeeId,CustomerId,OrderDate,ProductId,Quantity,Price");
            buffer.AppendLine();
            if (context.Object is IEnumerable<OrderDto> orders)
            {
                foreach (var order in orders)
                {
                    FormatCSV(buffer, order);
                }
            }
            else
            {
                FormatCSV(buffer, (OrderDto)context.Object!);
            }

            await httpContext.Response.WriteAsync(buffer.ToString(), selectedEncoding);
        }

        private void FormatCSV(StringBuilder buffer, OrderDto contact)
        {
            buffer.Append($"{contact.OrderId},");
            buffer.Append($"{contact.EmployeeId},");
            buffer.Append($"{contact.CustomerId},");
            buffer.Append($"{contact.OrderDate},");
            foreach (var product in contact.Products)
            {
                buffer.Append($"{product.ProductId},");
                buffer.Append($"{product.Quantity},");
                buffer.Append($"{product.Price},");
            }
            buffer.AppendLine();
        }
    }
}
