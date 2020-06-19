using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Entity;
using Tea.Core.Impl.Data;

namespace Tea.Core.Impl
{
    public class DataStore : IDataStore
    {
        private TeaContext _context;

        public DataStore(TeaContext context)
        {
            _context = context;
        }

        public async Task<User> CreateNewUserAsync(string LocalizationString, string password)
        {            
            var user = User.CreateNewUser(LocalizationString, password);
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User> UpdateBrewCount(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.SimpleId == id);

            if (user == null)
                return null;


            //WE NEED TO MAKE SURE WE UPDATE THE HISTORY AND RESET THE COUNT IF WE HAVE ROLLED A DAY


            user.CurrentDayCount++;
            await _context.SaveChangesAsync();




            return user;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.SimpleId == username && x.Password == password);

            if (user == null)
                return null;
            
            return user;
           
        }

        public Task<User> GetUserAsync(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string Id)
        {
            throw new NotImplementedException();
        }

        
    }
}
