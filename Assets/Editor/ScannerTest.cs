using UnityEngine;
using NUnit.Framework;
using BarcodeScanner;

[TestFixture]
public class ScannerTest
{
	#region Test Failure

	[Test]
	public void TestErrors()
	{
		IScanner scanner = new ZXingScanner();
		string value = scanner.Decode(new Color32[0], 0, 0);
		Assert.AreEqual(value.Length, 0);
	}

	[Test]
	public void TestEmpty()
	{
		IScanner scanner = new ZXingScanner();
		var image = Resources.Load<Texture2D>("standard");
		string value = scanner.Decode(image.GetPixels32(), image.width, image.height);
		Assert.AreEqual(value.Length, 0);
	}

	#endregion

	#region Test Barcode Types

	static string[] ImageTests = {
		"code_39",
		"code_128",
		"code_qr"
	};

	[Test, TestCaseSource("ImageTests")]
	public void TestCodes(string file)
	{
		IScanner scanner = new ZXingScanner();
		var image = Resources.Load<Texture2D>(file);
		string value = scanner.Decode(image.GetPixels32(), image.width, image.height);
		StringAssert.Contains("google", value.ToLowerInvariant());
	}

	#endregion

	#region Test Samples

	static object[] ImageSamples = {
		new object[] {"sample_giant_qr", "september" },
		new object[] {"sample_screen_blurry_qr", "http" }
	};

	[Test, TestCaseSource("ImageSamples")]
	public void TestSamples(string file, string result)
	{
		IScanner scanner = new ZXingScanner();
		var image = Resources.Load<Texture2D>(file);
		string value = scanner.Decode(image.GetPixels32(), image.width, image.height);
		StringAssert.Contains(result, value.ToLowerInvariant());
	}

	#endregion
}
