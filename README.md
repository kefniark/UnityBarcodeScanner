# Unity Barcode Scanner
Since months, I was looking for a good way to parse QRCodes (and generic barcode) in Unity.
On the Asset Store, few library are already providing that, but they are expensive, overly complex, not tested and not always maintained.
So, I just want to do something simple, readable, cross-plateform and open source.

# How it's working ?
Every piece of this library is separated and can be replaced if needed
* Camera : use directly the API of unity to access the webcam (available on iOS, Android, Windows, Mac, Linux, ...)
* Parsing : use zxing to extract the data https://zxingnet.codeplex.com/ (Apache 2)
* ContinuousScanner : use both other classes & c# threading library
And that's it, few C# files and a dll

# Contents
* `Samples` : Folder where you can find demo
* `Scripts` : The core of this repo (if you want to use this repo in your project, just take this folder)
* `Editor` & `Resources` : Folder where are the unit tests and some sample files

# Usage
Soon ...

# License
Under license WTFPL (http://www.wtfpl.net/about/)