using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using Globeweigh.Model;

namespace Globeweigh.UI.Shared.Services
{
    public interface IReferenceDataRepository
    {
        Task<List<ReferenceData>> GetReferenceData(eRefDataType refDataType);
        Task<List<RefDataLight>> GetReferenceDataLight(eRefDataType refDataType);
        Task<RefDataLight> GetReferenceDataLightById(int refId);
        Task<ReferenceData> GetReferenceDataById(int refId);
        Task<ReferenceData> AddUpdateRefDataAsync(ReferenceData refData);
        Task<object> MarkRefDataAsDeletedAsync(object selectedRefData);
        Task<EntityTypeField> UpdateEntityTypeFieldAsync(EntityTypeField entityTypeField);
        Task<List<EntityTypeField>> GetRequiredEntityTypeFields(EntityType selectedEntity);
        Task<List<EntityType>> GetEntityTypes();
        Task<List<EntityTypeField>> GetAndSyncEntityTypeFields(EntityType selectedEntity);
        Task<List<GridColumn>> GetIsUsedColumnList(EntityType selectedEntity);
        Task<List<object>> GetGenericReferenceData(int entityTypeId, bool isRefDataType);
        Task<List<GridColumn>> GetVisibleColumnList(int entityTypeId);
    }

    public class ReferenceDataRepository : IReferenceDataRepository
    {
        public async Task<List<EntityType>> GetEntityTypes()
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.EntityTypes.OrderBy(a => a.SortOrder).ToListAsync();
            }
        }

        public async Task<List<GridColumn>> GetIsUsedColumnList(EntityType selectedEntity)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var list = await context.EntityTypeFields.Where(a => a.EntityTypeId == selectedEntity.EntityTypeId && a.IsUsed).OrderBy(a => a.SortOrder).ToListAsync();
                return list.Select(field => new GridColumn() { FieldName = field.FieldName, Header = field.DisplayName }).ToList();
            }
        }

        public async Task<List<GridColumn>> GetVisibleColumnList(int entityTypeId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                TextEditSettings textEditSettings = new TextEditSettings { HorizontalContentAlignment = EditSettingsHorizontalAlignment.Left };
                var list = await context.EntityTypeFields.Where(a => a.EntityTypeId == entityTypeId && a.IsVisible).OrderBy(a => a.SortOrder).ToListAsync();
                return list.Select(field => new GridColumn() { FieldName = field.FieldName, Header = field.DisplayName, EditSettings = textEditSettings }).ToList();
            }
        }

        public async Task<List<object>> GetGenericReferenceData(int entityTypeId, bool isRefDataType)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {

                if (isRefDataType)
                {
                    var list = await context.ReferenceDatas.Where(a => a.RefTypeId == entityTypeId && a.DateDeleted == null).ToListAsync();
                    return new List<object>(list.Cast<object>().ToList());
                }
                else
                {
                    //                    if (entityTypeId == (int)eRefDataType.Product)
                    //                    {
                    //                        var list = await context.vwProducts.OrderByDescending(a => a.CategoryName).ThenBy(a => a.ProductDescription).ToArrayAsync();
                    //                        return new List<object>(list.Cast<object>().ToList());
                    //                    }
                }

                return new List<object>();
            }
        }

        public async Task<List<RefDataLight>> GetReferenceDataLight(eRefDataType refDataType)
        {

            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await (from a in context.ReferenceDatas
                              where a.RefTypeId == (int)refDataType && a.DateDeleted == null
                              select new RefDataLight()
                              {
                                  id = a.id,
                                  RefTypeId = a.RefTypeId,
                                  Code = a.Code,
                                  DisplayName = a.DisplayName,
                                  DecimalData = a.DecimalData
                              }).ToListAsync();
            }
        }

        public async Task<List<ReferenceData>> GetReferenceData(eRefDataType refDataType)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.ReferenceDatas.Where(a => a.RefTypeId == (int)refDataType && a.DateDeleted == null).ToListAsync();
            }
        }


        public async Task<List<EntityTypeField>> GetAndSyncEntityTypeFields(EntityType selectedEntity)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                var entity = Type.GetType("Globeweigh.Model." + selectedEntity.EntityTypeName + ", Globeweigh.Model");
                if (entity != null)
                {
                    var properties = entity.GetProperties();
                    var mappedList = await context.EntityTypeFields.Where(a => a.EntityTypeId == selectedEntity.EntityTypeId).ToListAsync();

                    foreach (var property in properties)
                    {
                        var property1 = property;
                        var mappedMatch = mappedList.FirstOrDefault(a => a.FieldName == property1.Name);
                        if (mappedMatch == null)
                        {
                            var newMappedField = new EntityTypeField();
                            newMappedField.FieldName = property1.Name;
                            newMappedField.EntityTypeId = selectedEntity.EntityTypeId;
                            context.EntityTypeFields.Add(newMappedField);
                        }
                    }

                    await context.SaveChangesAsync();
                    var mappedList2 = await context.EntityTypeFields.Where(a => a.EntityTypeId == selectedEntity.EntityTypeId).ToListAsync();

                    foreach (var mappedField in mappedList2)
                    {
                        var propertyMatch = properties.FirstOrDefault(a => a.Name == mappedField.FieldName);
                        if (propertyMatch == null)
                        {
                            context.EntityTypeFields.Remove(mappedField);
                            context.Entry(mappedField).State = EntityState.Deleted;
                        }
                    }
                    await context.SaveChangesAsync();
                }
                return await context.EntityTypeFields.Where(a => a.EntityTypeId == selectedEntity.EntityTypeId).ToListAsync();
            }
        }

        public async Task<List<EntityTypeField>> GetRequiredEntityTypeFields(EntityType selectedEntity)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.EntityTypeFields.Where(a => a.EntityTypeId == selectedEntity.EntityTypeId && a.IsRequired).ToListAsync();
            }
        }

        public async Task<EntityTypeField> UpdateEntityTypeFieldAsync(EntityTypeField entityTypeField)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                context.EntityTypeFields.Attach(entityTypeField);
                context.Entry(entityTypeField).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
            return entityTypeField;
        }


        public async Task<RefDataLight> GetReferenceDataLightById(int refId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await (from a in context.ReferenceDatas
                              where a.id == (int)refId
                              select new RefDataLight()
                              {
                                  id = a.id,
                                  RefTypeId = a.RefTypeId,
                                  Code = a.Code,
                                  DisplayName = a.DisplayName,
                                  DecimalData = a.DecimalData
                              }).FirstOrDefaultAsync();
            }
        }

        public async Task<ReferenceData> GetReferenceDataById(int refId)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                return await context.ReferenceDatas.Where(a => a.id == refId).FirstOrDefaultAsync();
            }
        }

        public async Task<object> MarkRefDataAsDeletedAsync(object selectedRefData)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (selectedRefData.GetType() == typeof(ReferenceData))
                {
                    var refData = (ReferenceData)selectedRefData;
                    var refDataToUpdate = await context.ReferenceDatas.FirstOrDefaultAsync(a => a.id == refData.id);
                    if (refDataToUpdate != null)
                    {
                        refDataToUpdate.DateDeleted = DateTime.Now;
                        context.ReferenceDatas.Attach(refDataToUpdate);
                        context.Entry(refDataToUpdate).State = EntityState.Modified;
                        await context.SaveChangesAsync();
                    }
                    return refDataToUpdate;
                }
                else
                {

                }
                return null;
            }
        }

        public async Task<ReferenceData> AddUpdateRefDataAsync(ReferenceData refData)
        {
            using (var context = new GlobeweighEntities(GlobalVariables.ConnectionString))
            {
                if (refData.id == 0)
                {
                    refData.RefType = ((eRefDataType)refData.RefTypeId).ToString();
                    refData.DateCreated = DateTime.Now;
                    context.ReferenceDatas.Add(refData);
                }
                else
                {
                    context.ReferenceDatas.Attach(refData);
                    context.Entry(refData).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
            }
            return refData;
        }

    }

    public class RefDataLight
    {
        public int id { get; set; }
        public int RefTypeId { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string AlternativeDisplayName { get; set; }
        public decimal DecimalData { get; set; }
    }
}
