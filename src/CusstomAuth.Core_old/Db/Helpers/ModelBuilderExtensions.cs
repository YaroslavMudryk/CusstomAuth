using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CusstomAuth.Core.Db.Helpers;

public static class ModelBuilderExtensions
{
    public static EntityTypeBuilder<T> ToTest<T>(this EntityTypeBuilder<T> builder, string someone) where T : class
    {
        builder.HasAnnotation("Model:Test", someone);
        return builder;
    }
}
