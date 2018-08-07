using Data.EF;
using Data.EF.Repositories;
using Data.EF.Repositories.Documents;
using Data.EF.Repositories.References;
using Domain.Interface.Repositories;
using Domain.Interface.Repositories.Documents;
using Domain.Interface.Repositories.References;
using Ninject.Modules;
using Ninject.Parameters;
using System;

namespace DAL
{
    public class DALmodule : NinjectModule
    {
        private bool loaded;
        private string connectionStringToDB;

        public DALmodule(string connectionString)
        {
            this.connectionStringToDB = connectionString;
        }

        public override void Load()
        {
            if (loaded)
                throw new Exception("Module should be initiated only once");
            loaded = true;

            var conStringParameter = new ConstructorArgument("connectionString", connectionStringToDB);
            Kernel.Bind<DatabaseContext>()
                .ToSelf()
                .InCustomRequestScope()
                .WithParameter(conStringParameter)
                ;

            #region Repository

            Kernel.Bind<IAgreementRepository>().To<AgreementRepository>();
            Kernel.Bind<IUnitRepository>().To<UnitRepository>();
            Kernel.Bind<ITariffGroupRepository>().To<TariffGroupRepository>();
            Kernel.Bind<IChannelRepository>().To<ChannelRepository>();
            Kernel.Bind<IConstantFlowRepository>().To<ConstantFlowRepository>();
            Kernel.Bind<IMeterRepository>().To<MeterRepository>();
            Kernel.Bind<IMeterScaleRepostitory>().To<MeterScaleRepostitory>();
            Kernel.Bind<IMeterTypeRepository>().To<MeterTypeRepository>();
            Kernel.Bind<ITariffRepository>().To<TariffRepository>();

            Kernel.Bind<IIndicationDocumentRepository>().To<IndicationDocumentRepository>();

            Kernel.Bind<IApplicationUserRepository>().To<ApplicationUserRepository>();

            #endregion Repository

            Mappings.Init();
        }
    }
}