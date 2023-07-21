﻿namespace FunicularSwitch.Generators.FluentAssertions.Templates;

internal class GenerateFluentAssertionsForTemplates
{
    public static string MyResultFluentAssertionExtensions =>
        Resources.ReadResource("MyResultFluentAssertionExtensions.cs");
    public static string MyResultAssertions => Resources.ReadResource("MyResultAssertions.cs");
    public static string MyUnionTypeFluentAssertionExtensions =>
        Resources.ReadResource("MyUnionTypeFluentAssertionExtensions.cs");
    public static string MyUnionTypeAssertions => Resources.ReadResource("MyUnionTypeAssertions.cs");
    public static string MyDerivedUnionTypeAssertions => Resources.ReadResource("MyUnionTypeAssertions_DerivedUnionType.cs");
    public static string StaticCode => Resources.ReadResource("GenerateFluentAssertionsForAttribute.cs");
}

internal static class Resources
{
    private static readonly string Namespace = $"{typeof(GenerateFluentAssertionsForTemplates).Namespace}";

    public static string ReadResource(string filename)
    {
        var resourcePath = $"{Namespace}.{filename}";
        using var stream = typeof(GenerateFluentAssertionsForTemplates).Assembly.GetManifestResourceStream(resourcePath);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }
}