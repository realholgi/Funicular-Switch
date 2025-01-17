﻿#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FunicularSwitch;

namespace FunicularSwitch.Generators.Consumer
{
#pragma warning disable 1591
    public abstract partial class Result
    {
        public static Result<T> Error<T>(String details) => new Result<T>.Error_(details);
        public static Result<T> Ok<T>(T value) => new Result<T>.Ok_(value);
        public bool IsError => GetType().GetGenericTypeDefinition() == typeof(Result<>.Error_);
        public bool IsOk => !IsError;
        public abstract String? GetErrorOrDefault();

        public static Result<T> Try<T>(Func<T> action, Func<Exception, String> formatError)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                return Error<T>(formatError(e));
            }
        }

        public static async Task<Result<T>> Try<T>(Func<Task<T>> action, Func<Exception, String> formatError)
        {
            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Error<T>(formatError(e));
            }
        }
    }

    public abstract partial class Result<T> : Result, IEnumerable<T>
    {
        public static Result<T> Error(String message) => Error<T>(message);
        public static Result<T> Ok(T value) => Ok<T>(value);

        public static implicit operator Result<T>(T value) => Result.Ok(value);

        public static bool operator true(Result<T> result) => result.IsOk;
        public static bool operator false(Result<T> result) => result.IsError;

        public static bool operator !(Result<T> result) => result.IsError;

        //just here to suppress warning, never called because all subtypes (Ok_, Error_) implement Equals and GetHashCode
        bool Equals(Result<T> other) => this switch
        {
            Ok_ ok => ok.Equals((object)other),
            Error_ error => error.Equals((object)other),
            _ => throw new InvalidOperationException($"Unexpected type derived from {nameof(Result<T>)}")
        };

        public override int GetHashCode() => this switch
        {
            Ok_ ok => ok.GetHashCode(),
            Error_ error => error.GetHashCode(),
            _ => throw new InvalidOperationException($"Unexpected type derived from {nameof(Result<T>)}")
        };

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Result<T>)obj);
        }

        public static bool operator ==(Result<T>? left, Result<T>? right) => Equals(left, right);

        public static bool operator !=(Result<T>? left, Result<T>? right) => !Equals(left, right);

        public void Match(Action<T> ok, Action<String>? error = null) => Match(
            v =>
            {
                ok.Invoke(v);
                return 42;
            },
            err =>
            {
                error?.Invoke(err);
                return 42;
            });

        public T1 Match<T1>(Func<T, T1> ok, Func<String, T1> error)
        {
            return this switch
            {
                Ok_ okResult => ok(okResult.Value),
                Error_ errorResult => error(errorResult.Details),
                _ => throw new InvalidOperationException($"Unexpected derived result type: {GetType()}")
            };
        }

        public async Task<T1> Match<T1>(Func<T, Task<T1>> ok, Func<String, Task<T1>> error)
        {
            return this switch
            {
                Ok_ okResult => await ok(okResult.Value).ConfigureAwait(false),
                Error_ errorResult => await error(errorResult.Details).ConfigureAwait(false),
                _ => throw new InvalidOperationException($"Unexpected derived result type: {GetType()}")
            };
        }

        public Task<T1> Match<T1>(Func<T, Task<T1>> ok, Func<String, T1> error) =>
            Match(ok, e => Task.FromResult(error(e)));

        public async Task Match(Func<T, Task> ok)
        {
            if (this is Ok_ okResult) await ok(okResult.Value).ConfigureAwait(false);
        }

        public T Match(Func<String, T> error) => Match(v => v, error);

        public Result<T1> Bind<T1>(Func<T, Result<T1>> bind)
        {
            switch (this)
            {
                case Ok_ ok:
                    return bind(ok.Value);
                case Error_ error:
                    return error.Convert<T1>();
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }

        public async Task<Result<T1>> Bind<T1>(Func<T, Task<Result<T1>>> bind)
        {
            switch (this)
            {
                case Ok_ ok:
                    return await bind(ok.Value).ConfigureAwait(false);
                case Error_ error:
                    return error.Convert<T1>();
                default:
                    throw new InvalidOperationException($"Unexpected derived result type: {GetType()}");
            }
        }

        public Result<T1> Map<T1>(Func<T, T1> map)
            => Bind(value => Ok(map(value)));

        public Task<Result<T1>> Map<T1>(Func<T, Task<T1>> map)
            => Bind(async value => Ok(await map(value).ConfigureAwait(false)));

        public T? GetValueOrDefault()
	        => Match(
		        v => (T?)v,
		        _ => default
	        );

        public T GetValueOrDefault(Func<T> defaultValue)
	        => Match(
		        v => v,
		        _ => defaultValue()
	        );

        public T GetValueOrDefault(T defaultValue)
	        => Match(
		        v => v,
		        _ => defaultValue
	        );

        public T GetValueOrThrow()
            => Match(
                v => v,
                details => throw new InvalidOperationException($"Cannot access error result value. Error: {details}"));

        public IEnumerator<T> GetEnumerator() => Match(ok => new[] { ok }, _ => Enumerable.Empty<T>()).GetEnumerator();

        public override string ToString() => Match(ok => $"Ok {ok?.ToString()}", error => $"Error {error}");
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public sealed partial class Ok_ : Result<T>
        {
            public T Value { get; }

            public Ok_(T value) => Value = value;

            public override String? GetErrorOrDefault() => null;

            public bool Equals(Ok_? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is Ok_ other && Equals(other);
            }

            public override int GetHashCode() => Value == null ? 0 : EqualityComparer<T>.Default.GetHashCode(Value);

            public static bool operator ==(Ok_ left, Ok_ right) => Equals(left, right);

            public static bool operator !=(Ok_ left, Ok_ right) => !Equals(left, right);
        }

        public sealed partial class Error_ : Result<T>
        {
            public String Details { get; }

            public Error_(String details) => Details = details;

            public Result<T1>.Error_ Convert<T1>() => new Result<T1>.Error_(Details);

            public override String? GetErrorOrDefault() => Details;

            public bool Equals(Error_? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(Details, other.Details);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is Error_ other && Equals(other);
            }

            public override int GetHashCode() => Details.GetHashCode();

            public static bool operator ==(Error_ left, Error_ right) => Equals(left, right);

            public static bool operator !=(Error_ left, Error_ right) => !Equals(left, right);
        }

    }

    public static partial class ResultExtension
    {
        #region bind

        public static async Task<Result<T1>> Bind<T, T1>(
            this Task<Result<T>> result,
            Func<T, Result<T1>> bind)
            => (await result.ConfigureAwait(false)).Bind(bind);

        public static async Task<Result<T1>> Bind<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<Result<T1>>> bind)
            => await (await result.ConfigureAwait(false)).Bind(bind).ConfigureAwait(false);

        #endregion

        #region map

        public static async Task<Result<T1>> Map<T, T1>(
            this Task<Result<T>> result,
            Func<T, T1> map)
            => (await result.ConfigureAwait(false)).Map(map);

        public static Task<Result<T1>> Map<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> bind)
            => Bind(result, async v => Result.Ok(await bind(v).ConfigureAwait(false)));

        public static Result<T> MapError<T>(this Result<T> result, Func<String, String> mapError) =>
            result.Match(ok => ok, error => Result.Error<T>(mapError(error)));

        #endregion

        #region match

        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> ok,
            Func<String, Task<T1>> error)
            => await (await result.ConfigureAwait(false)).Match(ok, error).ConfigureAwait(false);

        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, Task<T1>> ok,
            Func<String, T1> error)
            => await (await result.ConfigureAwait(false)).Match(ok, error).ConfigureAwait(false);

        public static async Task<T1> Match<T, T1>(
            this Task<Result<T>> result,
            Func<T, T1> ok,
            Func<String, T1> error)
            => (await result.ConfigureAwait(false)).Match(ok, error);

        #endregion

        public static Result<T> Flatten<T>(this Result<Result<T>> result) => result.Bind(r => r);

        public static Result<T1> As<T, T1>(this Result<T> result, Func<String> errorTIsNotT1) =>
            result.Bind(r =>
            {
                if (r is T1 converted)
                    return converted;
                return Result.Error<T1>(errorTIsNotT1());
            });

        public static Result<T1> As<T1>(this Result<object> result, Func<String> errorIsNotT1) =>
            result.As<object, T1>(errorIsNotT1);
    }
}

