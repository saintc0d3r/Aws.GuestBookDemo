using GuestBook.Domain.Services.Messages;

namespace GuestBook.Domain.Services
{
    public interface IGuestBookService
    {
        SubmitGuestBookEntryResponse SubmitGuestBookEntry(SubmitGuestBookEntryRequest request);
        GetCurrentGuestBookEntriesResponse GetCurrentGuestBookEntries(GetCurrentGuestBookEntriesRequest request);
    }
}
