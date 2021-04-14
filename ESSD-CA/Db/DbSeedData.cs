using ESSD_CA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using ESSD_CA.Controllers;
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

            AddPO(); // for populating PO only, to be deleted
            AddPOD(); // for populating PODetail only, to be deleted

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

        // To be deleted, For Populating Purchase Orders
        private void AddPO()
        {
            User john = db.Users.FirstOrDefault(x => x.Username == "john");
            User jane = db.Users.FirstOrDefault(x => x.Username == "jane");
            db.PurchaseOrders.AddRange(new PurchaseOrder[]
            {
                new PurchaseOrder(){
                    OrderId = "001",
                    PurchaseDate=DateTime.Now.ToUniversalTime(),
                    UserId = john.UserId,
                    GrandTotal = 2230.65
                },
                new PurchaseOrder()
                {
                    OrderId = "002",
                    PurchaseDate = DateTime.Now.ToUniversalTime(),
                    UserId = jane.UserId,
                    GrandTotal = 3000.50
                }
            });
        }

         // To be deleted, for populating PODetail only
        private void AddPOD()
        {
            User john = db.Users.FirstOrDefault(x => x.Username == "john");
            User jane = db.Users.FirstOrDefault(x => x.Username == "jane");
            Product product1 = db.Products.FirstOrDefault(x => x.ProductName == "Churn Analysis");
            Product product2 = db.Products.FirstOrDefault(x => x.ProductName == "Retention Graph");
            PurchaseOrder order1 = db.PurchaseOrders.FirstOrDefault(x => x.OrderId == "001");
            PurchaseOrder order2 = db.PurchaseOrders.FirstOrDefault(x => x.OrderId == "002");
            db.PODetails.AddRange(new PurchaseOrderDetails[]
            {
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product1.Id,
                    OrderId = order1.OrderId
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product1.Id,
                    OrderId = order1.OrderId
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product1.Id,
                    OrderId = order1.OrderId
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product2.Id,
                    OrderId = order1.OrderId
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product1.Id,
                    OrderId = order2.OrderId
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = product2.Id,
                    OrderId = order2.OrderId
                }
            });

        }

    }
}
