﻿//HintName: FunicularSwitchTestIBaseMatchExtension.g.cs
#pragma warning disable 1591
namespace FunicularSwitch.Test
{
	public static partial class IBaseMatchExtension
	{
		public static T Match<T>(this FunicularSwitch.Test.IBase iBase, global::System.Func<FunicularSwitch.Test.One, T> one, global::System.Func<FunicularSwitch.Test.Two, T> two) =>
		iBase switch
		{
			FunicularSwitch.Test.One case1 => one(case1),
			FunicularSwitch.Test.Two case2 => two(case2),
			_ => throw new global::System.ArgumentException($"Unknown type derived from FunicularSwitch.Test.IBase: {iBase.GetType().Name}")
		};
		
		public static global::System.Threading.Tasks.Task<T> Match<T>(this FunicularSwitch.Test.IBase iBase, global::System.Func<FunicularSwitch.Test.One, global::System.Threading.Tasks.Task<T>> one, global::System.Func<FunicularSwitch.Test.Two, global::System.Threading.Tasks.Task<T>> two) =>
		iBase switch
		{
			FunicularSwitch.Test.One case1 => one(case1),
			FunicularSwitch.Test.Two case2 => two(case2),
			_ => throw new global::System.ArgumentException($"Unknown type derived from FunicularSwitch.Test.IBase: {iBase.GetType().Name}")
		};
		
		public static async global::System.Threading.Tasks.Task<T> Match<T>(this global::System.Threading.Tasks.Task<FunicularSwitch.Test.IBase> iBase, global::System.Func<FunicularSwitch.Test.One, T> one, global::System.Func<FunicularSwitch.Test.Two, T> two) =>
		(await iBase.ConfigureAwait(false)).Match(one, two);
		
		public static async global::System.Threading.Tasks.Task<T> Match<T>(this global::System.Threading.Tasks.Task<FunicularSwitch.Test.IBase> iBase, global::System.Func<FunicularSwitch.Test.One, global::System.Threading.Tasks.Task<T>> one, global::System.Func<FunicularSwitch.Test.Two, global::System.Threading.Tasks.Task<T>> two) =>
		await (await iBase.ConfigureAwait(false)).Match(one, two).ConfigureAwait(false);
		
		public static void Switch(this FunicularSwitch.Test.IBase iBase, global::System.Action<FunicularSwitch.Test.One> one, global::System.Action<FunicularSwitch.Test.Two> two)
		{
			switch (iBase)
			{
				case FunicularSwitch.Test.One case1:
					one(case1);
					break;
				case FunicularSwitch.Test.Two case2:
					two(case2);
					break;
				default:
					throw new global::System.ArgumentException($"Unknown type derived from FunicularSwitch.Test.IBase: {iBase.GetType().Name}");
			}
		}
		
		public static async global::System.Threading.Tasks.Task Switch(this FunicularSwitch.Test.IBase iBase, global::System.Func<FunicularSwitch.Test.One, global::System.Threading.Tasks.Task> one, global::System.Func<FunicularSwitch.Test.Two, global::System.Threading.Tasks.Task> two)
		{
			switch (iBase)
			{
				case FunicularSwitch.Test.One case1:
					await one(case1).ConfigureAwait(false);
					break;
				case FunicularSwitch.Test.Two case2:
					await two(case2).ConfigureAwait(false);
					break;
				default:
					throw new global::System.ArgumentException($"Unknown type derived from FunicularSwitch.Test.IBase: {iBase.GetType().Name}");
			}
		}
		
		public static async global::System.Threading.Tasks.Task Switch(this global::System.Threading.Tasks.Task<FunicularSwitch.Test.IBase> iBase, global::System.Action<FunicularSwitch.Test.One> one, global::System.Action<FunicularSwitch.Test.Two> two) =>
		(await iBase.ConfigureAwait(false)).Switch(one, two);
		
		public static async global::System.Threading.Tasks.Task Switch(this global::System.Threading.Tasks.Task<FunicularSwitch.Test.IBase> iBase, global::System.Func<FunicularSwitch.Test.One, global::System.Threading.Tasks.Task> one, global::System.Func<FunicularSwitch.Test.Two, global::System.Threading.Tasks.Task> two) =>
		await (await iBase.ConfigureAwait(false)).Switch(one, two).ConfigureAwait(false);
	}
}
#pragma warning restore 1591
