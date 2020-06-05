using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunicularSwitch.Extensions;

namespace FunicularSwitch
{
    public abstract class Result
    {
        public static Result<T> Error<T>(string message) => new Error<T>(message);
        public static Result<T> Ok<T>(T value) => new Ok<T>(value);
        public abstract string GetErrorOrDefault(Func<string> defaultValue = null);
        public abstract bool IsError { get; }
        public bool IsOk => !IsError;
    }

    public abstract class Result<T> : Result, IEnumerable<T>
    {
        public static Result<T> Error(string message) => Error<T>(message);
        public static Result<T> Ok(T value) => Ok<T>(value);
        public override bool IsError => GetType() == typeof(Error<T>);

        public static implicit operator Result<T>(T value) 
            => Result.Ok(value);
        public static implicit operator bool(Result<T> result) 
            => result.Match(ok => true, error => false);

        // Matches
        public void Match(Action<T> ok, Action<string> error = null) => Match(
            v => ok.ToFunc().Invoke(v),
            err =>
            {
                error?.Invoke(err);
                return 42;
            });
        public T1 Match<T1>(Func<T, T1> ok, Func<string, T1> error)
        {
            switch (this)
            {
                case Ok<T> okResult:
                    return ok(okResult.Value);
                case Error<T> errorResult:
                    return error(errorResult.Message);
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }
        public async Task<T1> Match<T1>(Func<T, Task<T1>> ok, Func<string, Task<T1>> error)
        {
            switch (this)
            {
                case Ok<T> okResult:
                    return await ok(okResult.Value).ConfigureAwait(false);
                case Error<T> errorResult:
                    return await error(errorResult.Message).ConfigureAwait(false);
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }
        public Task<T1> Match<T1>(Func<T, Task<T1>> ok, Func<string, T1> error) => Match(ok, e => Task.FromResult(error(e)));
        public async Task Match(Func<T, Task> ok)
        {
            switch (this)
            {
                case Ok<T> okResult:
                    await ok(okResult.Value).ConfigureAwait(false);
                    break;
                case Error<T> _:
                    break;
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }
        public T Match(Func<string, T> error) => Match(v => v, error);

        #region Bind

        public Result<T1> Bind<T1>(Func<T, Result<T1>> bind)
        {
            switch (this)
            {
                case Ok<T> ok:
                    return bind(ok.Value);
                case Error<T> error:
                    return error.Convert<T1>();
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }
        public async Task<Result<T1>> Bind<T1>(Func<T, Task<Result<T1>>> bind)
        {
            switch (this)
            {
                case Ok<T> ok:
                    return await bind(ok.Value).ConfigureAwait(false);
                case Error<T> error:
                    return error.Convert<T1>();
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }

        #endregion
        
        // Maps
        public Result<T1> Map<T1>(Func<T, T1> map) 
            => Bind(value => Ok(map(value)));

        public Task<Result<T1>> Map<T1>(Func<T, Task<T1>> map) 
            => Bind(async value => Ok(await map(value).ConfigureAwait(false)));

        // Helpers
        public T GetValueOrDefault(Func<T> defaultValue = null)
            => Match(
                v => v,
                _ => defaultValue != null ? defaultValue() : default);

        public override string GetErrorOrDefault(Func<string> defaultValue = null)
            => Match(
                _ => defaultValue?.Invoke(),
                error => error);

        public T GetValueOrThrow()
            => Match(
                v => v,
                message => throw new InvalidOperationException($"Cannot access error result value. Error: {message}"));

        public IEnumerator<T> GetEnumerator() => Match(ok => new[]{ok}, error => Enumerable.Empty<T>()).GetEnumerator();

        public override string ToString() => Match(ok => $"Ok {ok?.ToString()}", error => $"Error {error}");
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    public sealed class Ok<T> : Result<T>
    {
        public T Value { get; }

        public Ok(T value) => Value = value;

        public bool Equals(Ok<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Ok<T> other && Equals(other);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);

        public static bool operator ==(Ok<T> left, Ok<T> right) => Equals(left, right);

        public static bool operator !=(Ok<T> left, Ok<T> right) => !Equals(left, right);
    }

    public sealed class Error<T> : Result<T>
    {
        public string Message { get; }

        public Error(string message) => Message = message;

        public Error<T1> Convert<T1>() => new Error<T1>(Message);

        public bool Equals(Error<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Error<T> other && Equals(other);
        }

        public override int GetHashCode() => Message != null ? Message.GetHashCode() : 0;

        public static bool operator ==(Error<T> left, Error<T> right) => Equals(left, right);

        public static bool operator !=(Error<T> left, Error<T> right) => !Equals(left, right);
    }

    public static class ResultExtensions
    {
        // Binds
        public static async Task<Result<T1>> Bind<T, T1>(
            this Task<Result<T>> result,
            Func<T, Result<T1>> bind) 
            => (await result.ConfigureAwait(false)).Bind(bind);

        public static async Task<Result<T1>> Bind<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<Result<T1>>> bind) 
            => await (await result.ConfigureAwait(false)).Bind(bind).ConfigureAwait(false);

        // Maps
        public static async Task<Result<T1>> Map<T, T1>(
            this Task<Result<T>> result,
            Func<T, T1> map) 
            => (await result.ConfigureAwait(false)).Map(map);

        public static Task<Result<T1>> Map<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> bind) 
            => Bind(result, async v => Result.Ok(await bind(v).ConfigureAwait(false)));

        //Matches
        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> ok,
            Func<string, Task<T1>> error) 
            => await (await result.ConfigureAwait(false)).Match(ok, error).ConfigureAwait(false);

        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> ok,
            Func<string, T1> error) 
            => await (await result.ConfigureAwait(false)).Match(ok, error).ConfigureAwait(false);

        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, T1> ok,
            Func<string, T1> error) 
            => (await result.ConfigureAwait(false)).Match(ok, error);

        // Aggregates
        public static Result<(T1, T2)> Aggregate<T1, T2>(
            this Result<T1> r1,
            Result<T2> r2,
            string errorSeparator = null) 
            => r1.Aggregate(r2, (v1, v2) => (v1, v2), errorSeparator);

        public static Result<TResult> Aggregate<T1, T2, TResult>(
            this Result<T1> r1,
            Result<T2> r2,
            Func<T1, T2, TResult> combine,
            string errorSeparator = null)
        {
            var errors = JoinErrorMessages(r1.Concat<Result>(r2), errorSeparator);
            return !string.IsNullOrEmpty(errors) 
                ? Result.Error<TResult>(errors) 
                : combine(r1.GetValueOrThrow(), r2.GetValueOrThrow());
        }

        public static Result<(T1, T2, T3)> Aggregate<T1, T2, T3>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            string errorSeparator = null)
            => r1.Aggregate(r2, r3, (v1, v2, v3) => (v1, v2, v3), errorSeparator);

