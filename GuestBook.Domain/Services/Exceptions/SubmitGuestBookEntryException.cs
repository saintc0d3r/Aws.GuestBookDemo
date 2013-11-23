using System;

namespace GuestBook.Domain.Services.Exceptions
{
    public class SubmitGuestBookEntryException : Exception
    {
        private const string ERROR_MESSAGE = "Unable to submit the entered data. Please check your connection's status.";

        public SubmitGuestBookEntryException(Exception innerException) : base(ERROR_MESSAGE, innerException) { }
    }
}
