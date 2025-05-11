# S4LResReplacer
An S4L Resource Tool that reads and writes raw files from a folder

# Features
* Write any changed or new file to resource.s4hd
* Automatically cleans up the resources again after S4Client is closed to keep it's original state

# Usage
1. Download `S4LResReplacer.exe` from releases
2. Place it in the root directory of S4 League
3. Create .bat files, name them anything you want, preferable "res_write.bat" and "res_clean.bat" or similar
4. For write, enter `S4LResReplacer.exe --enable-custom`
5. For clean, enter `S4LResReplacer.exe --clean-only`
6. Create a folder named "Custom" inside the root directory of S4 League
7. Place your resource files in there and make sure they match the exact same location as seen in the resource tool
8. Run `res_write.bat` and it should write all added resources to S4 League
9. It now created a `resource.s4hd.bak` which will be used for `res_clean.bat` to revert all changes later on

You can bind the `S4LResReplacer` to a launcher as well and make it autorun when starting S4 and clean up again when S4 is closed.  
This allows you to always place any file you want inside `Custom` to be added/replaced and immediately clean up your game again to it's original state when exiting.  
It can be extremely powerful because you no longer have to overwrite the files again with a Resource Tool!

# Libraries
- [S4Zip](https://github.com/wtfblub/NetspherePirates/blob/dev/src/Netsphere.Resource/S4Zip.cs)
- [BlubLib](https://gitlab.com/wtfblub/BlubLib/-/tree/dev/src/BlubLib)
