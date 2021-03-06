using System;
using TinyJSON;
using NUnit.Framework;
using System.Collections.Generic;


[TestFixture]
public class TestClassType
{
	public static bool encodeCallbackFired = false;

	class TestClass
	{
		public int x;
		public int y;

		[Exclude] 
		public int z;

		public List<int> list;


		[AfterDecode]
		public void AfterDecode()
		{
			TestClassType.encodeCallbackFired = true;
		}
	}


	[Test]
	public void TestDumpClass()
	{
		var testClass = new TestClass() { x = 5, y = 7, z = 0 };
		testClass.list = new List<int>() { { 3 }, { 1 }, { 4 } };

		Assert.AreEqual( "{\"@type\":\"" + testClass.GetType().FullName + "\",\"x\":5,\"y\":7,\"list\":[3,1,4]}", JSON.Dump( testClass ) );
	}


	[Test]
	public void TestDumpClassNoTypeHint()
	{
		var testClass = new TestClass() { x = 5, y = 7, z = 0 };
		testClass.list = new List<int>() { { 3 }, { 1 }, { 4 } };

		Assert.AreEqual( "{\"x\":5,\"y\":7,\"list\":[3,1,4]}", JSON.Dump( testClass, EncodeOptions.NoTypeHints ) );
	}


	[Test]
	public void TestDumpClassPrettyPrint()
	{
		var testClass = new TestClass() { x = 5, y = 7, z = 0 };
		testClass.list = new List<int>() { { 3 }, { 1 }, { 4 } };

		Assert.AreEqual( @"{
	""x"": 5,
	""y"": 7,
	""list"": [
		3,
		1,
		4
	]
}", JSON.Dump( testClass, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints ) );
	}


	[Test]
	public void TestLoadStruct()
	{
		TestClass testClass = JSON.Load( "{\"x\":5,\"y\":7,\"z\":3,\"list\":[3,1,4]}}" ).Make<TestClass>();

		Assert.AreEqual( 5, testClass.x );
		Assert.AreEqual( 7, testClass.y );
		Assert.AreEqual( 0, testClass.z ); // should not get assigned

		Assert.AreEqual( 3, testClass.list.Count );
		Assert.AreEqual( 3, testClass.list[0] );
		Assert.AreEqual( 1, testClass.list[1] );
		Assert.AreEqual( 4, testClass.list[2] );

		Assert.IsTrue( encodeCallbackFired );
	}

}

