﻿using System.Threading.Tasks;
using Tea.Core.Domain;

namespace Tea.Core.Data
{
    public interface IDataStore
    {
        Task<User> GetUserBySimpleIdAsync(string Id);     
        Task<User> AuthenticateAsync(string username, string password);
        Task<T> CreateAsync<T>(T entity) where T : class, IBaseDomain;
        Task<T> UpdateAsync<T>(T entity) where T : class, IBaseDomain;
    }
}
