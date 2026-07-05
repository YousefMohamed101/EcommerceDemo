using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EcommerceDemo.Singeltons
{
    internal class DatabaseService
    {
        private SQLiteAsyncConnection _userDatabase;

        public User? UserLog = null;


        private DatabaseService(SQLiteAsyncConnection database)
        {
            _userDatabase = database;

        }


        public static DatabaseService Instance { get; private set; }

        public static async Task<DatabaseService> CreateAsync()
        {

            string UserdbPath = Path.Combine(FileSystem.AppDataDirectory, "Users.db3");

            var flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
            var userconnection = new SQLiteAsyncConnection(UserdbPath, flags);

            await userconnection.CreateTableAsync<User>();

            Instance = new DatabaseService(userconnection);

            return Instance;
        }



        public async Task<bool> RegisterUser(User user)
        {


            var isEmailExist = await _userDatabase.Table<User>().Where(u => u.Email == user.Email).FirstOrDefaultAsync();

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


        public async Task<bool> IsUserExist(User user)
        {

            User isUserExist = await _userDatabase.Table<User>().Where(u => u.Name == user.Name && u.Password == user.Password).FirstOrDefaultAsync();
            if (isUserExist != null)
            {
                Debug.WriteLine($"Successfully Found {isUserExist.Name} with email {isUserExist.Email}");
                return true;

            }
            else
            {
                Debug.WriteLine($"No user is found");
                return false;
            }

        }


    }
}
