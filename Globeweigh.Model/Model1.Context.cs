﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Globeweigh.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GlobeweighEntities : DbContext
    {
        public GlobeweighEntities()
            : base("name=GlobeweighEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Batch_OperatorTime> Batch_OperatorTime { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<EntityType> EntityTypes { get; set; }
        public virtual DbSet<EntityTypeField> EntityTypeFields { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<Portion> Portions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ReferenceData> ReferenceDatas { get; set; }
        public virtual DbSet<Scale> Scales { get; set; }
        public virtual DbSet<vwBatchView> vwBatchViews { get; set; }
        public virtual DbSet<vwOperatorBatch> vwOperatorBatches { get; set; }
        public virtual DbSet<vwOperatorRefData> vwOperatorRefDatas { get; set; }
        public virtual DbSet<vwPortionView> vwPortionViews { get; set; }
        public virtual DbSet<UserDisplay> UserDisplays { get; set; }
    
        public virtual ObjectResult<spOperatorBatch_Result> spOperatorBatch(Nullable<int> batchId)
        {
            var batchIdParameter = batchId.HasValue ?
                new ObjectParameter("batchId", batchId) :
                new ObjectParameter("batchId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spOperatorBatch_Result>("spOperatorBatch", batchIdParameter);
        }
    
        public virtual ObjectResult<spOperatorStats_Result> spOperatorStats(Nullable<System.DateTime> dateFrom, Nullable<System.DateTime> dateTo)
        {
            var dateFromParameter = dateFrom.HasValue ?
                new ObjectParameter("dateFrom", dateFrom) :
                new ObjectParameter("dateFrom", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("dateTo", dateTo) :
                new ObjectParameter("dateTo", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spOperatorStats_Result>("spOperatorStats", dateFromParameter, dateToParameter);
        }
    }
}