        public static Result<TResult> Aggregate<T1, T2, T3, TResult>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Func<T1, T2, T3, TResult> combine,
            string errorSeparator = null)
        {
            var errors = JoinErrorMessages(r1.Concat<Result>(r2, r3), errorSeparator);
            return !string.IsNullOrEmpty(errors) 
                ? Result.Error<TResult>(errors) 
                : combine(r1.GetValueOrThrow(), r2.GetValueOrThrow(), r3.GetValueOrThrow());
        }

        public static Result<(T1, T2, T3, T4)> Aggregate<T1, T2, T3, T4>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            string errorSeparator = null)
            => r1.Aggregate(r2, r3, r4, (v1, v2, v3, v4) => (v1, v2, v3, v4), errorSeparator);

        public static Result<TResult> Aggregate<T1, T2, T3, T4, TResult>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            Func<T1, T2, T3, T4, TResult> combine,
            string errorSeparator = null)
        {
            var errors = JoinErrorMessages(r1.Concat<Result>(r2, r3, r4), errorSeparator);
            return !string.IsNullOrEmpty(errors) 
                ? Result.Error<TResult>(errors) 
                : combine(r1.GetValueOrThrow(), r2.GetValueOrThrow(), r3.GetValueOrThrow(), r4.GetValueOrThrow());
        }

        public static Result<(T1, T2, T3, T4, T5)> Aggregate<T1, T2, T3, T4, T5>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            Result<T5> r5,
            string errorSeparator = null)
            => r1.Aggregate(r2, r3, r4, r5, (v1, v2, v3, v4, v5) => (v1, v2, v3, v4, v5), errorSeparator);

        public static Result<TResult> Aggregate<T1, T2, T3, T4, T5, TResult>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            Result<T5> r5,
            Func<T1, T2, T3, T4, T5, TResult> combine,
            string errorSeparator = null)
        {
            var errors = JoinErrorMessages(r1.Concat<Result>(r2, r3, r4, r5), errorSeparator);
            return !string.IsNullOrEmpty(errors)
                ? Result.Error<TResult>(errors)
                : combine(
                    r1.GetValueOrThrow(), r2.GetValueOrThrow(), r3.GetValueOrThrow(),
                    r4.GetValueOrThrow(), r5.GetValueOrThrow());
        }

        public static Result<(T1, T2, T3, T4, T5, T6)> Aggregate<T1, T2, T3, T4, T5, T6>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            Result<T5> r5,
            Result<T6> r6,
            string errorSeparator = null)
            => r1.Aggregate(r2, r3, r4, r5, r6, (v1, v2, v3, v4, v5, v6) => (v1, v2, v3, v4, v5, v6), errorSeparator);

        public static Result<TResult> Aggregate<T1, T2, T3, T4, T5, T6, TResult>(
            this Result<T1> r1,
            Result<T2> r2,
            Result<T3> r3,
            Result<T4> r4,
            Result<T5> r5,
            Result<T6> r6,
            Func<T1, T2, T3, T4, T5, T6, TResult> combine,
            string errorSeparator = null)
        {
            var errors = JoinErrorMessages(r1.Concat<Result>(r2, r3, r4, r5, r6), errorSeparator);
            return !string.IsNullOrEmpty(errors)
                ? Result.Error<TResult>(errors)
                : combine(
                    r1.GetValueOrThrow(), r2.GetValueOrThrow(), r3.GetValueOrThrow(),
                    r4.GetValueOrThrow(), r5.GetValueOrThrow(), r6.GetValueOrThrow());
        }

        public static Result<List<T>> Aggregate<T>(
            this IEnumerable<Result<T>> results,
            string errorSeparator = null)
        {
            var sb = new StringBuilder();
            var oks = new List<T>();
            foreach (var result in results)
            {
                result.Match(
                    ok => oks.Add(ok),
                    error => sb.Append(error).Append(errorSeparator ?? Environment.NewLine));
            }

            var errors = sb.ToString();
            return !string.IsNullOrEmpty(errors) ? Result.Error<List<T>>(errors) : Result.Ok(oks);
        }

        public static async Task<Result<List<T>>> Aggregate<T>(
            this Task<IEnumerable<Result<T>>> results,
            string errorSeparator = null) 
            => (await results.ConfigureAwait(false))
                .Aggregate(errorSeparator);

        public static async Task<Result<List<T>>> Aggregate<T>(
            this IEnumerable<Task<Result<T>>> results,
            string errorSeparator = null) 
            => (await results.SelectAsync(e => e).ConfigureAwait(false))
                .Aggregate(errorSeparator);

        public static async Task<Result<List<T>>> Aggregate<T>(
            this IEnumerable<Task<Result<T>>> results,
            int maxDegreeOfParallelism,
            string errorSeparator = null)
            => (await results.SelectAsync(e => e, maxDegreeOfParallelism).ConfigureAwait(false))
                .Aggregate(errorSeparator);

        public static async Task<Result<List<T>>> AggregateMany<T>(
            this IEnumerable<Task<IEnumerable<Result<T>>>> results,
            string errorSeparator = null)
            => (await results.SelectAsync(e => e).ConfigureAwait(false))
                .SelectMany(e => e)
                .Aggregate(errorSeparator);

        public static async Task<Result<List<T>>> AggregateMany<T>(
            this IEnumerable<Task<IEnumerable<Result<T>>>> results,
            int maxDegreeOfParallelism,
            string errorSeparator = null)
            => (await results.SelectAsync(e => e, maxDegreeOfParallelism).ConfigureAwait(false))
                .SelectMany(e => e)
                .Aggregate(errorSeparator);

        // Chooses
        public static IEnumerable<T1> Choose<T, T1>(
            this IEnumerable<T> items,
            Func<T, Result<T1>> choose,
            Action<string> onError)
            => items.Select(i => choose(i))
                .Choose(onError);

        public static IEnumerable<T> Choose<T>(
            this IEnumerable<Result<T>> results,
            Action<string> onError) 
            => results
                .Where(r =>
                {
                    var isOk = r.IsOk;
                    if (!isOk)
                        onError(r.GetErrorOrDefault());
                    return isOk;
                })
                .Select(r => r.GetValueOrThrow());

        public static Option<T> ToOption<T>(this Result<T> result) => result.Match(ok => Option.Some(ok), _ => Option.None<T>());

        // Helpers
        private static string JoinErrorMessages(
            this IEnumerable<Result> results,
            string errorSeparator = null) 
            => string.Join(
                errorSeparator ?? Environment.NewLine,
                results.Where(r => r.IsError)
                    .Select(r => r.GetErrorOrDefault()));
    }
}