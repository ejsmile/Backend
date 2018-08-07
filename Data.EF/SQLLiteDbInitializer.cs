using Data.EF.Models;
using Data.EF.Models.References;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;

namespace Data.EF
{
    public class SQLLiteDbInitializer : SqliteDropCreateDatabaseWhenModelChanges<DatabaseContext>
    {
        public SQLLiteDbInitializer(DbModelBuilder modelBuilder)
            : base(modelBuilder, typeof(CustomHistory))
        { }

        protected override void Seed(DatabaseContext context)
        {
            // Here you can seed your core data if you have any.
            var baseTariff = new TariffDTO()
            {
                Name = "base Tariff",
                Price = 4.0
            };
            var dayTariff = new TariffDTO()
            {
                Name = "Day Tariff",
                Price = 4.2
            };
            var nightTariff = new TariffDTO()
            {
                Name = "Night Tariff",
                Price = 4.2
            };

            context.TariffDTOs.Add(baseTariff);
            context.TariffDTOs.Add(dayTariff);
            context.TariffDTOs.Add(nightTariff);

            var meterType = new MeterTypeDTO()
            {
                ManufacturerName = "Meteor",
                ModelName = "Signal 01",
                CalibrationIntervals = 10
            };

            var fistAgreement = new AgreementDTO()
            {
                Name = "First",
                Number = "01-01-2018",
                BegDate = new DateTime(2018, 3, 1),
                EndDate = new DateTime(2020, 3, 1),
                Deleted = false
            };

            var secondAgr = new AgreementDTO()
            {
                Name = "Second (deleted)",
                Number = "01-01-2018",
                BegDate = new DateTime(2018, 3, 1),
                EndDate = new DateTime(2020, 3, 1),
                Deleted = true
            };

            context.AgreementDTOs.Add(fistAgreement);
            context.AgreementDTOs.Add(secondAgr);

            var unitHouse = new UnitDTO()
            {
                Name = "House",
                Adress = "Moscow 1"
            };

            var unitStore = new UnitDTO()
            {
                Name = "Store",
                Adress = "Novgorod 1"
            };
            context.UnitDTOs.Add(unitHouse);
            context.UnitDTOs.Add(unitStore);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = fistAgreement,
                Childen = unitHouse,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0,
            });
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = fistAgreement,
                Childen = unitStore,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 0.5
            });

            var tariffGroupHouse = new TariffGroupDTO()
            {
                Name = "Base Tariff House",
                Tariff = baseTariff
            };
            var tariffGroupStore = new TariffGroupDTO()
            {
                Name = "Base Tariff Store",
                Tariff = baseTariff
            };

            context.TariffGroupDTOs.Add(tariffGroupHouse);
            context.TariffGroupDTOs.Add(tariffGroupStore);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = unitHouse,
                Childen = tariffGroupHouse,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = unitStore,
                Childen = tariffGroupStore,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });

            var channelHouse = new ChannelDTO()
            {
                Name = "Base Channel",
                VoltageLevelId = 0
            };
            context.ChannelDTOs.Add(channelHouse);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = tariffGroupHouse,
                Childen = channelHouse,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });

            var consFlowStore = new ConstantFlowDTO()
            {
                Name = "Signal system",
                VoltageLevelId = 0,
                Сonsumption = 10.0
            };
            context.ConstantFlowDTOs.Add(consFlowStore);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = tariffGroupStore,
                Childen = consFlowStore,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });

            var meterHouse = new MeterDTO()
            {
                Name = "ground flow",
                CountScale = 6,
                PublicNumber = "34785",
                SerialNumber = "2016-04-23-02-2349"
            };
            context.MeterDTOs.Add(meterHouse);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = channelHouse,
                Childen = meterHouse,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });

            var scaleHouse = new MeterScaleDTO()
            {
                Name = "All day",
                Dimension = 6,
                ZoneOfDayId = 0
            };

            context.MeterScaleDTOs.Add(scaleHouse);
            context.LinkObjectsDTOs.Add(new LinkObjectsDTO()
            {
                Owner = meterHouse,
                Childen = scaleHouse,
                BeginDate = new DateTime(2018, 1, 1),
                EndDate = new DateTime(2079, 1, 1),
                Factore = 1.0
            });

            context.IndicationDocumentDTOs.Add(new Models.Documents.IndicationDocumentDTO()
            {
                CreateDate = new DateTime(2018, 1, 1),
                ReceiptDate = new DateTime(2018, 1, 1),
                Meter = meterHouse,
                Scale = scaleHouse,
                Indication = 1012
            });

            context.IndicationDocumentDTOs.Add(new Models.Documents.IndicationDocumentDTO()
            {
                CreateDate = new DateTime(2018, 2, 1),
                ReceiptDate = new DateTime(2018, 2, 1),
                Meter = meterHouse,
                Scale = scaleHouse,
                Indication = 1057
            });

            context.IndicationDocumentDTOs.Add(new Models.Documents.IndicationDocumentDTO()
            {
                CreateDate = new DateTime(2018, 3, 1),
                ReceiptDate = new DateTime(2018, 3, 1),
                Meter = meterHouse,
                Scale = scaleHouse,
                Indication = 1135
            });

            context.IndicationDocumentDTOs.Add(new Models.Documents.IndicationDocumentDTO()
            {
                CreateDate = new DateTime(2018, 4, 1),
                ReceiptDate = new DateTime(2018, 4, 1),
                Meter = meterHouse,
                Scale = scaleHouse,
                Indication = 1208
            });
        }
    }
}