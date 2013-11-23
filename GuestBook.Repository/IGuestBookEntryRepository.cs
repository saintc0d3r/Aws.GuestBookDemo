using System.Collections.Generic;

using GuestBook.Domain.Models;
using Xtremecode.Infrastructure.Persistence;

namespace GuestBook.Repository
{
    public interface IGuestBookEntryRepository : IRepository<GuestBookEntry, long>
    {
        GuestBookEntry FindByName(string guestName);
        IEnumerable<GuestBookEntry> FindLatestRecords(int top);
    }
}
