using Data.EF;
using Data.EF.Repositories.Documents;
using Data.EF.Repositories.References;
using System;

namespace ConsoleDBContextTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var db = new DatabaseContext("test");
            var documentRepository = new IndicationDocumentRepository(db);
            var scaleRepository = new MeterScaleRepostitory(db, documentRepository);
            var meterRepository = new MeterRepository(db, scaleRepository);
            var channelRepository = new ChannelRepository(db, meterRepository);
            var constRepository = new ConstantFlowRepository(db);
            var tariffRepostitory = new TariffGroupRepository(db, constRepository, channelRepository);
            var unitRepostitory = new UnitRepository(db, tariffRepostitory);
            var repostitory = new AgreementRepository(db, unitRepostitory);
            foreach (var agreement in repostitory.GetAll())
            {
                //при all не поднимаем с бд схему
                var calc = repostitory.Get(agreement.Id);
                Console.WriteLine($"{agreement.Name} {calc.СalculationPeriod(new DateTime(2018, 4, 1), new DateTime(2018, 3, 1), new DateTime(2018, 4, 1))}");
            }

            Console.ReadKey();
        }
    }
}