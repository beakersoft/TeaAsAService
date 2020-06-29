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

        public async Task<User> UpdateBrewCount(string Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.SimpleId == Id);

            if (user == null)
                return null;

            if (user.CurrentDayCount > 0 && (DateTime.UtcNow.Subtract(user.LastTimeUtc).TotalDays >= 1))
            {
                var histEntry = user.CreateHistoryEntry();
                _context.Add(histEntry);
            }

            user.CurrentDayCount++;
            user.LastTimeUtc = DateTime.UtcNow;
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

        public async Task<User> GetUserAsync(string Id)
        {
            return await _context.Users
                .Include(user => user.History)
                .FirstOrDefaultAsync(x => x.SimpleId == Id);
        }
    }
}
