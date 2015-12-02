using Examine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Examine.Linq.Attributes;

namespace Umbraco.Examine.Linq.Mapper
{
    public class SearchResultMapper<T> : IMapper<T>
    {
        public IEnumerable<T> Map(IEnumerable<SearchResult> results)
        {
            List<T> mappedResults = new List<T>();
            var configuration = AnalyseTypeMapping();
            Type type = typeof(T);
            foreach(var result in results)
            {
                T mappedResult = Activator.CreateInstance<T>();

                if (!string.IsNullOrEmpty(configuration.SearchResultProperty))
                    type.GetProperty(configuration.SearchResultProperty).SetMethod.Invoke(mappedResult, new [] { result });
                
                foreach(var propertyName in configuration.FieldMappings.Keys)
                {
                    var property = type.GetProperty(propertyName);
                    if (result.Fields.ContainsKey(configuration.FieldMappings[propertyName]))
                        property.SetMethod.Invoke(mappedResult, new[] { convertPropertyValue(property, result.Fields[configuration.FieldMappings[propertyName]]) });
                }
                mappedResults.Add(mappedResult);
            }
            return mappedResults;
        }

        protected object convertPropertyValue(PropertyInfo property, string p)
        {
            try
            {
                return Convert.ChangeType(p, property.PropertyType);
            }
            catch (Exception)
            {
                if (property.PropertyType == typeof(DateTime))
                    return DateTime.ParseExact(p, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);
                return null;
            }
        }

        protected MapperConfiguration AnalyseTypeMapping()
        {
            var configuration = new MapperConfiguration();
            Type type = typeof(T);
            foreach(var property in type.GetProperties())
            {
                if (!property.CanWrite) continue;

                if(property.PropertyType == typeof(SearchResult))
                {
                    configuration.SearchResultProperty = property.Name;
                }
                else
                {
                    var fieldAttribute = (FieldAttribute)property.GetCustomAttribute(typeof(FieldAttribute));
                    configuration.FieldMappings.Add(property.Name, fieldAttribute != null ? fieldAttribute.Name : property.Name);
                }
            }
            return configuration;
        }
    }
}
