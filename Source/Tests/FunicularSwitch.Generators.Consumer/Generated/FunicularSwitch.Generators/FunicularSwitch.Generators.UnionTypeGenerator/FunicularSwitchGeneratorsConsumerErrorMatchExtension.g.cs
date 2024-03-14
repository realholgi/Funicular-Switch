﻿#pragma warning disable 1591
namespace FunicularSwitch.Generators.Consumer
{
	public static partial class ErrorMatchExtension
	{
		public static T Match<T>(this FunicularSwitch.Generators.Consumer.Error error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, T> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, T> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, T> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, T> aggregated) =>
		error switch
		{
			FunicularSwitch.Generators.Consumer.Error.Generic_ case1 => generic(case1),
			FunicularSwitch.Generators.Consumer.Error.NotFound_ case2 => notFound(case2),
			FunicularSwitch.Generators.Consumer.Error.NotAuthorized_ case3 => notAuthorized(case3),
			FunicularSwitch.Generators.Consumer.Error.Aggregated_ case4 => aggregated(case4),
			_ => throw new System.ArgumentException($"Unknown type derived from FunicularSwitch.Generators.Consumer.Error: {error.GetType().Name}")
		};
		
		public static System.Threading.Tasks.Task<T> Match<T>(this FunicularSwitch.Generators.Consumer.Error error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, System.Threading.Tasks.Task<T>> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, System.Threading.Tasks.Task<T>> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, System.Threading.Tasks.Task<T>> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, System.Threading.Tasks.Task<T>> aggregated) =>
		error switch
		{
			FunicularSwitch.Generators.Consumer.Error.Generic_ case1 => generic(case1),
			FunicularSwitch.Generators.Consumer.Error.NotFound_ case2 => notFound(case2),
			FunicularSwitch.Generators.Consumer.Error.NotAuthorized_ case3 => notAuthorized(case3),
			FunicularSwitch.Generators.Consumer.Error.Aggregated_ case4 => aggregated(case4),
			_ => throw new System.ArgumentException($"Unknown type derived from FunicularSwitch.Generators.Consumer.Error: {error.GetType().Name}")
		};
		
		public static async System.Threading.Tasks.Task<T> Match<T>(this System.Threading.Tasks.Task<FunicularSwitch.Generators.Consumer.Error> error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, T> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, T> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, T> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, T> aggregated) =>
		(await error.ConfigureAwait(false)).Match(generic, notFound, notAuthorized, aggregated);
		
		public static async System.Threading.Tasks.Task<T> Match<T>(this System.Threading.Tasks.Task<FunicularSwitch.Generators.Consumer.Error> error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, System.Threading.Tasks.Task<T>> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, System.Threading.Tasks.Task<T>> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, System.Threading.Tasks.Task<T>> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, System.Threading.Tasks.Task<T>> aggregated) =>
		await (await error.ConfigureAwait(false)).Match(generic, notFound, notAuthorized, aggregated).ConfigureAwait(false);
		
		public static void Switch(this FunicularSwitch.Generators.Consumer.Error error, System.Action<FunicularSwitch.Generators.Consumer.Error.Generic_> generic, System.Action<FunicularSwitch.Generators.Consumer.Error.NotFound_> notFound, System.Action<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_> notAuthorized, System.Action<FunicularSwitch.Generators.Consumer.Error.Aggregated_> aggregated)
		{
			switch (error)
			{
				case FunicularSwitch.Generators.Consumer.Error.Generic_ case1:
					generic(case1);
					break;
				case FunicularSwitch.Generators.Consumer.Error.NotFound_ case2:
					notFound(case2);
					break;
				case FunicularSwitch.Generators.Consumer.Error.NotAuthorized_ case3:
					notAuthorized(case3);
					break;
				case FunicularSwitch.Generators.Consumer.Error.Aggregated_ case4:
					aggregated(case4);
					break;
				default:
					throw new System.ArgumentException($"Unknown type derived from FunicularSwitch.Generators.Consumer.Error: {error.GetType().Name}");
			}
		}
		
		public static async System.Threading.Tasks.Task Switch(this FunicularSwitch.Generators.Consumer.Error error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, System.Threading.Tasks.Task> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, System.Threading.Tasks.Task> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, System.Threading.Tasks.Task> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, System.Threading.Tasks.Task> aggregated)
		{
			switch (error)
			{
				case FunicularSwitch.Generators.Consumer.Error.Generic_ case1:
					await generic(case1).ConfigureAwait(false);
					break;
				case FunicularSwitch.Generators.Consumer.Error.NotFound_ case2:
					await notFound(case2).ConfigureAwait(false);
					break;
				case FunicularSwitch.Generators.Consumer.Error.NotAuthorized_ case3:
					await notAuthorized(case3).ConfigureAwait(false);
					break;
				case FunicularSwitch.Generators.Consumer.Error.Aggregated_ case4:
					await aggregated(case4).ConfigureAwait(false);
					break;
				default:
					throw new System.ArgumentException($"Unknown type derived from FunicularSwitch.Generators.Consumer.Error: {error.GetType().Name}");
			}
		}
		
		public static async System.Threading.Tasks.Task Switch(this System.Threading.Tasks.Task<FunicularSwitch.Generators.Consumer.Error> error, System.Action<FunicularSwitch.Generators.Consumer.Error.Generic_> generic, System.Action<FunicularSwitch.Generators.Consumer.Error.NotFound_> notFound, System.Action<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_> notAuthorized, System.Action<FunicularSwitch.Generators.Consumer.Error.Aggregated_> aggregated) =>
		(await error.ConfigureAwait(false)).Switch(generic, notFound, notAuthorized, aggregated);
		
		public static async System.Threading.Tasks.Task Switch(this System.Threading.Tasks.Task<FunicularSwitch.Generators.Consumer.Error> error, System.Func<FunicularSwitch.Generators.Consumer.Error.Generic_, System.Threading.Tasks.Task> generic, System.Func<FunicularSwitch.Generators.Consumer.Error.NotFound_, System.Threading.Tasks.Task> notFound, System.Func<FunicularSwitch.Generators.Consumer.Error.NotAuthorized_, System.Threading.Tasks.Task> notAuthorized, System.Func<FunicularSwitch.Generators.Consumer.Error.Aggregated_, System.Threading.Tasks.Task> aggregated) =>
		await (await error.ConfigureAwait(false)).Switch(generic, notFound, notAuthorized, aggregated).ConfigureAwait(false);
	}
	
	public abstract partial class Error
	{
		public static FunicularSwitch.Generators.Consumer.Error Generic(string message) => new FunicularSwitch.Generators.Consumer.Error.Generic_(message);
		public static FunicularSwitch.Generators.Consumer.Error NotFound() => new FunicularSwitch.Generators.Consumer.Error.NotFound_();
		public static FunicularSwitch.Generators.Consumer.Error NotAuthorized() => new FunicularSwitch.Generators.Consumer.Error.NotAuthorized_();
		public static FunicularSwitch.Generators.Consumer.Error Aggregated(global::System.Collections.Immutable.ImmutableList<global::FunicularSwitch.Generators.Consumer.Error> errors) => new FunicularSwitch.Generators.Consumer.Error.Aggregated_(errors);
	}
}
#pragma warning restore 1591
