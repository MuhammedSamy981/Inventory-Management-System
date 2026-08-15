
using InventoryManagementSystem.Domain.Entities;
using InventoryManagementSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Infrastructure.Data
{
    public static class ApplicationDbHelper
    {

        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
        , ApplicationDbContext context)
        {
            await SeedRoleAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedPeopleAsync(context);
            await SeedInventoriesAsync(context);
            await SeedItemsAsync(context);
            await SeedOperationsAsync(context);
        }
        public static async Task SeedRoleAsync(RoleManager<IdentityRole> roleManager)
        {

            if (await roleManager.Roles.AnyAsync()) return;

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var users = new List<ApplicationUser>
            {
              new ApplicationUser
              {
                FirstName = "Muhammad",
                LastName = "Samy",
                UserName = "muhammed.samy@example.com",
                Email = "muhammed.samy@example.com",
                PhoneNumber = "687443593789",
                HasActiveSubscription = true,
                EmailConfirmed = true
              }
            };

            for (int i = 0; i < users.Count; i++)
            {

                var result = await userManager.CreateAsync(users[i], "P@ssw0rd123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(users[i], "User");
                }


            }
        }



        private static async Task SeedPeopleAsync(ApplicationDbContext context)
        {
            if (await context.People.AnyAsync()) return;

            var users = await context.Users.ToListAsync();
            var personId = 1;

            var persons = new List<Person>
            {
                // Store Managers
                new Person
                {
                    Id = personId++,
                    Name = "Ahmed Hassan",
                    Telephone = "01234567890",
                    Fax = "0223456789",
                    Phone = "01012345678",
                    Email = "ahmed.hassan@store.com",
                    Website = "www.store1.com",
                    Type = "StoreManager",
                    ApplicationUserId = users[0].Id
                },
                new Person
                {
                    Id = personId++,
                    Name = "Sara Mohamed",
                    Telephone = "01234567891",
                    Fax = "0223456790",
                    Phone = "01012345679",
                    Email = "sara.mohamed@store.com",
                    Website = "www.store2.com",
                    Type = "StoreManager",
                    ApplicationUserId = users[0].Id
                },
                // Clients
                new Person
                {
                    Id = personId++,
                    Name = "Mahmoud Ali",
                    Telephone = "01234567892",
                    Fax = "0223456791",
                    Phone = "01012345680",
                    Email = "mahmoud.ali@email.com",
                    Website = "www.client1.com",
                    Type = "Client",
                    ApplicationUserId = users[0].Id
                },
                new Person
                {
                    Id = personId++,
                    Name = "Fatima Ibrahim",
                    Telephone = "01234567893",
                    Fax = "0223456792",
                    Phone = "01012345681",
                    Email = "fatima.ibrahim@email.com",
                    Website = null,
                    Type = "Client",
                    ApplicationUserId = users[0].Id
                },
                new Person
                {
                    Id = personId++,
                    Name = "Omar Hassan",
                    Telephone = "0223456789",
                    Fax = "0223456780",
                    Phone = "01012345678",
                    Email = "omar.hassan@email.com",
                    Website = null,
                    Type = "Client",
                    ApplicationUserId = users[0].Id
                },
                // Suppliers
                new Person
                {
                    Id = personId++,
                    Name = "Hassan Supply Co",
                    Telephone = "01234567894",
                    Fax = "0223456793",
                    Phone = "01012345682",
                    Email = "info@hassansupply.com",
                    Website = "www.hassansupply.com",
                    Type = "Supplier",
                    ApplicationUserId = users[0].Id
                },
                new Person
                {
                    Id = personId++,
                    Name = "Golden Trading",
                    Telephone = "01234567895",
                    Fax = "0223456794",
                    Phone = "01012345683",
                    Email = "sales@goldentrading.com",
                    Website = "www.goldentrading.com",
                    Type = "Supplier",
                    ApplicationUserId = users[0].Id
                }
                ,
                new Person
                {
                    Id = personId++,
                    Name = "Khaled Mostafa",
                    Telephone = "0245678912",
                    Fax = null,
                    Phone = "01234567890",
                    Email = "omar.hassan@email.com",
                    Website = "https://mostafa-supplies.example.com",
                    Type = "Supplier",
                    ApplicationUserId = users[0].Id
                }
            };

            await context.People.AddRangeAsync(persons);
            await context.SaveChangesAsync();
        }

        private static async Task SeedInventoriesAsync(ApplicationDbContext context)
        {

            if (await context.Inventories.AnyAsync()) return;
            var users = await context.Users.ToListAsync();
            var inventoryId = 1;
            var inventories = new List<Inventory>
            {
                new Inventory
                {
                    Id = inventoryId++,
                    Name = "Main Warehouse",
                    Address = "123 Industrial Zone, Cairo",
                    ManagerId = 1, // Ahmed Hassan
                    ApplicationUserId = users[0].Id
                },
                new Inventory
                {
                    Id = inventoryId++,
                    Name = "Branch Store",
                    Address = "456 Commercial Area, Giza",
                    ManagerId = 2,// Sara Mohamed
                    ApplicationUserId = users[0].Id
                },
                new Inventory
                {
                    Id = inventoryId++,
                    Name = "Distribution Center",
                    Address = "789 Logistics Park, Alexandria",
                    ManagerId = 1, // Ahmed Hassan
                    ApplicationUserId = users[0].Id
                },
                new Inventory
                {
                    Id = inventoryId++,
                    Name = "Main Warehouse",
                    Address = "10 Industrial Zone, Cairo",
                    ManagerId = 2, // Sara Mohamed
                    ApplicationUserId = users[0].Id
                },
                new Inventory
                {
                    Id = inventoryId++,
                    Name = "Branch Warehouse - Giza",
                    Address = "25 Pyramids Road, Giza",
                    ManagerId = 1, // Ahmed Hassan
                    ApplicationUserId = users[0].Id
                }
            };

            context.Inventories.AddRange(inventories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedItemsAsync(ApplicationDbContext context)
        {

            if (await context.Items.AnyAsync()) return;
            var users = await context.Users.ToListAsync();
            var itemId = 1;
            var items = new List<Item>
            {
                new Item
                {
                    Id = itemId++,
                    Name = "Laptop Pro",
                    Code = "LAP-PRO-001",
                    Size = "15 inch",
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "USB Flash Drive",
                    Code = "USB-32G-001",
                    Size = "32GB",
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "Printer Paper",
                    Code = "PAP-A4-001",
                    Size = "A4",
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "Desk Chair",
                    Code = "CHR-ERG-001",
                    Size = "Standard",
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "Monitor 24",
                    Code = "MON-24-001",
                    Size = "24 inch",
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                 Name = "Steel Rod",
                  Code = "STL-ROD-001",
                   Size = "12mm" ,
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "Cement Bag",
                    Code = "CEM-BAG-050",
                    Size = "50kg" ,
                    ApplicationUserId = users[0].Id
                },
                new Item
                {
                    Id = itemId++,
                    Name = "Wooden Plank",
                    Code = "WD-PLK-200",
                    Size = "200cm" ,
                    ApplicationUserId = users[0].Id
                }
            };

            context.Items.AddRange(items);
            await context.SaveChangesAsync();
        }


        /* private static async Task SeedInventoryItemsAsync(ApplicationDbContext context)
         {

             if (await context.InventoryItems.AnyAsync()) return;

                 var stock = new List<InventoryItem>
                 {
                     new InventoryItem { InventoryId = 1, ItemId = 1, Quantity = 500 },
                     new InventoryItem { InventoryId = 1, ItemId = 2, Quantity = 300 },
                     new InventoryItem { InventoryId = 1, ItemId = 3, Quantity = 150 }, 
                 };
             context.Set<InventoryItem>().AddRange(stock);
             await context.SaveChangesAsync();
         }*/



        private static async Task SeedOperationsAsync(ApplicationDbContext context)
        {
            if (await context.Operations.AnyAsync()) return;

            var operations = new List<Operation>();
            var inventoryItems = new List<InventoryItem>();
            var users = await context.Users.ToListAsync();
            var operationId = 1;

            // ===== SUPPLY PERMIT OPERATIONS =====
            // Supply Laptops to Main Warehouse from Hassan Supply Co (Supplier)
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 1, 15),
                Quantity = 50,
                Type = "Supply Permit",
                ProDate = new DateTime(2026, 1, 10),
                ExpiryDate = new DateTime(2027, 1, 10),
                ItemId = 1,
                PersonId = 5, // Hassan Supply Co (Supplier)
                InventoryId = 1, // Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Supply USB Drives to Main Warehouse from Golden Trading (Supplier)
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 1, 20),
                Quantity = 200,
                Type = "Supply Permit",
                ProDate = new DateTime(2026, 1, 15),
                ExpiryDate = new DateTime(2027, 7, 15),
                ItemId = 2,
                PersonId = 6, // Golden Trading (Supplier)
                InventoryId = 1,// Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Supply Printer Paper to Main Warehouse from Hassan Supply Co
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 2, 1),
                Quantity = 500,
                Type = "Supply Permit",
                ProDate = new DateTime(2026, 1, 25),
                ExpiryDate = new DateTime(2027, 1, 25),
                ItemId = 3,
                PersonId = 5, // Hassan Supply Co (Supplier)
                InventoryId = 1, // Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Supply Desk Chairs to Main Warehouse from Golden Trading
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 2, 10),
                Quantity = 30,
                Type = "Supply Permit",
                ProDate = new DateTime(2026, 2, 5),
                ExpiryDate = new DateTime(2028, 2, 5),
                ItemId = 4,
                PersonId = 6, // Golden Trading (Supplier)
                InventoryId = 1, // Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Supply Monitors to Branch Store from Hassan Supply Co
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 2, 15),
                Quantity = 40,
                Type = "Supply Permit",
                ProDate = new DateTime(2026, 2, 10),
                ExpiryDate = new DateTime(2027, 2, 10),
                ItemId = 5,
                PersonId = 5, // Hassan Supply Co (Supplier)
                InventoryId = 2,// Branch Store
                ApplicationUserId = users[0].Id
            });

            // ===== EXCHANGE PERMIT OPERATIONS (for Clients) =====
            // Exchange Laptops to Mahmoud Ali (Client)
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 3, 1),
                Quantity = 5,
                Type = "Exchange Permit",
                ProDate = new DateTime(2026, 1, 10),
                ExpiryDate = new DateTime(2027, 1, 10),
                ItemId = 1,
                PersonId = 3, // Mahmoud Ali (Client)
                InventoryId = 1, // Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Exchange USB Drives to Fatima Ibrahim (Client)
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 3, 5),
                Quantity = 20,
                Type = "Exchange Permit",
                ProDate = new DateTime(2026, 1, 15),
                ExpiryDate = new DateTime(2027, 7, 15),
                ItemId = 2,
                PersonId = 4, // Fatima Ibrahim (Client)
                InventoryId = 1,// Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Exchange Printer Paper to Mahmoud Ali (Client)
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 3, 10),
                Quantity = 100,
                Type = "Exchange Permit",
                ProDate = new DateTime(2026, 1, 25),
                ExpiryDate = new DateTime(2027, 1, 25),
                ItemId = 3,
                PersonId = 3, // Mahmoud Ali (Client)
                InventoryId = 1, // Main Warehouse
                ApplicationUserId = users[0].Id
            });

            // Exchange Chairs to Fatima Ibrahim (Client) from Branch Store
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 3, 15),
                Quantity = 3,
                Type = "Exchange Permit",
                ProDate = new DateTime(2026, 2, 5),
                ExpiryDate = new DateTime(2028, 2, 5),
                ItemId = 4,
                PersonId = 4, // Fatima Ibrahim (Client)
                InventoryId = 2, // Branch Store
                ApplicationUserId = users[0].Id
            });

            // Exchange Monitors to Mahmoud Ali (Client) from Branch Store
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 3, 20),
                Quantity = 5,
                Type = "Exchange Permit",
                ProDate = new DateTime(2026, 2, 10),
                ExpiryDate = new DateTime(2027, 2, 10),
                ItemId = 5,
                PersonId = 3, // Mahmoud Ali (Client)
                InventoryId = 2,// Branch Store
                ApplicationUserId = users[0].Id
            });

            // ===== TRANSFER OPERATIONS =====
            // Transfer Laptops from Main Warehouse to Branch Store
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 4, 1),
                Quantity = 10,
                Type = "Transfer from InventoryId=1",
                ItemId = 1,
                PersonId = 1, // Ahmed Hassan (Store Manager)
                InventoryId = 2,
                ProDate = null,
                ExpiryDate = null,
                ApplicationUserId = users[0].Id
            });

            // Transfer USB Drives from Main Warehouse to Distribution Center
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 4, 5),
                Quantity = 50,
                Type = "Transfer from InventoryId=1",
                ItemId = 2,
                PersonId = 1, // Ahmed Hassan (Store Manager)
                InventoryId = 3, // destination inventory (ToInventoryId = 3)
                ProDate = null,
                ExpiryDate = null,
                ApplicationUserId = users[0].Id
            });

            // Transfer Printer Paper from Main Warehouse to Branch Store
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 4, 10),
                Quantity = 200,
                Type = "Transfer from InventoryId=1",
                ItemId = 3,
                PersonId = 2, // Sara Mohamed (Store Manager)
                InventoryId = 2, // destination inventory (ToInventoryId = 2)
                ProDate = null,
                ExpiryDate = null,
                ApplicationUserId = users[0].Id
            });

            operations.Add(new Operation
            {
                Id = operationId++,
                Date = DateTime.UtcNow.Date,
                Quantity = 20,
                Type = "Transfer from InventoryId=2",
                ProDate = null,
                ExpiryDate = null,
                ItemId = 3,
                PersonId = 1, // Ahmed Hassan (Store Manager)
                InventoryId = 1, // destination inventory (ToInventoryId = 1)
                ApplicationUserId = users[0].Id
            });


            // Transfer Chairs from Main Warehouse to Distribution Center
            operations.Add(new Operation
            {
                Id = operationId++,
                Date = new DateTime(2026, 4, 15),
                Quantity = 5,
                Type = "Transfer from InventoryId=1",
                ItemId = 4,
                PersonId = 1, // Ahmed Hassan (Store Manager)
                InventoryId = 3, // To: Distribution Center
                ProDate = null,
                ExpiryDate = null,
                ApplicationUserId = users[0].Id
            });


            context.Operations.AddRange(operations);
            await context.SaveChangesAsync();

            // ===== CALCULATE INVENTORY ITEMS BASED ON OPERATIONS =====
            var inventoryItemDict = new Dictionary<(int InventoryId, int ItemId), int>();

            foreach (var operation in operations)
            {
                var key = (operation.InventoryId, operation.ItemId);

                if (operation.Type == "Supply Permit")
                {
                    // Supply: Add to the inventory
                    if (!inventoryItemDict.ContainsKey(key))
                        inventoryItemDict[key] = 0;
                    inventoryItemDict[key] += operation.Quantity;
                }
                else if (operation.Type == "Exchange Permit")
                {
                    // Exchange: Remove from the inventory
                    if (!inventoryItemDict.ContainsKey(key))
                        inventoryItemDict[key] = 0;
                    inventoryItemDict[key] -= operation.Quantity;
                }
                else if (operation.Type!.StartsWith("Transfer from InventoryId="))
                {
                    // Extract from inventory ID from the operation type
                    var fromInventoryIdStr = operation.Type.Replace("Transfer from InventoryId=", "");
                    if (int.TryParse(fromInventoryIdStr, out int fromInventoryId))
                    {
                        var fromKey = (fromInventoryId, operation.ItemId);

                        // Remove from source inventory
                        if (!inventoryItemDict.ContainsKey(fromKey))
                            inventoryItemDict[fromKey] = 0;
                        inventoryItemDict[fromKey] -= operation.Quantity;

                        // Add to destination inventory
                        if (!inventoryItemDict.ContainsKey(key))
                            inventoryItemDict[key] = 0;
                        inventoryItemDict[key] += operation.Quantity;
                    }
                }
            }

            // Create InventoryItem records for positive quantities
            foreach (var kvp in inventoryItemDict.Where(x => x.Value > 0))
            {
                inventoryItems.Add(new InventoryItem
                {
                    InventoryId = kvp.Key.InventoryId,
                    ItemId = kvp.Key.ItemId,
                    Quantity = kvp.Value
                });
            }

            context.InventoryItems.AddRange(inventoryItems);
            await context.SaveChangesAsync();
        }


    }
}


