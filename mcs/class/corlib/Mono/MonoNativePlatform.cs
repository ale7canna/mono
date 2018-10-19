//
// MonoNativePlatform.cs
//
// Author:
//       Martin Baulig <mabaul@microsoft.com>
//
// Copyright (c) 2018 Xamarin, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mono
{
	static class MonoNativePlatform
	{
		[DllImport ("System.Native")]
		extern static int mono_native_get_platform_type ();

		public static MonoNativePlatformType GetPlatformType ()
		{
			return (MonoNativePlatformType)mono_native_get_platform_type ();
		}

		[MethodImpl (MethodImplOptions.InternalCall)]
		extern static void MartinTest ();

		[DllImport ("System.Native")]
		extern static int mono_native_initialize ();

		public static void Initialize ()
		{
			Console.Error.WriteLine ($"MONO NATIVE INITIALIZE!");
			mono_native_initialize ();
			Console.Error.WriteLine ($"MONO NATIVE INITIALIZE #1!");
			MartinTest ();
			Console.Error.WriteLine ($"MONO NATIVE INITIALIZE #2!");
		}
	}
}
