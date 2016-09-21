using System;
using UnityEngine;
using ZXing;

namespace BarcodeScanner
{
	public class ZXingScanner : IScanner
	{
		public BarcodeReader Scanner { get; private set; }

		public ZXingScanner()
		{
			Scanner = new BarcodeReader();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="colors"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public string Decode(Color32[] colors, int width, int height)
		{
			var value = string.Empty;
			try
			{
				var result = Scanner.Decode(colors, width, height);
				if (result != null)
				{
					value = result.Text;
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
			
			return value;
		}
	}
}
