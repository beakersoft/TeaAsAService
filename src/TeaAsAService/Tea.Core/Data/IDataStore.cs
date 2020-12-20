﻿using System.Threading.Tasks;
using Tea.Core.Domain;

namespace Tea.Core.Data
{
    public interface IDataStore
    {
        Task<User> GetUserBySimpleIdAsync(string Id);     
        Task<User> UpdateBrewCount(string Id);
        Task<User> CreateNewUserAsync(string LocalizationString, string password);
        Task<User> Authenticate(string username, string password);
    }
}
