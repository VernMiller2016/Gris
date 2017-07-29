using AutoMapper;
using Gris.Application.Core.Contracts.Reports;
using Gris.Domain.Core.Models;
using GRis.ViewModels.Element;
using GRis.ViewModels.PaySource;
using GRis.ViewModels.Program;
using GRis.ViewModels.Server;
using GRis.ViewModels.ServerAvailableHourModels;
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
                .ForMember(dest => dest.PaysourceDescription, opt => opt.MapFrom(src => src.PaySource.Description))
                .ForMember(dest => dest.ProgramId, opt => opt.MapFrom(src => src.Program.Id))
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.Program.Name))
                .ForMember(dest => dest.ServerCategoryName, opt => opt.MapFrom(src => src.Server.Category.Name))
                .ForMember(dest => dest.ServerCategoryId, opt => opt.MapFrom(src => src.Server.Category.Id))
                ;

                #region Program

                cfg.CreateMap<Program, ProgramDetailsViewModel>()
                .MaxDepth(1)
                ;

                cfg.CreateMap<ProgramAddViewModel, Program>()
                ;

                cfg.CreateMap<Program, ProgramEditViewModel>()
                .ForMember(dest => dest.PaySources, opt => opt.Ignore());

                cfg.CreateMap<ProgramEditViewModel, Program>()
              .ForMember(dest => dest.PaySources, opt => opt.Ignore());

                #endregion Program

                #region Server

                cfg.CreateMap<Server, ServerDetailsViewModel>()
                .ForMember(dest => dest.ElementDisplayName, opt => opt.MapFrom(src => src.Element.DisplayName))
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

                #region ServerTimeEntry

                cfg.CreateMap<ServerTimeEntry, ServerTimeEntryDetailsViewModel>()
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Server.FullName))
                .ForMember(dest => dest.ServerId, opt => opt.MapFrom(src => src.ServerId))
                .ForMember(dest => dest.PaySourceId, opt => opt.MapFrom(src => src.PaySourceId))
                .ForMember(dest => dest.PaySourceDescription, opt => opt.MapFrom(src => src.PaySource.Description))
                .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.Program.Name))
                ;

                cfg.CreateMap<ServerTimeEntryAddViewModel, ServerTimeEntry>()
                ;

                cfg.CreateMap<ServerTimeEntry, ServerTimeEntryEditViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.Server, opt => opt.Ignore())
                .ForMember(dest => dest.PaySource, opt => opt.Ignore())
                .ForMember(dest => dest.Program, opt => opt.Ignore())
                ;

                #endregion ServerTimeEntry

                #region ServerAvailableHour

                cfg.CreateMap<ServerAvailableHour, ServerAvailableHourDetailsViewModel>()
                .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.Server.FullName))
                ;

                cfg.CreateMap<ServerAvailableHourAddViewModel, ServerAvailableHour>()
                .ForMember(dest => dest.ServerId, opt => opt.Ignore())
                ;

                #endregion ServerAvailableHour

                #region Element

                cfg.CreateMap<Element, ElementDetailsViewModel>()
                ;

                cfg.CreateMap<ElementAddViewModel, Element>()
                ;

                cfg.CreateMap<Element, ElementEditViewModel>()
                .ReverseMap()
                ;

                #endregion Element

                #region ServerSalaryMonthlyReport
                cfg.CreateMap<ServerSalaryReportEntity, ServerSalaryReportViewModel>()
               .ForMember(dest => dest.ServerName, opt => opt.MapFrom(src => src.ORMSTRNM))
               .ForMember(dest => dest.GpEmpNumber, opt => opt.MapFrom(src => src.ORMSTRID))
               //.ForMember(dest => dest.CreditAmount, opt => opt.MapFrom(src => src.CRDTAMNT))
               //.ForMember(dest => dest.DebitAmount, opt => opt.MapFrom(src => src.DEBITAMT))
               //.ForMember(dest => dest.AccountDescription , opt => opt.MapFrom(src =>  src.ACTDESCR))
               ;
                #endregion
            });
        }
    }
}