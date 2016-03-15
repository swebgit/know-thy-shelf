using KTS.Web.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTS.Web.Objects
{
    public class DatabaseJObject
    {
        public JObject JObject { get; set; }

        public DatabaseJObject()
        {

        }

        public DatabaseJObject(JObject jObject)
        {
            this.JObject = jObject;
        }
                
        public string ObjectType
        {
            get
            {
                if (this.JObject != null && this.JObject[DatabaseFields.OBJECT_TYPE] != null)
                {
                    return this.JObject[DatabaseFields.OBJECT_TYPE].Value<string>();
                }
                return DatabaseObjectType.NONE;
            }
            set
            {
                if (this.JObject != null)
                {
                    this.JObject[DatabaseFields.OBJECT_TYPE] = value.ToString();
                }
            }
        }
        
        public int? ObjectId
        {
            get
            {
                if (this.JObject != null && this.JObject[DatabaseFields.OBJECT_ID] != null)
                {
                    int objectId;
                    if (int.TryParse(this.JObject[DatabaseFields.OBJECT_ID].Value<string>(), out objectId))
                    {
                        return objectId;
                    }
                }
                return null;
            }
            set
            {
                if (this.JObject != null)
                {
                    this.JObject[DatabaseFields.OBJECT_ID] = value;
                }
            }
        }

        public bool ObjectTypeIs(params string[] objectTypes)
        {
            return objectTypes.Any(o => o.Equals(this.ObjectType));
        }

        public bool ObjectTypeIsNot(params string[] objectTypes)
        {
            return objectTypes.All(o => !o.Equals(this.ObjectType));
        }
    }
}