namespace FunicularSwitch.Generators.Consumer.Extensions
{
    public static partial class ResultExtension
    {
        public static IEnumerable<T1> Choose<T, T1>(
            this IEnumerable<T> items,
            Func<T, Result<T1>> choose,
            Action<String> onError)
            => items
                .Select(i => choose(i))
                .Choose(onError);

        public static IEnumerable<T> Choose<T>(
            this IEnumerable<Result<T>> results,
            Action<String> onError)
            => results
                .Where(r =>
                    r.Match(_ => true, error =>
                    {
                        onError(error);
                        return false;
                    }))
                .Select(r => r.GetValueOrThrow());

        public static Result<T> As<T>(this object item, Func<String> error) =>
            !(item is T t) ? Result.Error<T>(error()) : t;

        public static Result<T> NotNull<T>(this T? item, Func<String> error) =>
            item ?? Result.Error<T>(error());

        public static Result<string> NotNullOrEmpty(this string? s, Func<String> error)
            => string.IsNullOrEmpty(s) ? Result.Error<string>(error()) : s!;

        public static Result<string> NotNullOrWhiteSpace(this string? s, Func<String> error)
            => string.IsNullOrWhiteSpace(s) ? Result.Error<string>(error()) : s!;

        public static Result<T> First<T>(this IEnumerable<T> candidates, Func<T, bool> predicate, Func<String> noMatch) =>
            candidates
                .FirstOrDefault(i => predicate(i))
                .NotNull(noMatch);
    }
#pragma warning restore 1591
}
