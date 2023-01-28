using System.Collections.Generic;
using WyCash.Domain;

namespace WyCash.Apllication.Repositories
{
    public interface IFinancialTitleRepository
    {
        IEnumerable<FinancialTitle> GetFinancialTitleFromCilentId(long clientId);
    }
}
