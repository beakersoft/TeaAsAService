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

        public async Task<User> CreateNewUserAsync(string LocalizationString)
        {
            var userId = Guid.NewGuid();
            var simpleId = Convert.ToBase64String(userId.ToByteArray());

            var user = new User
            {
                Id = userId,
                Localization = LocalizationString,
                SimpleId = simpleId,
                CurrentDayCount = 1,
                LastTimeUtc = DateTime.UtcNow
            };

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
