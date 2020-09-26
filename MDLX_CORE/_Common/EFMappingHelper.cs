using System.Collections;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;


namespace MDL_BASE.Models
{
    public class EFMetadataMappingHelper
    {
        public static string GetTableName(MetadataWorkspace metadata, DbEntityEntry entry)
        {
            var entityType = entry.Entity.GetType();

            var objectType = getObjectType(metadata, entityType);
            var conceptualSet = getConceptualSet(metadata, objectType);
            var storeSet = getStoreSet(metadata, conceptualSet);
            var tableName = findTableName(storeSet);

            return tableName;
        }

        //HOW TO USE EXAMPLE:
        //public string GetTableName(IDbContextBase db, DbEntityEntry entry)
        //{
        //    var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;
        //    return EFMetadataMappingHelper.GetTableName(metadata, entry);
        //}

        private static EntitySet getStoreSet(MetadataWorkspace metadata, EntitySetBase entitySet)
        {
            var csSpace = metadata.GetItems(DataSpace.CSSpace).Single();
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var entitySetMaps = (ICollection)csSpace.GetType().GetProperty("EntitySetMaps", flags).GetValue(csSpace, null);

            object mapping = null;

            foreach (var map in entitySetMaps)
            {
                var set = map.GetType().GetProperty("Set", flags).GetValue(map, null);
                if (entitySet == set)
                {
                    mapping = map;
                    break;
                }
            }

            var m_typeMappings = ((ICollection)mapping.GetType().BaseType.GetField("m_typeMappings", flags).GetValue(mapping)).OfType<object>().Single();
            var m_fragments = ((ICollection)m_typeMappings.GetType().BaseType.GetField("m_fragments", flags).GetValue(m_typeMappings)).OfType<object>().Single();
            var storeSet = (EntitySet)m_fragments.GetType().GetProperty("TableSet", flags).GetValue(m_fragments, null);

            return storeSet;
        }
        private static EntityType getObjectType(MetadataWorkspace metadata, System.Type entityType)
        {
            var objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

            var edmEntityType = metadata
                .GetItems<EntityType>(DataSpace.OSpace)
                .First(e => objectItemCollection.GetClrType(e) == entityType);

            return edmEntityType;
        }
        private static EntitySetBase getConceptualSet(MetadataWorkspace metadata, EntityType entityType)
        {
            var entitySetBase = metadata
                .GetItems<EntityContainer>(DataSpace.CSpace)
                .SelectMany(a => a.BaseEntitySets)
                .Where(s => s.ElementType.Name == entityType.Name)
                .FirstOrDefault();

            return entitySetBase;
        }

        private static string findTableName(EntitySet storeSet)
        {
            string tableName = null;

            MetadataProperty tableProperty;

            storeSet.MetadataProperties.TryGetValue("Table", true, out tableProperty);
            if (tableProperty == null || tableProperty.Value == null)
                storeSet.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2007/12/edm/EntityStoreSchemaGenerator:Table", true, out tableProperty);

            if (tableProperty != null)
                tableName = tableProperty.Value as string;

            if (tableName == null)
                tableName = storeSet.Name;

            return tableName;
        }
    }
}