using MyTrap.Model.Mobile.Request;

namespace MyTrap.Model.Mobile.Result
{
    public class ImageResult
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string OriginUrl { get; set; }

        public ImageResult() { }

        public ImageResult(ImageRequest request)
        {
            Url = request.Url;
            OriginUrl = request.OriginUrl;
        }
    }
}