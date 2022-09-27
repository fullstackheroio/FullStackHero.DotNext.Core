namespace FullStackHero.DotNext.Core.Json.Microsoft.NamingPolicy;

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
    #region Overrides of JsonNamingPolicy

    /// <summary>When overridden in a derived class, converts the specified name according to the policy.</summary>
    /// <param name="name">The name to convert.</param>
    /// <returns>The converted name.</returns>
    public override string ConvertName(string name) => name.ToUpper();

    #endregion
}