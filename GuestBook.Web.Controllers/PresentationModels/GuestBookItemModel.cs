using Xtremecode.Infrastructure.WebMvc;

namespace GuestBook.Web.Controllers.PresentationModels
{
    public class GuestBookItemModel : GuestBookEntryModel
    {
        [MapToModelProperty("ThumbnailUrl")]
        public string ThumbnailUrl { set; get; }

        [MapToModelProperty("CreateDate")]
        public System.DateTime MessageTimestamp { set; get; }

        [MapToModelProperty("Photo")]
        public byte[] Photo { set; get; }
    }
}