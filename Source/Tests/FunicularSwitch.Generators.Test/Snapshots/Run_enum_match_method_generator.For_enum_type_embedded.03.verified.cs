﻿//HintName: FunicularSwitch.Test.Outer.testMatchExtension.g.cs
#pragma warning disable 1591
using System;
using System.Threading.Tasks;

namespace FunicularSwitch.Test
{
	public static partial class MatchExtension
	{
		public static T Match<T>(this FunicularSwitch.Test.Outer.test test, Func<T> one, Func<T> two) =>
		test switch
		{
			FunicularSwitch.Test.Outer.test.one => one(),
			FunicularSwitch.Test.Outer.test.two => two(),
			_ => throw new ArgumentException($"Unknown enum value from FunicularSwitch.Test.Outer.test: {test.GetType().Name}")
		};
		
		public static Task<T> Match<T>(this FunicularSwitch.Test.Outer.test test, Func<Task<T>> one, Func<Task<T>> two) =>
		test switch
		{
			FunicularSwitch.Test.Outer.test.one => one(),
			FunicularSwitch.Test.Outer.test.two => two(),
			_ => throw new ArgumentException($"Unknown enum value from FunicularSwitch.Test.Outer.test: {test.GetType().Name}")
		};
		
		public static async Task<T> Match<T>(this Task<FunicularSwitch.Test.Outer.test> test, Func<T> one, Func<T> two) =>
		(await test.ConfigureAwait(false)).Match(one, two);
		
		public static async Task<T> Match<T>(this Task<FunicularSwitch.Test.Outer.test> test, Func<Task<T>> one, Func<Task<T>> two) =>
		await (await test.ConfigureAwait(false)).Match(one, two).ConfigureAwait(false);
		
		public static void Switch(this FunicularSwitch.Test.Outer.test test, Action one, Action two)
		{
			switch (test)
			{
				case FunicularSwitch.Test.Outer.test.one:
					one();
					break;
				case FunicularSwitch.Test.Outer.test.two:
					two();
					break;
				default:
					throw new ArgumentException($"Unknown enum value from FunicularSwitch.Test.Outer.test: {test.GetType().Name}");
			}
		}
		
		public static async Task Switch(this FunicularSwitch.Test.Outer.test test, Func<Task> one, Func<Task> two)
		{
			switch (test)
			{
				case FunicularSwitch.Test.Outer.test.one:
					await one().ConfigureAwait(false);
					break;
				case FunicularSwitch.Test.Outer.test.two:
					await two().ConfigureAwait(false);
					break;
				default:
					throw new ArgumentException($"Unknown enum value from FunicularSwitch.Test.Outer.test: {test.GetType().Name}");
			}
		}
		
		public static async Task Switch(this Task<FunicularSwitch.Test.Outer.test> test, Action one, Action two) =>
		(await test.ConfigureAwait(false)).Switch(one, two);
		
		public static async Task Switch(this Task<FunicularSwitch.Test.Outer.test> test, Func<Task> one, Func<Task> two) =>
		await (await test.ConfigureAwait(false)).Switch(one, two).ConfigureAwait(false);
	}
}
#pragma warning restore 1591
