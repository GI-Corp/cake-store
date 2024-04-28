namespace Shared.Common.Extensions.Attribute;

public class AllowUpdateAttribute : global::System.Attribute
{
    public bool Updatable { get; set; } = true;
}