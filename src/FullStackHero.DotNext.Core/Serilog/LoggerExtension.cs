namespace FullStackHero.DotNext.Core.Serilog;

public static class LoggerExtension
{
    public const string OutputTemplate =
        "{Timestamp:dd-MM-yyyy HH:mm:ss.fff} [{EventType:x8} {Level:u3}] <s:[{SourceContext}]> <f:[{FileName} > {MemberName}]>{NewLine}at {FilePath}:{LineNumber}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

    /// <summary>
    ///     Phương thức hỗ trợ ghi log kèm theo tên phương thức, file có phương thức log được gọi, vị trí dòng gọi log.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ILogger WithCaller<T>(this ILogger logger,
                                        [CallerMemberName] string memberName = "",
                                        [CallerFilePath] string filePath = "",
                                        [CallerLineNumber] int lineNumber = 0) where T : class =>
        logger.ForContext<T>()
              .ForContext("MemberName", memberName)
              .ForContext("FilePath", filePath)
              .ForContext("FileName", Path.GetFileName(filePath))
              .ForContext("LineNumber", lineNumber);

    /// <summary>
    ///     Phương thức hỗ trợ ghi log kèm theo tên phương thức, file có phương thức log được gọi, vị trí dòng gọi log.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="memberName"></param>
    /// <param name="filePath"></param>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public static ILogger WithCaller(this ILogger logger,
                                     [CallerMemberName] string memberName = "",
                                     [CallerFilePath] string filePath = "",
                                     [CallerLineNumber] int lineNumber = 0) =>
        logger.ForContext("MemberName", memberName)
              .ForContext("FilePath", filePath)
              .ForContext("FileName", Path.GetFileName(filePath))
              .ForContext("LineNumber", lineNumber);
}