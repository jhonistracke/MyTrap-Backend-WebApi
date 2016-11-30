using MyTrap.Model.Mobile.Result;

namespace MyTrap.Model.Mobile.Request
{
    public class ImageRequest
    {
        public string Url { get; set; }

        public string OriginUrl { get; set; }

        public ImageRequest() { }

        public ImageRequest(ImageResult response)
        {
            Url = response.Url;
        }
    }
}