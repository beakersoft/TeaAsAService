using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;

namespace Tea.Core
{
    public interface IRoundService
    {
        Task<bool> UpdateExistingRoundAsync(Round round, IDataStore dataStore, string UserGettingRound, string roundNotes);
    }
}
