using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    //private static readonly string[] Products = new[]
    //{
    //    "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    //};
    private const decimal ConversionRate = 1.11m; // 1 GBP to 1.11 EUR
    private readonly ILogger<ProductController> _logger;
    private readonly Greggs.Products.Api.DataAccess.IDataAccess _dataAccess;

    public ProductController(ILogger<ProductController> logger, IDataAccess dataAccess)
    {
        _logger = logger;
        _dataAccess = dataAccess;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        foreach (var product in listOfProducts)
        {
            product.PriceInEuros = ConvertToEuro(product.PriceInPounds);
        }


        var listOfProducts = await _dataAccess.List(pageStart, pageSize);
        return listOfProducts;

    }

    private decimal ConvertToEuro(decimal priceInPounds)
    {
        return priceInPounds * ConversionRate;
    }
}