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
            AddCustomerEnquiries();
            //AddPurchaseOrderAndDetails();
            /*AddPO(); // for populating PO only, to be deleted
            AddPOD();*/ // for populating PODetail only, to be deleted
        }

        private void AddProduct()
        {
            string[] productNames = { "Photoshop Elements 2021", "Successful Software Manager", "Software Estimation", "Code Complete 2", "More Effective Agile",
                "Adobe Photoshop Classroom in a Book", "Python Basics", "System Design Interview", "SQL Quick Start Guide", "C# 9 and .NET 5",
                "Java: The Complete Reference" };
            string[] productDescription =
                { "Deliver the photo editing power that non-professional photographers and designers need to get eye-catching and art-quality results.",
                "The definitive guide to growing from developer to manager.",
                "Unravels the mystery to successful software estimation - distilling academic information and real-world experience into a practical guide for working software professionals.",
                "The original CODE COMPLETE has been fully updated and revised with leading-edge practices—and hundreds of new code samples—illustrating the art and science of software construction.",
                "A Roadmap for Software Leaders covering the practical considerations needed to ensure you reap the full benefits of effective Agile.",
                "Key step-by-step techniques for working in Photoshop, including how to correct, enhance, and distort digital images, create image composites, and prepare images for print and the web.",
                "Your Complete Python Curriculum—With Exercises, Interactive Quizzes, and Sample Projects.",
                "Second Edition, Provides a step-by-step framework on how to tackle a system design question.",
                "The Simplified Beginner's Guide to Managing, Analyzing, and Manipulating Data With SQL.",
                "Build intelligent apps, websites, and services with Blazor, ASP.NET Core, and Entity Framework Core using Visual Studio Code, 5th Edition.",
                "The Complete Reference, Eleventh Edition explains how to develop, compile, debug, and run Java programs." };
            double[] unitPrice = { 25.65, 35.99, 19.8, 45.2, 34.95,
                53.99, 35.99, 25, 22.5, 32.4,
                44 };
            // to replace with real links later
            /*string[] downloadLink =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350",
                "https://images.unsplash.com/photo-1471194402529-8e0f5a675de6?w=350",
                "https://images.unsplash.com/photo-1438907046657-4ae137eb8c5e?w=350",
                "https://images.unsplash.com/photo-1453487021979-5b739b2849f4?w=350"
            };*/
            string[] downloadLink =
            {
                "PhotoshopElements2021.pdf",
                "SuccessfulSoftwareManager.pdf",
                "SoftwareEstimation.pdf",
                "CodeComplete2.pdf",
                "MoreEffectiveAgile.pdf",
                "AdobePhotoshopClassroom.pdf",
                "PythonBasics.pdf",
                "SystemDesignInterview.pdf",
                "SQLQuickStart.pdf",
                "CSharp9andDotNet5.pdf",
                "JavaCompleteReference.pdf"
            };
            // to replace with real links later
            /*string[] imagePath =
            {
                "https://images.unsplash.com/photo-1482012792084-a0c3725f289f?w=350",
                "https://images.unsplash.com/photo-1464454709131-ffd692591ee5?w=350",
                "https://images.unsplash.com/photo-1471194402529-8e0f5a675de6?w=350",
                "https://images.unsplash.com/photo-1438907046657-4ae137eb8c5e?w=350",
                "https://images.unsplash.com/photo-1453487021979-5b739b2849f4?w=350"
            };*/
            string[] imagePath =
            {
                "PhotoshopElements2021.jpg",
                "SuccessfulSoftwareManager.jpg",
                "SoftwareEstimation.jpg",
                "CodeComplete2.jpg",
                "MoreEffectiveAgile.jpg",
                "AdobePhotoshopClassroom.jpg",
                "PythonBasics.jpg",
                "SystemDesignInterview.jpg",
                "SQLQuickStart.jpg",
                "CSharp9andDotNet5.jpg",
                "JavaCompleteReference.jpg"
            };
            for (int i = 0; i < productNames.Length; i++)
            {
                db.Products.Add(new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductName = productNames[i],
                    ProductDescription = productDescription[i],
                    UnitPrice = unitPrice[i],
                    DownloadLink = "/products/pdf/" + downloadLink[i],
                    ImagePath = "/products/img/" + imagePath[i],
                    ProductStatus = "Available"
                }) ;

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

            db.Users.Add(new User("admin")
            {
                AccountType = "Admin"       // Admin dictates role as Admin, User dictates User
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
            for (int i = 0; i < poId.Length; i++)
            {
                poId[i] = Guid.NewGuid().ToString();
            }
            // add two orders for user1
            db.Add(new PurchaseOrder
            {
                OrderId = poId[0],
                PurchaseDate = DateTime.Parse(purchaseDate[0]).ToUniversalTime(),
                GrandTotal = grandTotal[0],
                User = users[0] // user1
            });
            db.Add(new PurchaseOrder
            {
                OrderId = poId[1],
                PurchaseDate = DateTime.Parse(purchaseDate[1]).ToUniversalTime(),
                GrandTotal = grandTotal[1],
                User = users[0] // user1
            });
            // add one order for user2
            db.Add(new PurchaseOrder
            {
                OrderId = poId[2],
                PurchaseDate = DateTime.Parse(purchaseDate[2]).ToUniversalTime(),
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
                    UserId = "11111",
                    GrandTotal = 2230.65
                },
                new PurchaseOrder()
                {
                    OrderId = "002",
                    PurchaseDate = DateTime.Now.ToUniversalTime(),
                    UserId = "22222",
                    GrandTotal = 3000.50
                }
            });
            db.SaveChanges();
        }

        // To be deleted, for populating PODetail only
        private void AddPOD()
        {
            db.PODetails.AddRange(new PurchaseOrderDetails[]
            {
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product001",
                    OrderId = "001"
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product001",
                    OrderId = "001"
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product001",
                    OrderId = "001"
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product002",
                    OrderId = "001"
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product001",
                    OrderId = "002"
                },
                new PurchaseOrderDetails()
                {
                    ActivationCode = Guid.NewGuid().ToString(),
                    ProductId = "Product002",
                    OrderId = "002"
                }
            });
            db.SaveChanges();

        }
        private void AddCustomerEnquiries()
        {
            string[] userNames = { "John", "Mary", "Jane"};
            string[] userEmails = { "john@email.com", "mary@email.com", "jane@email.com"};
            string[] userMessages =
                {"Hi, what types of license are available for the ebook 'PhotoshopElements2021'?",
                 "I'm looking to bulk license 'CodeComplete2', do you have a separate quotation for this?",
                 "Are your e-book licenses two-year terms/limit in perpetuity?",
                };

            for (int i = 0; i < userMessages.Length; i++)
            {
                db.CustomerMessages.Add(new CustomerMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = userNames[i],
                    Email = userEmails[i],
                    Message = userMessages[i],
                    EnquiryStatus = false
                }); ;
            }
            db.SaveChanges();
        }

    }
}
