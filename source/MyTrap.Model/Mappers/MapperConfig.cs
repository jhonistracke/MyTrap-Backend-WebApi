using AutoMapper;
using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Model.ViewModel.Request;
using MyTrap.Model.ViewModel.Result;

namespace MyTrap.Model.Mappers
{
    public class MapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(map =>
            {
                map.AddProfile<UserApiToRequestMapper>();
                map.AddProfile<UserRequestToApiMapper>();
                map.AddProfile<UserResponseToApiMapper>();
                map.AddProfile<ImageApiToImageRequestMapper>();
                map.AddProfile<ImageRequestToApiMapper>();
                map.AddProfile<ImageResponseToApiMapper>();
                map.AddProfile<PositionApiToRequestMapper>();
                map.AddProfile<PlantedTrapApiToRequestMapper>();
                map.AddProfile<UserToRequest>();
                map.AddProfile<ImageToRequest>();
                map.AddProfile<UserTrapToRequestMapper>();
                map.AddProfile<UserTrapResponseToApiMapper>();
                map.AddProfile<PlantedTrapResponseToApiMapper>();
                map.AddProfile<AvailableTrapResponseToApiMapper>();
                map.AddProfile<BuyIntentApiToRequestMapper>();
                map.AddProfile<BuyIntentResponseToApiMapper>();
                map.AddProfile<BuyIntentResponseToRequestMapper>();
                map.AddProfile<PlantedTrapResponseToRequestMapper>();
                map.AddProfile<PositionResponseToRequestMapper>();
            });
        }

        private class UserApiToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserApiRequest, UserRequest>().ReverseMap();
            }
        }

        private class UserRequestToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserRequest, UserApiRequest>().ReverseMap();
            }
        }

        private class UserResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserResult, UserApiResult>().ReverseMap();
            }
        }

        private class ImageApiToImageRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ImageApiRequest, ImageRequest>().ReverseMap();
            }
        }

        private class ImageRequestToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ImageRequest, ImageApiRequest>().ReverseMap();
            }
        }

        private class ImageResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ImageResult, ImageApiResult>().ReverseMap();
            }
        }

        private class PositionApiToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<PositionApiRequest, PositionRequest>().ReverseMap();
            }
        }

        private class PlantedTrapApiToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ArmedTrapApiRequest, ArmedTrapRequest>().ReverseMap();
            }
        }

        private class UserToRequest : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserResult, UserRequest>().ReverseMap();
            }
        }

        private class ImageToRequest : Profile
        {
            protected override void Configure()
            {
                CreateMap<ImageResult, ImageRequest>().ReverseMap();
            }
        }

        private class UserTrapToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserTrapResult, UserTrapRequest>().ReverseMap();
            }
        }

        private class UserTrapResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<UserTrapResult, UserTrapApiResult>().ReverseMap();
            }
        }

        private class PlantedTrapResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ArmedTrapResult, ArmedTrapApiResult>().ReverseMap();
            }
        }

        private class AvailableTrapResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<AvailableTrapResult, AvailableTrapApiResult>().ReverseMap();
            }
        }

        private class BuyIntentApiToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<BuyIntentApiRequest, BuyIntentRequest>().ReverseMap();
            }
        }

        private class BuyIntentResponseToApiMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<BuyIntentResult, BuyIntentApiResult>().ReverseMap();
            }
        }

        private class BuyIntentResponseToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<BuyIntentResult, BuyIntentRequest>().ReverseMap();
            }
        }

        private class PlantedTrapResponseToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<ArmedTrapResult, ArmedTrapRequest>().ReverseMap();
            }
        }

        private class PositionResponseToRequestMapper : Profile
        {
            protected override void Configure()
            {
                CreateMap<PositionResult, PositionRequest>().ReverseMap();
            }
        }
    }
}