using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackEndDotnetPlumsail.Data.Mapping
{
    public static class Mapping
    {         
        public static CreateIndexDescriptor ObjectMapping(this CreateIndexDescriptor descriptor)
        {
            return descriptor.Map<object>(x => x.DynamicTemplates(d => d.DynamicTemplate("Search", dt => dt.PathMatch("*")
                                                .Mapping(m => m.Generic(g => g.Type("text")
                                                               .CopyTo(x => x.Field("Search")))))));
        }
    }
}
