using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IntelTypesetDesign.Json;

public class ListContractResolver(Type type) : DefaultContractResolver
{
    public override JsonContract ResolveContract(Type type)
    {
        if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
        {
            return base.ResolveContract(type.MakeGenericType(type.GenericTypeArguments[0]));
        }
        return base.ResolveContract(type);
    }

    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        return base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
    }
}