namespace FullStackHero.DotNext.Core.Communication;

public sealed class ActionResult<T>
{
    #region Constructor

    private ActionResult(bool isSuccess = false, string? message = default, T result = default!) => (IsSuccess, Message, Result) = (isSuccess, message, result);

    #endregion

    #region Override methods

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => IsSuccess ? $"{Constant.SuccessPrefix}{Message}" : $"{Constant.ErrorPrefix}{Message}";

    #endregion

    #region Static methods

    public static ActionResult<T> Success(T result, string message = "Success") => new(true, message, result);
    public static ActionResult<T> Error(string message = "Failed")              => new(false, message);
    public static bool operator true(ActionResult<T> result)                    => result.IsSuccess;
    public static bool operator false(ActionResult<T> result)                   => !result.IsSuccess;

    #endregion

    #region Properties

    public bool    IsSuccess { get; }
    public string? Message   { get; }
    public T       Result    { get; }

    #endregion
}