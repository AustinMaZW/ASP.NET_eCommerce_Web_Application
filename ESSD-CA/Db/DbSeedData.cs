using ESSD_CA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Db
{
    public class DbSeedData
    {
        private readonly DbESSDCA db;
        public DbSeedData(DbESSDCA db)
        {
            this.db = db;
        }

        public void Init()
        {
            //add code for generating data
            AddProduct();
        }

        private void AddProduct()
        {
            string[] productNames = { "Retention Graph", "Churn Analysis" };
            string[] productDescription = { "Visual representation of retention rates", "Detailed Analysis of customer churn" };
            double[] unitPrice = { 120, 299.99 };
            // to replace with real links later
            string[] downloadLink =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350"
            };
            // to replace with real links later
            string[] imagePath =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350"
            };
            for (int i = 0; i < productNames.Length; i++)
            {
                db.Products.Add(new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductName = productNames[i],
                    ProductDescription = productDescription[i],
                    UnitPrice = unitPrice[i],
                    DownloadLink = downloadLink[i],
                    ImagePath = imagePath[i],
                });

                db.SaveChanges();
            }
        }
    }
}
