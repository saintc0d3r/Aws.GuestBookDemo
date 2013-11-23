using System.Collections.Generic;
using System.Linq;

using Amazon.SimpleDB.Model;

namespace GuestBook.Repository.Aws.SimpleDb
{
    static class SimpleDbHelpers
    {
        public static string GetValue(this List<Attribute> attributes, string attributeName )
        {
            var result = attributes.FirstOrDefault(attribute => attribute.Name.Equals(attributeName));
            return result != null ? result.Value : string.Empty;
        }
    }
}
