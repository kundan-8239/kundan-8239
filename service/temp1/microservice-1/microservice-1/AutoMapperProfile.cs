using AutoMapper;

namespace microservice_1
{
    /// <summary>
    /// Define the AutoMapper mappings.
    /// For large applications it is fine to have multiple files, typically in separate namespaces.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Map Thing requests (for when we have a Thing domain object)
            //CreateMap<CreateThingRequest, Thing>();
            //CreateMap<UpdateThingRequest, Thing>();
        }
    }
}
