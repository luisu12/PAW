using Microsoft.Identity.Client;
using PAW.DataAccess.MSSQL;
using PAW.Models;

namespace PAW.Manager
{
    public class ProductTransactionsManager(IProductTransactionLayer productTransactionLayer)
    {
        public async Task<Product> GenerateProductTransaction()
        {
            var transaction = await productTransactionLayer.CreateProductAsync();
            transaction.UpdateTransaction()
                .AddTimestamp()
                .AssignCategory(new Category { })
                .UpdateTransaction()
                .AddTimestamp()
                .CreateInventory()
                .UpdateTransaction()
                .FunctionExample(_ => transaction.Product.ProductId > 5);

            return transaction.Product;
        }        
    }
}
