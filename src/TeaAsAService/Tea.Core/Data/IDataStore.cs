using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Tea.Core.Entity;

namespace Tea.Core.Data
{
    public interface IDataStore
    {
        Task<User> GetUserAsync(Guid Id);       //pass the actuall guid
        Task<User> GetUserAsync(string Id);     //pass the sortname
        Task<User> UpdateBrewCount(string id);
        Task<User> CreateNewUserAsync(string LocalizationString, string password);
        Task<User> Authenticate(string username, string password);


        
        //create history
        //update user
        //create round
        //update round


    }
}
