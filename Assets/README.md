# Unity Barcode Scanner

# Folder Structure
* `Scripts` : The core of this library (if you want to use it in your project, just take this folder)
* `Samples` : Folder where you can find demo and learn how to use it
* `Editor` & `Resources` : Folder where are the unit tests and some sample files

# Install
You can reuse this library in any recent unity project:

## Manually
Download the project: https://github.com/kefniark/UnityBarcodeScanner/zipball/master
And just copy the `Assets/Scripts` folder into your project.
The code use namespaces, so they shouldn't conflict with anything else.

## Unity Package
Soon ...

## Git
To download only the code of this library into `Assets/Libraries/BarcodeScanner/` (not the samples, test or doc)
```bash
git remote add BarcodeScanner git@github.com:kefniark/UnityBarcodeScanner.git
git fetch BarcodeScanner
git read-tree --prefix=Assets/Libraries/BarcodeScanner -u BarcodeScanner/master:Assets/Scripts
```
Here is a example with read-tree (that just copy the code).
You can achieve the same kind of operation with `git subtree` or `git submodule`