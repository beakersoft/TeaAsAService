using System.Threading.Tasks;
using Tea.Core.Domain;

namespace Tea.Core
{
    public interface IRoundService
    {
        Task<bool> UpdateExistingRoundAsync(Round round, string UserGettingRound, string roundNotes);
    }
}
