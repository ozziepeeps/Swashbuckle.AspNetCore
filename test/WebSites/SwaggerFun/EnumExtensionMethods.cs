using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nexus.Extensions;

namespace SwaggerFun
{
    internal static class EnumExtensionMethods
    {
        public static string Describe(this IEnumerable<EnumMember> values)
        {
            return string.Join("<br/>", values.EmptyIfNull()
                .Select(x => string.Format(CultureInfo.CurrentCulture, "{0}&nbsp;({1})", x.Name, x.Value.ToString())))
                .TrimToNull();
        }
    }
}
