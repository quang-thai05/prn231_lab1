﻿using Lab1.Dtos;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace Lab1.Formatter
{
    public class CsvInputFormatter : TextInputFormatter
    {
        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(OrderDto);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;
            using var reader = new StreamReader(httpContext.Request.Body, encoding);
            string? dataLine = null;
            try
            {
                await ReadLineAsync($"EmployeeId,CustomerId,OrderDate,Products", reader, context);
                dataLine = await ReadLineAsync(null, reader, context);
                var data = dataLine.Split(',');
                var products = new List<ProductDto>();

                for (var i = 3; i < data.Length; i += 3)
                {
                    var product = new ProductDto
                    {
                        ProductId = int.Parse(data[i]),
                        Quantity = short.Parse(data[i + 1]),
                        Price = decimal.Parse(data[i + 2])
                    };
                    products.Add(product);
                }

                var order = new OrderDto()
                {
                    EmployeeId = Convert.ToInt32(data[0]),
                    CustomerId = data[1],
                    OrderDate = Convert.ToDateTime(data[2]),
                    Products = products
                };
                return await InputFormatterResult.SuccessAsync(order);
            }
            catch (Exception)
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        private static async Task<string> ReadLineAsync(string expectedText, StreamReader reader, InputFormatterContext context)
        {
            var line = await reader.ReadLineAsync();

            if (expectedText != null)
            {
                if (line is null || !line.StartsWith(expectedText))
                {
                    var errorMessage = $"Looked for '{expectedText}' and got '{line}'";
                    context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            return line;
        }
    }
}