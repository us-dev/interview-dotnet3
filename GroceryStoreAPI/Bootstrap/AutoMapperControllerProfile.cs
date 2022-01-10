using AutoMapper;
using GroceryStore.Models;
using GroceryStoreAPI.ApiRequestModel;

namespace GroceryStoreAPI.Bootstrap
{
    /// <summary>
    /// Automapper profile for controller layer.
    /// </summary>
    public class ControllerAutoMapperProfile : Profile
    {
        /// <summary>
        /// Constructor for <see cref="ControllerAutoMapperProfile"/>
        /// </summary>
        public ControllerAutoMapperProfile()
        {
            CreateMap<CustomerRequestModel, CustomerModel>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
