using AutoFixture;
using Domain.Aggregates.Document;
using Domain.Aggregates.References;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Domain.Test
{
    [TestClass]
    public class MeterScaleAggregateTests
    {
        private readonly Fixture randomizer = new Fixture();

        [TestMethod]
        public void СalculationPeriodTest_Indecation_OverDimenshion()
        {
            //arrange
            var begDate = randomizer.Create<DateTime>().Date;
            var endDate = begDate.AddMonths(1);

            var aggregate = new MeterScaleAggregate(
                randomizer.Create<long>(),
                randomizer.Create<long>(),
                randomizer.Create<string>(),
                randomizer.Create<Const.ZoneOfDay>(),
                5,
                begDate,
                endDate,
                false,
                false
                );
            var begIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                begDate,
                9900,
                false);
            var endIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                endDate,
                1100,
                false);
            //act
            aggregate.AddIndication(begIndication);
            aggregate.AddIndication(endIndication);
            var act = aggregate.СalculationPeriod(begDate, begDate, endDate);

            //assert
            act.Should().Be(108800.0);
        }

        [TestMethod]
        public void СalculationPeriodTest_Indecation_NotOverDimenshion()
        {
            //arrange
            var begDate = randomizer.Create<DateTime>().Date;
            var endDate = begDate.AddMonths(1);

            var aggregate = new MeterScaleAggregate(
                randomizer.Create<long>(),
                randomizer.Create<long>(),
                randomizer.Create<string>(),
                randomizer.Create<Const.ZoneOfDay>(),
                5,
                begDate,
                endDate,
                false,
                false
                );
            var begIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                begDate,
                9000,
                false);
            var endIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                endDate,
                9100,
                false);
            //act
            aggregate.AddIndication(begIndication);
            aggregate.AddIndication(endIndication);
            var act = aggregate.СalculationPeriod(begDate, begDate, endDate);

            //assert
            act.Should().Be(100.0);
        }

        [TestMethod]
        public void СalculationPeriodTest_NotIndecation_Should_ThrowExcaption()
        {
            //arrange
            var begDate = randomizer.Create<DateTime>().Date;
            var endDate = begDate.AddMonths(1);

            var aggregate = new MeterScaleAggregate(
                randomizer.Create<long>(),
                randomizer.Create<long>(),
                randomizer.Create<string>(),
                randomizer.Create<Const.ZoneOfDay>(),
                5,
                begDate,
                endDate,
                false,
                false
                );
            //act

            Action act = () => aggregate.СalculationPeriod(begDate, begDate, endDate);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Error not set indication date");
        }

        [TestMethod]
        public void СalculationPeriodTest_NotIndecationOnEnd_Should_ThrowExcaption()
        {
            //arrange
            var begDate = randomizer.Create<DateTime>().Date;
            var endDate = begDate.AddMonths(1);

            var aggregate = new MeterScaleAggregate(
                randomizer.Create<long>(),
                randomizer.Create<long>(),
                randomizer.Create<string>(),
                randomizer.Create<Const.ZoneOfDay>(),
                5,
                begDate,
                endDate,
                false,
                false
                );
            var begIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                begDate,
                9000,
                false);

            var begIndicationDouble = new IndicationDocumentAggregate(randomizer.Create<long>(),
                    begDate,
                    9100,
                    false);

            //act
            aggregate.AddIndication(begIndication);
            aggregate.AddIndication(begIndicationDouble);
            Action act = () => aggregate.СalculationPeriod(begDate, begDate, endDate);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Error not find indication date");
        }

        [TestMethod]
        public void СalculationPeriodTest_NotIndecationOnStart_Should_ThrowExcaption()
        {
            //arrange
            var begDate = randomizer.Create<DateTime>().Date;
            var endDate = begDate.AddMonths(1);

            var aggregate = new MeterScaleAggregate(
                randomizer.Create<long>(),
                randomizer.Create<long>(),
                randomizer.Create<string>(),
                randomizer.Create<Const.ZoneOfDay>(),
                5,
                begDate,
                endDate,
                false,
                false
                );
            var endIndication = new IndicationDocumentAggregate(randomizer.Create<long>(),
                endDate,
                9000,
                false);

            var endIndicationDouble = new IndicationDocumentAggregate(randomizer.Create<long>(),
                    endDate,
                    9100,
                    false);

            //act
            aggregate.AddIndication(endIndication);
            aggregate.AddIndication(endIndicationDouble);
            Action act = () => aggregate.СalculationPeriod(begDate, begDate, endDate);

            //assert
            act.Should().Throw<ArgumentException>().WithMessage("Error not find indication date");
        }
    }
}