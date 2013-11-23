using System.Web;

namespace GuestBook.Domain.Services.Messages
{
    public class SubmitGuestBookEntryRequest
    {
        public string Name { get; set; }

        public string Comment { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }
    }
}
