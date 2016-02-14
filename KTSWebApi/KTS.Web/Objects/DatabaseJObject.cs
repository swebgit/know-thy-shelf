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
        public const string TYPE_PROPERTY_NAME = "type";
        public const string OBJECT_ID_PROPERTY_NAME = "objectID";

        public JObject JObject { get; set; }

        public DatabaseJObject()
        {

        }

        public DatabaseJObject(JObject jObject)
        {
            this.JObject = jObject;
        }
                
        public DatabaseObjectType ObjectType
        {
            get
            {
                if (this.JObject != null && this.JObject[TYPE_PROPERTY_NAME] != null)
                {
                    DatabaseObjectType objectType;
                    if (Enum.TryParse(this.JObject[TYPE_PROPERTY_NAME].Value<string>(), out objectType))
                    {
                        return objectType;
                    }
                }
                return DatabaseObjectType.None;
            }
            set
            {
                if (this.JObject != null)
                {
                    this.JObject[TYPE_PROPERTY_NAME] = value.ToString();
                }
            }
        }
        
        public int? ObjectId
        {
            get
            {
                if (this.JObject != null && this.JObject[OBJECT_ID_PROPERTY_NAME] != null)
                {
                    int objectId;
                    if (int.TryParse(this.JObject[OBJECT_ID_PROPERTY_NAME].Value<string>(), out objectId))
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
                    this.JObject[OBJECT_ID_PROPERTY_NAME] = value;
                }
            }
        }
    }
}