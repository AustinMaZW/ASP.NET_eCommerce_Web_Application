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
            AddUser();
            AddPurchaseOrderAndDetails(); // just for testing
        }

        private void AddProduct()
        {
            string[] productNames = { "Retention Graph", "Churn Analysis", "Software Estimation", "Code Complete 2", "More Effective Agile" };
            string[] productDescription = { "Visual representation of retention rates", "Detailed Analysis of customer churn",
            "Unravels the mystery to successful software estimation", "A Practical Handbook of Software Construction",
            "A Roadmap for Software Leaders" };
            double[] unitPrice = { 120, 299.99, 32.57, 45, 34.95 };
            // to replace with real links later
            string[] downloadLink =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350",
                "https://images.unsplash.com/photo-1471194402529-8e0f5a675de6?w=350",
                "https://images.unsplash.com/photo-1438907046657-4ae137eb8c5e?w=350",
                "https://images.unsplash.com/photo-1453487021979-5b739b2849f4?w=350"
            };
            // to replace with real links later
            string[] imagePath =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350",
                "https://images.unsplash.com/photo-1471194402529-8e0f5a675de6?w=350",
                "https://images.unsplash.com/photo-1438907046657-4ae137eb8c5e?w=350",
                "https://images.unsplash.com/photo-1453487021979-5b739b2849f4?w=350"
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

        private void AddUser()
        {
            db.Users.AddRange(new User[] 
            { 
                new User("john"), 
                new User("mary"),
                new User("jane"),
                new User("peter")
            });
            db.SaveChanges();

        }

        private void AddPurchaseOrderAndDetails()
        {
            List<User> users = db.Users.ToList();

            // Add 3 records of purchase order
            string[] purchaseDate = { "1/4/2021", "1/12/2021", "2/15/2021" }; // MM/dd/yyyy
            double[] grandTotal = { 128.0, 90.5, 110.7 };
            string[] poId = new string[3];
            for (int i=0; i< poId.Length; i++)
            { 
                poId[i] = Guid.NewGuid().ToString();
            }
            // add two orders for user1
            db.Add(new PurchaseOrder
            {
                OrderId = poId[0],
                PurchaseDate = DateTime.Parse(purchaseDate[0]),
                GrandTotal = grandTotal[0],
                User = users[0] // user1
            });
            db.Add(new PurchaseOrder
            {
                OrderId = poId[1],
                PurchaseDate = DateTime.Parse(purchaseDate[1]),
                GrandTotal = grandTotal[1],
                User = users[0] // user1
            });
            // add one order for user2
            db.Add(new PurchaseOrder
            {
                OrderId = poId[2],
                PurchaseDate = DateTime.Parse(purchaseDate[2]),
                GrandTotal = grandTotal[2],
                User = users[1] // user2
            });
            db.SaveChanges();

            List<Product> products = db.Products.ToList();

            // Add PODetails for 1st purchase order
            string[] activationCode = new string[5];
            for (int i = 0; i < activationCode.Length; i++)
            {
                /*activationCode[i] = Guid.NewGuid().ToString();*/

                db.Add(new PurchaseOrderDetails
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    OrderId = poId[0], // 1st purchase order
                    ProductId = products[i].Id
                });
            }

            // Add PODetails for 2nd purchase order
            db.Add(new PurchaseOrderDetails
            {
                ActivationCode = Guid.NewGuid().ToString(),
                OrderId = poId[1], // 2nd purchase order
                ProductId = products[0].Id
            });
            db.Add(new PurchaseOrderDetails
            {
                ActivationCode = Guid.NewGuid().ToString(),
                OrderId = poId[1],
                ProductId = products[0].Id
            });

            // Add PODetails for 3rd purchase order
            db.Add(new PurchaseOrderDetails
            {
                ActivationCode = Guid.NewGuid().ToString(),
                OrderId = poId[2], // 3rd purchase order
                ProductId = products[1].Id
            });
            db.Add(new PurchaseOrderDetails
            {
                ActivationCode = Guid.NewGuid().ToString(),
                OrderId = poId[2],
                ProductId = products[1].Id
            });
            db.Add(new PurchaseOrderDetails
            {
                ActivationCode = Guid.NewGuid().ToString(),
                OrderId = poId[2],
                ProductId = products[2].Id
            });

            db.SaveChanges();
        }
    }
}
