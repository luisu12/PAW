using PAW.Models;
using PAW.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PAW.DataAccess.MSSQL;

public interface IProductTransactionLayer
{
    ProductTransactionLayer AssignCategory(Category category);
    ProductTransactionLayer CreateInventory();
    Task<ProductTransactionLayer> CreateProductAsync();
}

public class ProductTransactionLayer(IProductRepository repository) : IProductTransactionLayer
{
    public Product Product { get { return _product; } }
    Product _product;
    // product product
    // category
    // incident inventory

    public async Task<ProductTransactionLayer> CreateProductAsync()
    {
        _product = new Product();
        await repository.CreateAsync(_product);
        // commit
        return this;
    }

    public ProductTransactionLayer AssignCategory(Category category)
    {
        _product.Category = category;
        // commit
        return this;
    }

    public ProductTransactionLayer CreateInventory()
    {
        _product.Inventory = new Inventory
        {
            InventoryId = new Random().Next()
        };
        // Finish creation of inventory
        // InventoryRepository create inventory Item
        // commit
        return this;
    }

    public ProductTransactionLayer UpdateTransaction()
    {
        // Update the product transaction details
        // Commit changes to the database
        return this;
    }

    public ProductTransactionLayer AddTimestamp()
    {
        // Add a timestamp to the product transaction
        // Commit changes to the database
        return this;
    }

    public ProductTransactionLayer FunctionExample(Func<Product, bool> func)
    {
        _product.Category.Products.ToList().Where(x => func(x));
        return this;
    }

    public ProductTransactionLayer FunctionExample(Action<Product> func)
    {
        _product.Category.Products.ToList().ForEach(x => func(x));
        return this;
    }
}
