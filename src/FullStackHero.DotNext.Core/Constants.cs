namespace FullStackHero.DotNext.Core;

public static class Constant
{
    public const           int     DelayMin      = 100;
    public const           string  Delim         = "->";
    public const           string  InternetErr   = "INTERNET NULL OR ERROR";
    public const           string  ErrorSign     = "~";
    public const           string  SuccessSign   = "+";
    public const           string  SuccessPrefix = "OK: ";
    public const           string  ErrorPrefix   = "ERR: ";
    public const           string  R             = "\r";
    public const           string  Rr            = "\r\r";
    public const           string  N             = "\n";
    public const           string  Nn            = "\n\n";
    public const           string  Rn            = "\r\n";
    public const           string  Nr            = "\n\r";
    public const           string  Tab           = "\t";
    public const           string  Q             = "\"";
    public const           string  Eq            = "\\\"";
    public const           string  Er            = "\\r";
    public const           string  En            = "\\n";
    public const           string  Etab          = "\\t";
    public const           string  Space         = " ";
    public const           string  Etc           = "...";
    public const           char    SpaceC        = ' ';
    public const           char    QuoteC        = '\'';
    public static readonly string  ExeFile       = Environment.GetCommandLineArgs()[0];
    public static readonly string? ExeFolder     = Path.GetDirectoryName(ExeFile);
}