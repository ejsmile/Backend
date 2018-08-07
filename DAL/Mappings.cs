using AutoMapper;
using AutoMapper.Configuration;
using Data.EF.Models.Documents;
using Data.EF.Models.References;
using Domain.Aggregates.Document;
using Domain.Aggregates.References;

namespace DAL
{
    public static class Mappings
    {
        public static void Init()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.CreateMap<AgreementAggregate, AgreementDTO>();
            cfg.CreateMap<UnitAggregate, UnitDTO>();
            cfg.CreateMap<ChannelAggregate, ChannelDTO>();
            cfg.CreateMap<ConstantFlowAggregate, ConstantFlowDTO>();
            cfg.CreateMap<MeterAggregate, MeterDTO>();
            cfg.CreateMap<MeterScaleAggregate, MeterScaleDTO>();
            cfg.CreateMap<MeterTypeAggregate, MeterTypeDTO>();
            cfg.CreateMap<TariffAggregate, TariffDTO>();
            cfg.CreateMap<MeterTypeAggregate, MeterTypeDTO>();

            cfg.CreateMap<IndicationDocumentAggregate, IndicationDocumentDTO>();

            Mapper.Initialize(cfg);
        }
    }
}