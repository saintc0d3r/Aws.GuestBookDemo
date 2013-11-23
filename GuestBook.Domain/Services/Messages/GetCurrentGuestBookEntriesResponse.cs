using System.Collections.Generic;

using GuestBook.Domain.Models;
using Xtremecode.Infrastructure.Service;

namespace GuestBook.Domain.Services.Messages
{
    public class GetCurrentGuestBookEntriesResponse : ResponseMessageBase
    {
        public IEnumerable<GuestBookEntry> Result { get; set; }
    }
}
