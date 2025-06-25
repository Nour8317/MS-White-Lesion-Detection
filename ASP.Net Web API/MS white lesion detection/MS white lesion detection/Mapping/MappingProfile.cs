using AutoMapper;
using MS_white_lesion_detection.DTOs;
using MS_white_lesion_detection.Models;

namespace MS_white_lesion_detection.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<ScanUploadDto, Scan>();
            CreateMap<PredictionResultDto, Prediction>();
            CreateMap<ScanSessionHistoryDto, ScanSession>();
        }
    }

}
