 using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Core.Impl.Data;

namespace Tea.Core.Impl
{
    public class DataStore : IDataStore
    {
        private readonly TeaContext _context;

        public DataStore(TeaContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.SimpleId == username && x.Password == password);

            if (user == null)
                return null;
            
            return user;
        }

        public async Task<User> GetUserBySimpleIdAsync(string Id)
        {
            return await _context.Users
                .Include(user => user.History)
                .FirstOrDefaultAsync(x => x.SimpleId == Id);
        }

        public async Task<T> CreateAsync<T>(T entity) where T : class, IBaseDomain
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync<T>(T entity) where T : class, IBaseDomain
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
