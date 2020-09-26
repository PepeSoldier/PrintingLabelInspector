using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace XLIB_COMMON.Model
{
    public class ObjectDataChange
    {
        public int objectId { get; set; }
        public string objectDescription { get; set; }
        public int parentObjectId { get; set; }
        public string parentObjectDescription { get; set; }
        public string objClassName { get; set; }
        public string fieldName { get; set; }
        public string fieldDisplayName { get; set; }
        public object oldValue { get; set; }
        public object newValue { get; set; }
        public Type type { get; set; }
    } 

    public class ObjectsComparer
    {
        public ObjectsComparer()
        {
            Config = new ObjectsComparerConfig();
            ObjectDataChanges = new List<ObjectDataChange>();
        }

        public ObjectsComparer(bool considerOnlyLoggable) : this()
        {
            Config.ConsiderOnlyLoggable = considerOnlyLoggable;
        }

        public ObjectsComparerConfig Config { get; set; }
        public List<ObjectDataChange> ObjectDataChanges { get; set; }

        public bool DetectChanges(object objOld, object objNew, int objectId, int parentObjectId = 0, string objectDescr = "", string parentObjectDescr = "")
        {
            bool flague = false;
            if (objOld == null)
            {
                objOld = new { id = 0 };
                ObjectDataChange change = new ObjectDataChange();
                change.objClassName = objNew.GetType().BaseType.Name == "Object" ? objNew.GetType().Name : objNew.GetType().BaseType.Name;
                change.objectId = objectId;
                change.objectDescription = objectDescr;
                change.parentObjectId = parentObjectId;
                change.parentObjectDescription = parentObjectDescr;
                flague = true;
                ObjectDataChanges.Add(change);
            }
            else
            {
                PropertyInfo[] pOld = objOld.GetType().GetProperties();
                PropertyInfo[] pNew = objNew.GetType().GetProperties();
                DateTime dtNow = DateTime.Now;
                object o1 = null;
                object o2 = null;

                for (int i = 0; i < pOld.Length; i++)
                {
                    ObjectDataChange change = new ObjectDataChange();

                    change.objClassName = objNew.GetType().BaseType.Name == "Object" ? objNew.GetType().Name : objNew.GetType().BaseType.Name;
                    change.objectId = objectId;
                    change.parentObjectId = parentObjectId;
                    change.fieldName = pOld[i].Name;
                    change.objectDescription = objectDescr;
                    change.parentObjectDescription = parentObjectDescr;

                    try
                    {
                        o1 = pOld.FirstOrDefault(x => x.Name == change.fieldName).GetValue(objOld);
                        o2 = pNew.FirstOrDefault(x => x.Name == change.fieldName).GetValue(objNew);
                    }
                    catch{

                        o1 = null;
                        o2 = null;
                    }
                    finally
                    {
                    }

                    if (o1 != null && o2 != null)
                    {
                        change.oldValue = o1;//!= null ? o1.ToString() : string.Empty;
                        change.newValue = o2;//!= null ? o2.ToString() : string.Empty;
                        change.type = o1 != null ? o1.GetType() : (string.Empty).GetType();

                        DisplayAttribute dispAttr = pOld[i].GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                        change.fieldDisplayName = (dispAttr != null) ? dispAttr.Name : null;
                        string dispAttrName = dispAttr != null ? dispAttr.GroupName : String.Empty;

                        if (isChangeDetected(change, dispAttrName))
                        {
                            flague = true;
                            ObjectDataChanges.Add(change);
                        }
                    }
                }
            }
            return flague;
        }

        private bool isChangeDetected(ObjectDataChange change, string dispAttrName)
        {
            return (isValueChanged(change)) &&
                    (onlyIncludedFields(change)) &&
                    (isNotFieldExcluded(change)) &&
                    (isAttributeLoggable(dispAttrName));
        }

        private bool isAttributeLoggable(string dispAttrName)
        {
            return !Config.ConsiderOnlyLoggable || (Config.ConsiderOnlyLoggable && dispAttrName == "Loggable");
        }

        private bool isValueChanged(ObjectDataChange change)
        {             
            if(Type.GetTypeCode(change.type) == TypeCode.Int32 || Type.GetTypeCode(change.type) == TypeCode.Int16)
            {
                return (int)change.newValue != (int)change.oldValue;
            }
            else if(Type.GetTypeCode(change.type) == TypeCode.Decimal)
            {
                return (decimal)change.newValue != (decimal)change.oldValue;
            }
            else if (Type.GetTypeCode(change.type) == TypeCode.DateTime)
            {
                return (DateTime)change.newValue != (DateTime)change.oldValue;
            }
            else if (Type.GetTypeCode(change.type) == TypeCode.Boolean)
            {
                return (bool)change.newValue != (bool)change.oldValue;
            }
            else if (Type.GetTypeCode(change.type) == TypeCode.Double)
            {
                return (double)change.newValue != (double)change.oldValue;
            }
            else
            {
                string val1 = string.Empty;
                string val2 = string.Empty;
                try
                {
                    val1 = (string)change.newValue;
                    val2 = (string)change.oldValue;
                    return val1 != val2;
                }
                catch {
                    return false;
                }
            }
        }

        private bool isNotFieldExcluded(ObjectDataChange change)
        {
            return Config.ExcludedFields == null || (Config.ExcludedFields != null && !Config.ExcludedFields.Contains(change.fieldName));
        }

        private bool onlyIncludedFields(ObjectDataChange change)
        {
            return Config.IncludedFields == null || (Config.IncludedFields != null && Config.IncludedFields.Contains(change.fieldName));
        }

        public T CastExamp1<T>(object input)
        {
            return (T)input;
        }
    }

    public class ObjectsComparerConfig
    {
        public ObjectsComparerConfig()
        {
            this.ConsiderOnlyLoggable = false;
            AdditionalLogData = new Dictionary<string, string>();
        }

        public string[] ExcludedFields { get; set; }
        public string[] IncludedFields { get; set; }
        public bool ConsiderOnlyLoggable { get; set; }
        public Dictionary<string, string>  AdditionalLogData { get; set; } //string - if change on property , string - log another property

        public void AddLogDependentField(string observedField, string fieldToLog)
        {
            AdditionalLogData.Add(observedField, fieldToLog);
        }

    }

    
}