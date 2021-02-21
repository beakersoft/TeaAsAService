using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Core.Impl.Data;
using Microsoft.Extensions.Logging;

namespace Tea.Core.Impl
{
    public class DataStore : IDataStore
    {
        private readonly TeaContext _context;
        private readonly ILogger<DataStore> _log;
        private readonly IPasswordHasher _passwordHasher;

        public DataStore(TeaContext context, IPasswordHasher passwordHasher, ILogger<DataStore> log)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _log = log;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.SimpleId == username);

            if (user == null)
                return null;

            var savedPassword = $"{user.Salt}.{user.Password}";

            if (!_passwordHasher.Check(savedPassword, password))
                return null;

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
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogError($"Exception Creating entity - {ex.Message}", entity, ex);
                return null;
            }
        }

        public async Task<T> UpdateAsync<T>(T entity) where T : class, IBaseDomain
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogError($"Exception Updating entity - {ex.Message}", entity, ex);
                return null;
            }
        }
    }
}
