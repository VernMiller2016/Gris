using AutoMapper;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using GRis.ViewModels.PaySource;
using GRis.ViewModels.Program;
using GRis.ViewModels.Server;
using GRis.ViewModels.ServerTimeEntry;

namespace GRis.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ServerTimeEntry, ServerTimeEntriesMonthlyReportEntity>()
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Server.FullName))
                .ForMember(dest => dest.ServerVendorId, opt => opt.MapFrom(src => src.Server.VendorId))
                .ForMember(dest => dest.PaysourceVendorId, opt => opt.MapFrom(src => src.PaySource.VendorId))
                .ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src => src.PaySource.Program.Id))
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.PaySource.Program.Name))
                ;

                #region Program

                cfg.CreateMap<Program, ProgramDetailsViewModel>()
                ;

                cfg.CreateMap<ProgramAddViewModel, Program>()
                ;

                cfg.CreateMap<Program, ProgramEditViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.PaySources, opt => opt.Ignore())
                ;

                #endregion Program

                #region Server

                cfg.CreateMap<Server, ServerDetailsViewModel>()
                ;

                cfg.CreateMap<ServerAddViewModel, Server>()
                ;

                cfg.CreateMap<Server, ServerEditViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.VendorId, opt => opt.Ignore())
                ;

                #endregion Server

                #region PaySource

                cfg.CreateMap<PaySource, PaySourceDetailsViewModel>()
                ;

                cfg.CreateMap<PaySourceAddViewModel, PaySource>()
                ;

                cfg.CreateMap<PaySource, PaySourceEditViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.VendorId, opt => opt.Ignore())
                ;

                #endregion PaySource

                #region PaySource

                cfg.CreateMap<ServerTimeEntry, ServerTimeEntryDetailsViewModel>()
                ;

                #endregion PaySource
            });
        }
    }
}