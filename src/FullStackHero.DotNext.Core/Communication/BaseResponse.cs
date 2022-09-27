namespace FullStackHero.DotNext.Core.Communication;

public abstract class BaseResponse<T> where T : class
{
    #region Constructor

    protected BaseResponse(bool isSuccess, string? message = default, T? result = default!) => (IsSuccess, Message, Result) = (isSuccess, message, result);

    #endregion

    #region Override methods

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => IsSuccess ? $"{Constant.SuccessPrefix}{Message}" : $"{Constant.ErrorPrefix}{Message}";

    #endregion

    #region Properties

    public bool    IsSuccess { get; }
    public string? Message   { get; }
    public T?      Result    { get; }

    #endregion
}