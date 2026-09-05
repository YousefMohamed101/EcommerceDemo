using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EcommerceDemo.Singeltons
{
    internal class DatabaseService
    {
        private SQLiteAsyncConnection _userDatabase;
        private SQLiteAsyncConnection _productDatabase;
        private SQLiteAsyncConnection _cartDatabase;

        public User? UserLog = null;


        private DatabaseService(SQLiteAsyncConnection database, SQLiteAsyncConnection productDatabase, SQLiteAsyncConnection cartDatabase)
        {
            _userDatabase = database;
            _productDatabase  = productDatabase;
            _cartDatabase =  cartDatabase;
        }


        public static DatabaseService Instance { get; private set; }

        public static async Task<DatabaseService> CreateAsync()
        {

            string userDbPath = Path.Combine(FileSystem.AppDataDirectory, "Users.db3");
            string productDbPath = Path.Combine(FileSystem.AppDataDirectory, "ProductsData.sqlite");
            string cartDbPath = Path.Combine(FileSystem.AppDataDirectory, "CartItems.db3");
            const SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
            SQLiteAsyncConnection userConnection = new SQLiteAsyncConnection(userDbPath, flags);
            SQLiteAsyncConnection productConnection = new SQLiteAsyncConnection(productDbPath, flags);
            SQLiteAsyncConnection cartConnection = new SQLiteAsyncConnection(cartDbPath, flags);
            await userConnection.CreateTableAsync<User>();
            await productConnection.CreateTableAsync<Product>();
            await cartConnection.CreateTableAsync<CartItem>();
            
            Instance = new DatabaseService(userConnection,productConnection,cartConnection);
            await Instance.InitializeProductList();
            return Instance;
        }


        public async Task InitializeProductList() {
            
            int currentCount = await _productDatabase.Table<Product>().CountAsync();
            if(currentCount > 0) {
                Console.WriteLine("Products already seeded, skipping fetch.");
                return;
            }

            try {

                using HttpClient http = new HttpClient();
                
                
                string json =  await http.GetStringAsync("https://dummyjson.com/products?limit=0");

                ProductFetching response = JsonSerializer.Deserialize<ProductFetching>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (response?.Products == null || response.Products.Count == 0)
                {
                    Console.WriteLine("No products returned from API.");
                    return;
                }

                List<Product> products = response.Products;
                
                await _productDatabase.InsertAllAsync(products);
                Console.WriteLine($"Seeded {products.Count} products.");

            } catch(Exception e) {
                Console.WriteLine($"Product seeding failed: {e.Message}");
            }
            
        }
        
        
        public async Task<bool> RegisterUser(User user)
        {


            User? isEmailExist = await _userDatabase.Table<User>().Where(u => u.Email == user.Email).FirstOrDefaultAsync();

            if (isEmailExist != null)
            {
                throw new InvalidOperationException("An account with that Email Exist");
            }


            await _userDatabase.InsertAsync(user);

            User isUserExist = await _userDatabase.Table<User>().Where(u => u.Email == user.Email).FirstOrDefaultAsync();

            if (isUserExist == null)
            {
                throw new InvalidOperationException("Failed to Insert User");
            }
            else
            {
                Debug.WriteLine($"Successfully inserted {user.Name} with email {user.Email}");
                return true;
            }
        }


        public async Task<User?> IsUserExist(User user)
        {

            User isUserExist = await _userDatabase.Table<User>().Where(u => u.Name == user.Name && u.Password == user.Password).FirstOrDefaultAsync();
            if (isUserExist != null)
            {
                Debug.WriteLine($"Successfully Found {isUserExist.Name} with email {isUserExist.Email}");
                return isUserExist;

            }
            
            Debug.WriteLine($"No user is found");
            return null;
            

        }

        public async Task<List<Product>> GetProducts() {
            return await _productDatabase.Table<Product>().ToListAsync();
        }

        public async Task UpdateUser(User user) {
            
            User? isExist = await IsUserExist(UserLog);

            if(isExist == null) {
                Console.WriteLine("No user found");
                return;
            }
            
            int updateCount = await  _userDatabase.UpdateAsync(user);
            if(updateCount == 0) {
                Console.WriteLine("Update failed");
                return;
            }
            Console.WriteLine("Update Successful");
            
            UserLog = user;
            
        }

        public async Task AddToCart(CartItem item) {
            
            User? isUserExist = await _userDatabase.Table<User>().Where(u => u.Id == item.UserId).FirstOrDefaultAsync();
            Product? isProductExist = await _productDatabase.Table<Product>().Where(p=> p.Id == item.ItemId).FirstOrDefaultAsync();
            if(isUserExist == null ) {
                Console.WriteLine("Couldn't find user");
                return;
            }

            if(isProductExist == null) {
                Console.WriteLine("Couldn't find product");
                return;
            }
            item.Name = isProductExist.Title;
            item.ImagePath = isProductExist.Thumbnail;
            item.Price = isProductExist.Price;
            
           CartItem? alreadyInCart = await _cartDatabase.Table<CartItem>().Where(c => c.ItemId == item.ItemId && c.UserId == isUserExist.Id).FirstOrDefaultAsync();

           if(alreadyInCart != null) {
               alreadyInCart.Count += item.Count;
               int updatedCount = await _cartDatabase.UpdateAsync(alreadyInCart);
               _productDatabase.UpdateAsync(isProductExist);
               return;
           }
           
            
            int addedCount = await _cartDatabase.InsertAsync(item);
            Console.WriteLine(addedCount == 0 ? "Add failed" : "Add Successful");
            isProductExist.Stock -= item.Count;
            _productDatabase.UpdateAsync(isProductExist);
        }
        
        public async Task RemoveFromCart(CartItem item) {
            User? isUserExist = await _userDatabase.Table<User>().Where(u => u.Id == item.UserId).FirstOrDefaultAsync();
            CartItem? isCartExist = await _cartDatabase.Table<CartItem>().Where(c => c.ItemId == item.ItemId && c.UserId == isUserExist.Id).FirstOrDefaultAsync();
            Product? isProductExist = await _productDatabase.Table<Product>().Where(p=> p.Id == item.ItemId).FirstOrDefaultAsync();
            if(isCartExist == null ) {
                Console.WriteLine("Couldn't find cart item");
                return;
            }

            if(isProductExist == null) {
                Console.WriteLine("Couldn't find product");
                return;
            }
            
            int addedCount = await _cartDatabase.DeleteAsync(item);
            Console.WriteLine(addedCount == 0 ? "Add failed" : "Add Successful");
            isProductExist.Stock += item.Count;
            _productDatabase.UpdateAsync(isProductExist);
        }

        public async Task<List<CartItem>> GetCartItems() {
            return await _cartDatabase.Table<CartItem>().Where(c=>c.UserId == UserLog.Id).ToListAsync(); 
        }

        public async Task UpdateUserWallet(decimal amount) {
            
            
            User? isUserExist = await _userDatabase.Table<User>().Where(u => u.Id == UserLog.Id).FirstOrDefaultAsync(); 
            if(isUserExist == null ) {
                Console.WriteLine("Couldn't find user");
                return;
            }
            
            isUserExist.Balance += amount;
            _userDatabase.UpdateAsync(isUserExist);
            UserLog = isUserExist;
            
        }

        public async Task Checkout(List<CartItem> items) {
            
            float totalPrice = items.Sum(cartItem => cartItem.TotalPrice);

            if(UserLog != null && totalPrice > (float)UserLog.Balance) {
                throw new Exception("Insufficent Funds");
                
            }
            UserLog.Balance -= (decimal)totalPrice;
            await _userDatabase.UpdateAsync(UserLog);
            Console.WriteLine("saved " + UserLog.Balance);
            foreach(CartItem cartItem in items) {
                User? isUserExist = await _userDatabase.Table<User>().Where(u => u.Id == cartItem.UserId).FirstOrDefaultAsync();
                CartItem? isCartExist = await _cartDatabase.Table<CartItem>().Where(c => c.ItemId == cartItem.ItemId && c.UserId == isUserExist.Id).FirstOrDefaultAsync();
                Product? isProductExist = await _productDatabase.Table<Product>().Where(p=> p.Id == cartItem.ItemId).FirstOrDefaultAsync();

                if(isUserExist == null) {
                    throw new Exception("User doesn't exist");
                }
        
                if(isCartExist == null) {
                    throw new Exception("Cart item doesn't exist");
                }

                if(isProductExist == null) {
                    throw new Exception("Product doesn't exist");
                }
                
                
                isProductExist?.Stock -= cartItem.Count;
                await _productDatabase.UpdateAsync(isProductExist);
                await _cartDatabase.DeleteAsync(isCartExist);

            }
            
            
            
            
            
            
        }

    }
}
