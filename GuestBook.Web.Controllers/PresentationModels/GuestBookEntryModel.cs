using System.ComponentModel.DataAnnotations;

using Xtremecode.Infrastructure.WebMvc;

namespace GuestBook.Web.Controllers.PresentationModels
{
    public class GuestBookEntryModel
    {
        public GuestBookEntryModel()
        {
            Name = string.Empty;
            Comment = string.Empty;
        }

        [Required(ErrorMessage = @"Please enter your name.")]
        [MapToModelProperty("GuestName")]
        public string Name { set; get; }

        [DataType(DataType.MultilineText)]
        [MapToModelProperty("Comment")]
        public string Comment { set; get; }

    }
}