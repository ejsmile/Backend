using Data.EF.Models;
using Data.EF.Models.Documents;
using Data.EF.Models.References;
using System.Data.Entity;

namespace Data.EF
{
    public class ModelConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            ConfigureObject(modelBuilder);
            ConfigureTariff(modelBuilder);
            ConfigureMeterType(modelBuilder);
            ConfigureBillingSchema(modelBuilder);
            ConfigureDocument(modelBuilder);
        }

        private static void ConfigureDocument(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentDTO>();
            modelBuilder.Entity<PaymentDocumentDTO>();
            modelBuilder.Entity<IndicationDocumentDTO>();
        }

        private static void ConfigureTariff(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TariffDTO>();
        }

        private static void ConfigureMeterType(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterTypeDTO>();
        }

        private static void ConfigureObject(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObjectDTO>();
            modelBuilder.Entity<LinkObjectsDTO>();
        }

        private static void ConfigureBillingSchema(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgreementDTO>();
            modelBuilder.Entity<UnitDTO>();
            modelBuilder.Entity<TariffGroupDTO>();
            modelBuilder.Entity<ChannelDTO>();
            modelBuilder.Entity<ConstantFlowDTO>();
            modelBuilder.Entity<MeterDTO>();
            modelBuilder.Entity<MeterScaleDTO>();
        }
    }
}