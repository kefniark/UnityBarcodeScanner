using System;
using UnityEngine;
using Wizcorp.Utils.Logger;
using ZXing;

namespace BarcodeScanner.Parser
{
	public class ZXingParser : IParser
	{
		public BarcodeReader Scanner { get; private set; }

		public ZXingParser()
		{
			Scanner = new BarcodeReader();
			Scanner.AutoRotate = true;
			Scanner.TryInverted = true;
			//Scanner.Options.TryHarder = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="colors"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public ParserResult Decode(Color32[] colors, int width, int height)
		{
			ParserResult value = null;
			try
			{
				var result = Scanner.Decode(colors, width, height);
				if (result != null)
				{
					value = new ParserResult(result.BarcodeFormat.ToString(), result.Text);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			
			return value;
		}
	}
}
