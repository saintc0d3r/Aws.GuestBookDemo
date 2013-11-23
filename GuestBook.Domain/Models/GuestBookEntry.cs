using System;
using System.ComponentModel.DataAnnotations;

using Xtremecode.Infrastructure.Persistence;

namespace GuestBook.Domain.Models
{
    public class GuestBookEntry : EntityBase
    {
        public GuestBookEntry()
        {
            Id = DateTime.MaxValue.Ticks - DateTime.Now.Ticks;
        }

        public long Id { get; set; }

        [Required(ErrorMessage="Guest's name should not be empty.")]
        public string GuestName { get; set; }

        [Required(ErrorMessage = "Comment should not be empty.")]
        public string Comment { get; set; }

        public string PhotoUrl { get; set; }

        public byte[] Photo { get; set; }
    }
}
