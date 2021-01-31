using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

            return user;
        }

        public async Task<T> GetAsync<T>(Guid id) where T : class, IBaseDomain
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<User> GetUserBySimpleIdAsync(string id)
        {
            return await _context.Users
                .Include(user => user.History)
                .FirstOrDefaultAsync(x => x.SimpleId == id);
        }

        public async Task<T> CreateAsync<T>(T entity) where T : class, IBaseDomain
        {
            await _context.Set<T>().AddAsync(entity);
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
