using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

            if(!existProduct)
            {
                productCollection.InsertManyAsync(GetPreConfiguredProducts());
            }
            
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id="602d2149e773f2a3990b47f1",
                    Name="Purnabramha Sugar 1 KG",
                    Category="Grocery",
                    Summary="Good Quality Sugar",
                    Description="Good Quality Sugar",
                    ImageFile="Sugar.jpg",
                    Price=55.0M
                    
                },
                new Product()
                {
                    Id="602d2149e773f2a3990b47f2",
                    Name="Purnabramha Sugar 500 GMS",
                    Category="Grocery",
                    Summary="Good Quality Sugar",
                    Description="Good Quality Sugar",
                    ImageFile="Sugar.jpg",
                    Price=28.0M

                },
                new Product()
                {
                    Id="602d2149e773f2a3990b47f3",
                    Name="Purnabramha Sugar 2 KG",
                    Category="Grocery",
                    Summary="Good Quality Sugar",
                    Description="Good Quality Sugar",
                    ImageFile="Sugar.jpg",
                    Price=110.0M

                },

            };
        }
    }
}