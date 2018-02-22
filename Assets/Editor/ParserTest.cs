using UnityEngine;
using NUnit.Framework;
using BarcodeScanner;
using BarcodeScanner.Parser;

[TestFixture]
public class ParserTest
{
	#region Test Failure

	[Test]
	public void TestErrors()
	{
		IParser parser = new ZXingParser();
		ParserResult result = parser.Decode(new Color32[0], 0, 0);
		Assert.IsNull(result);
	}

	[Test]
	public void TestEmpty()
	{
		IParser parser = new ZXingParser();
		var image = Resources.Load<Texture2D>("standard");
		ParserResult result = parser.Decode(image.GetPixels32(), image.width, image.height);
		Assert.IsNull(result);
	}

	#endregion

	#region Test Barcode Types

	private static object[] ImageTests = {
		// 1D
		new object[] { "code_39", "google"},
		new object[] { "code_128", "google"},
		new object[] { "code_isbn", "9783161484100" },

		// 2D
		new object[] { "code_qr", "google"},
		new object[] { "code_datamatrix", "google"},
		new object[] { "code_aztec", "google"}
	};

	[Test, TestCaseSource("ImageTests")]
	public void TestCodes(string file, string check)
	{
		IParser parser = new ZXingParser();
		var image = Resources.Load<Texture2D>(file);
		ParserResult result = parser.Decode(image.GetPixels32(), image.width, image.height);
		StringAssert.Contains(check, result.Value.ToLowerInvariant());
	}

	#endregion

	#region Test Samples

	static object[] ImageSamples = {
		new object[] {"sample_giant_qr", "september" },
		new object[] {"sample_screen_blurry_qr", "http" }
	};

	[Test, TestCaseSource("ImageSamples")]
	public void TestSamples(string file, string check)
	{
		IParser parser = new ZXingParser();
		var image = Resources.Load<Texture2D>(file);
		ParserResult result = parser.Decode(image.GetPixels32(), image.width, image.height);
		StringAssert.Contains(check, result.Value.ToLowerInvariant());
	}

	#endregion
}
