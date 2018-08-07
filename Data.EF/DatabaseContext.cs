using Data.EF.Models;
using Data.EF.Models.Documents;
using Data.EF.Models.References;
using SQLite.CodeFirst;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

namespace Data.EF
{
    public class DatabaseContext : DbContext
    {
        private short countTransaction = 0;

        public DatabaseContext(string connectionString) : base(connectionString)
        {
        }

        #region dbSet

        public virtual DbSet<CustomHistory> CustomHistories { get; set; }

        public virtual DbSet<TariffDTO> TariffDTOs { get; set; }
        public virtual DbSet<MeterTypeDTO> MeterTypeDTOs { get; set; }

        public virtual DbSet<ObjectDTO> ObjectDTOs { get; set; }
        public virtual DbSet<LinkObjectsDTO> LinkObjectsDTOs { get; set; }

        public virtual DbSet<AgreementDTO> AgreementDTOs { get; set; }
        public virtual DbSet<UnitDTO> UnitDTOs { get; set; }
        public virtual DbSet<TariffGroupDTO> TariffGroupDTOs { get; set; }
        public virtual DbSet<ChannelDTO> ChannelDTOs { get; set; }
        public virtual DbSet<ConstantFlowDTO> ConstantFlowDTOs { get; set; }
        public virtual DbSet<MeterDTO> MeterDTOs { get; set; }
        public virtual DbSet<MeterScaleDTO> MeterScaleDTOs { get; set; }

        public virtual DbSet<DocumentDTO> DocumentDTOs { get; set; }
        public virtual DbSet<PaymentDocumentDTO> PaymentDocumentDTOs { get; set; }
        public virtual DbSet<IndicationDocumentDTO> IndicationDocumentDTOs { get; set; }

        #endregion dbSet

        #region Transaction

        public virtual IDatabaseTransaction BeginTransaction() => BeginTransaction(IsolationLevel.Serializable);

        public virtual IDatabaseTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            countTransaction++;
            //var transaction = new DatabaseTransaction(this, isolationLevel);
            var transaction = new DatabaseTransaction(this);
            transaction.EndTransaction += (object sender, System.EventArgs e) => countTransaction--;
            return transaction;
        }

        public virtual short GetCountTransaction() => countTransaction;

        private List<Dictionary<string, object>> QueryCommand(DbCommand cmd)
        {
            using (var dataReader = cmd.ExecuteReader())
            {
                var results = new List<Dictionary<string, object>>();
                while (dataReader.Read())
                {
                    var dataRow = GetDataRow(dataReader);
                    results.Add(dataRow);
                }
                return results;
            }
        }

        private Dictionary<string, object> GetDataRow(DbDataReader dataReader)
        {
            var result = new Dictionary<string, object>();
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
            {
                var name = dataReader.GetName(fieldCount);
                var rawValue = dataReader.GetValue(fieldCount);
                result.Add(name, rawValue);
            }
            return result;
        }

        #endregion Transaction

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ModelConfiguration.Configure(modelBuilder);
            var initializer = new SQLLiteDbInitializer(modelBuilder);
            Database.SetInitializer(initializer);

            var model = modelBuilder.Build(Database.Connection);
            var sqlGenerator = new SqliteSqlGenerator();
            string sql = sqlGenerator.Generate(model.StoreModel);
        }
    }
}