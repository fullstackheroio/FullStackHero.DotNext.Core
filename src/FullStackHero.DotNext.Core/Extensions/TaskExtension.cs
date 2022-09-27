namespace FullStackHero.DotNext.Core.Extensions;

public static class TaskExtension
{
    /// <summary>
    ///     Cancelling uncancellable operations.
    /// </summary>
    /// <typeparam name="TRsult">Có thể là 1 class, hoặc 1 kiểu dữ liệu: string, int, long...</typeparam>
    /// <param name="task">
    ///     Đầu vào là một task có kết quả trả về <see cref="TRsult" />
    ///     Task này thực hiện một function được wrap trong lambda method: Task.Run(()=>action)
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="TaskCanceledException">
    ///     Phương thức này sẽ ném ra ngoại lệ TaskCanceledException khi người dùng gọi
    ///     phương thức _cts.Cancel();
    ///     _cts là đối tượng của <see cref="CancellationTokenSource" />
    /// </exception>
    /// <seealso>
    ///     <cref>https://johnthiriet.com/cancel-asynchronous-operation-in-csharp</cref>
    ///     <cref>https://github.com/mincasoft/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md</cref>
    /// </seealso>
    public static async Task<TRsult> WithCancellationAsync<TRsult>(this Task<TRsult> task, CancellationToken cancellationToken = default)
    {
        // We create a TaskCompletionSource of TRsult
        var taskCompletionSource = new TaskCompletionSource<TRsult>(TaskCreationOptions.RunContinuationsAsynchronously);

        // Registering a lambda into the cancellationToken
        // We received a cancellation message, cancel the TaskCompletionSource.Task
        await using (cancellationToken.Register(() => taskCompletionSource.TrySetCanceled()))
        {
            // Wait for the first task to finish among the two
            var completedTask = await Task.WhenAny(task, taskCompletionSource.Task).ConfigureAwait(false);

            return await completedTask.ConfigureAwait(false);
        }
    }
}