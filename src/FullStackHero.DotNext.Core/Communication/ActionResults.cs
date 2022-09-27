namespace FullStackHero.DotNext.Core.Communication;

public sealed class ActionResults<T>
{
    #region Constructor

    private ActionResults(bool isSuccess, string? message = default, IEnumerable<T>? results = default) => (IsSuccess, Message, Results) = (isSuccess, message, results);

    #endregion

    #region Override methods

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => IsSuccess ? $"{Constant.SuccessPrefix}{Message}" : $"{Constant.ErrorPrefix}{Message}";

    #endregion

    #region Static methods

    public static ActionResults<T> Success(IEnumerable<T>? results, string message = "Success") => new(true, message, results);
    public static ActionResults<T> Error(string message = "Failed")                             => new(false, message);
    public static bool operator true(ActionResults<T> results)                                  => results.IsSuccess;
    public static bool operator false(ActionResults<T> results)                                 => !results.IsSuccess;

    #endregion

    #region Properties

    public bool            IsSuccess { get; }
    public string?         Message   { get; }
    public IEnumerable<T>? Results   { get; }

    #endregion
}