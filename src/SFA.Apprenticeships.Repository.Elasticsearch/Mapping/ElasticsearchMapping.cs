﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using SFA.Apprenticeships.Repository.Elasticsearch.Attributes;

namespace SFA.Apprenticeships.Repository.Elasticsearch.Mapping
{
    public static class ElasticsearchMapping
    {
        public static string  Create<T>()
        {
            return Create(typeof(T));
        }

        public static string Create(Type esType)
        {
            // verify the class has the appropriate mapping attribute
            var attribute = esType.GetCustomAttributes(true).FirstOrDefault() as ElasticSearchMappingAttribute;
            if (attribute == null)
            {
                throw new ArgumentException("The class must be declared with the ElasticSearchMappingAttribute.");
            }

            var mapping = new StringBuilder();

            // start
            mapping.AppendFormat("{{\"{0}\":{{",
                string.IsNullOrEmpty(attribute.Name) ? esType.Name.ToLower() : attribute.Name.ToLower());

            mapping.Append(CreateProperties(esType));

            // finalize
            mapping.Append("}}");

            return mapping.ToString();
        }

        public static string CreateProperties(Type esType)
        {
            var mapping = new StringBuilder("\"properties\":{");

            // For all public properties build the mappings
            bool isfirst = true;
            foreach (PropertyInfo info in esType.GetProperties())
            {
                bool ignoreProperty = false;
                string propertyType;

                var isCollection = typeof(IList).IsAssignableFrom(info.PropertyType);
                if (isCollection)
                {
                    // assign properties of nested type - needs attention for items not of IList !!!
                    propertyType = "\"nested\"," + CreateProperties(info.PropertyType.GetGenericArguments()[0]);
                }
                else
                {
                    switch (info.PropertyType.FullName)
                    {
                        case "System.Int32":
                            propertyType = "\"integer\"";
                            break;

                        case "System.DateTime": 
                            // user attribute to set date only
                            propertyType = "\"date\", \"format\":\"yyyy-MM-dd HH:mm:ss\"";
                            break;

                        default:
                            propertyType = string.Format("\"{0}\"", info.PropertyType.Name.ToLower());
                            break;
                    }
                }

                var attrs = info.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    // add additional indexing
                    //if (attr is ElasticsearchIndex)
                    //{ example
                           //                 "movie": {
                           //   "properties": {
                           //      "director": {
                           //         "type": "multi_field",
                           //         "fields": {
                           //             "director": {"type": "string"},
                           //             "original": {"type" : "string", "index" : "not_analyzed"}
                           //         }
                           //      }
                           //   }
                           //}
                    //}

                    // override property type from attribute
                    if (attr is ElasticSearchTypeAttribute)
                    {
                        var typeAttrib = attr as ElasticSearchTypeAttribute;
                        propertyType = string.IsNullOrEmpty(typeAttrib.Format) ? 
                            string.Format("\"{0}\"", typeAttrib.Name.ToLower()) : 
                            string.Format("\"{0}\",\"format\":\"{1}\"", typeAttrib.Name.ToLower(), typeAttrib.Format);
                    }

                    // ignore this property in the mapping
                    if (attr is ElasticSearchIgnoreAttribute)
                    {
                        ignoreProperty = true;
                    }
                }

                if (!ignoreProperty)
                {
                    if (!isfirst)
                    {
                        mapping.Append(",");
                    }
                    else
                    {
                        isfirst = false;
                    }

                    mapping.AppendFormat("\"{0}\":{{\"type\":{1}}}", info.Name, propertyType);
                }
            }

            // finalize
            mapping.Append("}");

            return mapping.ToString();
        }
    }
}
