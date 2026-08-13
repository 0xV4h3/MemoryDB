using System.Linq.Expressions;
using System.Reflection;

namespace Engine.Serialization.Binary;

internal static class TypeAccessorCache
{
    private static readonly Dictionary<Type, TypeAccessorPlan> _cache = [];

    public static TypeAccessorPlan GetOrBuild(Type type)
    {
        if (_cache.TryGetValue(type, out var plan)) return plan;

        plan = BuildPlan(type);
        _cache[type] = plan;
        return plan;
    }

    private static TypeAccessorPlan BuildPlan(Type type)
    {
        var properties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0)
            .Where(p => p.GetCustomAttribute<BinaryIgnoreAttribute>() is null)
            .OrderBy(p => p.GetCustomAttribute<BinaryOrderAttribute>()?.Order ?? int.MaxValue)
            .ThenBy(p => p.Name, StringComparer.Ordinal)
            .Select(BuildAccessor)
            .ToArray();

        return new TypeAccessorPlan { Type = type, Properties = properties };
    }

    private static PropertyAccessor BuildAccessor(PropertyInfo property)
    {
        var (kind, elementType, underlyingType) = FieldKindClassifier.Classify(property.PropertyType);

        return new PropertyAccessor
        {
            Name = property.Name,
            PropertyType = property.PropertyType,
            Kind = kind,
            ElementType = elementType,
            UnderlyingType = underlyingType,
            Getter = BuildGetter(property),
            Setter = BuildSetter(property)
        };
    }

    private static Func<object, object?> BuildGetter(PropertyInfo property)
    {
        var instanceParam = Expression.Parameter(typeof(object), "instance");
        var typedInstance = Expression.Convert(instanceParam, property.DeclaringType!);
        var propertyAccess = Expression.Property(typedInstance, property);
        var boxedResult = Expression.Convert(propertyAccess, typeof(object));

        return Expression.Lambda<Func<object, object?>>(boxedResult, instanceParam).Compile();
    }

    private static Action<object, object?> BuildSetter(PropertyInfo property)
    {
        var instanceParam = Expression.Parameter(typeof(object), "instance");
        var valueParam = Expression.Parameter(typeof(object), "value");
        var typedInstance = Expression.Convert(instanceParam, property.DeclaringType!);
        var typedValue = Expression.Convert(valueParam, property.PropertyType);
        var propertyAccess = Expression.Property(typedInstance, property);
        var assign = Expression.Assign(propertyAccess, typedValue);

        return Expression.Lambda<Action<object, object?>>(assign, instanceParam, valueParam).Compile();
    }
    
    public static int CachedTypeCount => _cache.Count;
}


