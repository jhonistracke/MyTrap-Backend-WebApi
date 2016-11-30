using MyTrap.Framework.Base;
using MyTrap.Model.Mobile.Request;

namespace MyTrap.Business.Mobile.Contracts
{
    public interface IBlobBusiness : IBaseBusiness
    {
        ImageRequest InsertUserImage(ImageRequest image);
    }
}