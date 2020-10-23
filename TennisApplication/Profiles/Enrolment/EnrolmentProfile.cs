using AutoMapper;
using TennisApplication.Dtos.Enrolment;

namespace TennisApplication.Profiles.Enrolment
{
    public class EnrolmentProfile : Profile
    {
        public EnrolmentProfile()
        {
            CreateMap<EnrolmentWriteDto, Models.Enrolment>();
        }
    }
}