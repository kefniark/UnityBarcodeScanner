using UnityEngine;

namespace BarcodeScanner
{
	public interface IScanner
	{
		string Decode(Color32[] colors, int width, int height);
	}
}