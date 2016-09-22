# Unity Barcode Scanner
[![Build Status](https://travis-ci.org/kefniark/UnityBarcodeScanner.svg?branch=master)](https://travis-ci.org/kefniark/UnityBarcodeScanner)

Since months, I was looking for a good way to parse QRCodes (and generic barcode) in Unity.
On the Asset Store, few library are already providing that, but they are expensive, overly complex, not tested and not always maintained.
So, I just want to do something simple, readable, cross-plateform and open source.

Tested with Unity 5.3.x, 5.4.x

# How it's working ?
This project is a Unity Project that you can open directly.

Every piece of this library is separated and can be replaced if needed
* Camera : use directly the API of unity to access the webcam (available on iOS, Android, Windows, Mac, Linux, ...)
* Parsing : use zxing to extract the data https://zxingnet.codeplex.com/ (Apache 2)
* ContinuousScanner : use both other classes & .net threading library

And that's it, few C# files and a dll

# How to use it ?
* [Example](Assets/Samples/)
* [Install](Assets/)
* [Tests](Assets/Editor/)

# License
Under license WTFPL (http://www.wtfpl.net/about/)