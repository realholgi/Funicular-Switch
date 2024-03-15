﻿//HintName: OuterBaseMatchExtension.g.cs
#pragma warning disable 1591
public static partial class BaseMatchExtension
{
	public static T Match<T>(this Outer.Base @base, global::System.Func<Outer.One, T> one, global::System.Func<Outer.Two, T> two) =>
	@base switch
	{
		Outer.One case1 => one(case1),
		Outer.Two case2 => two(case2),
		_ => throw new global::System.ArgumentException($"Unknown type derived from Outer.Base: {@base.GetType().Name}")
	};
	
	public static global::System.Threading.Tasks.Task<T> Match<T>(this Outer.Base @base, global::System.Func<Outer.One, global::System.Threading.Tasks.Task<T>> one, global::System.Func<Outer.Two, global::System.Threading.Tasks.Task<T>> two) =>
	@base switch
	{
		Outer.One case1 => one(case1),
		Outer.Two case2 => two(case2),
		_ => throw new global::System.ArgumentException($"Unknown type derived from Outer.Base: {@base.GetType().Name}")
	};
	
	public static async global::System.Threading.Tasks.Task<T> Match<T>(this global::System.Threading.Tasks.Task<Outer.Base> @base, global::System.Func<Outer.One, T> one, global::System.Func<Outer.Two, T> two) =>
	(await @base.ConfigureAwait(false)).Match(one, two);
	
	public static async global::System.Threading.Tasks.Task<T> Match<T>(this global::System.Threading.Tasks.Task<Outer.Base> @base, global::System.Func<Outer.One, global::System.Threading.Tasks.Task<T>> one, global::System.Func<Outer.Two, global::System.Threading.Tasks.Task<T>> two) =>
	await (await @base.ConfigureAwait(false)).Match(one, two).ConfigureAwait(false);
	
	public static void Switch(this Outer.Base @base, global::System.Action<Outer.One> one, global::System.Action<Outer.Two> two)
	{
		switch (@base)
		{
			case Outer.One case1:
				one(case1);
				break;
			case Outer.Two case2:
				two(case2);
				break;
			default:
				throw new global::System.ArgumentException($"Unknown type derived from Outer.Base: {@base.GetType().Name}");
		}
	}
	
	public static async global::System.Threading.Tasks.Task Switch(this Outer.Base @base, global::System.Func<Outer.One, global::System.Threading.Tasks.Task> one, global::System.Func<Outer.Two, global::System.Threading.Tasks.Task> two)
	{
		switch (@base)
		{
			case Outer.One case1:
				await one(case1).ConfigureAwait(false);
				break;
			case Outer.Two case2:
				await two(case2).ConfigureAwait(false);
				break;
			default:
				throw new global::System.ArgumentException($"Unknown type derived from Outer.Base: {@base.GetType().Name}");
		}
	}
	
	public static async global::System.Threading.Tasks.Task Switch(this global::System.Threading.Tasks.Task<Outer.Base> @base, global::System.Action<Outer.One> one, global::System.Action<Outer.Two> two) =>
	(await @base.ConfigureAwait(false)).Switch(one, two);
	
	public static async global::System.Threading.Tasks.Task Switch(this global::System.Threading.Tasks.Task<Outer.Base> @base, global::System.Func<Outer.One, global::System.Threading.Tasks.Task> one, global::System.Func<Outer.Two, global::System.Threading.Tasks.Task> two) =>
	await (await @base.ConfigureAwait(false)).Switch(one, two).ConfigureAwait(false);
}
#pragma warning restore 1591
