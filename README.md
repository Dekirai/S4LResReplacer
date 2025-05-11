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

You can bind the `S4LResReplacer` to a launcher as well and make it autorun when starting S4 and clean up again when S4 is closed (--clean-only auto-checks if s4 is closed or not).  
This allows you to always place any file you want inside `Custom` to be added/replaced and immediately clean up your game again to it's original state when exiting.  
It can be extremely powerful because you no longer have to overwrite the files again with a Resource Tool!

## Example for adding it to a launcher
```csharp
private async void bt_StartGame_Click(object sender, RoutedEventArgs e)
{
    bt_StartGame.IsEnabled = false;
    if (chbk_CustomResources.IsChecked == true)
    {
        try
        {
            string baseDir = AppContext.BaseDirectory;

            string exePath = Path.Combine(baseDir, "S4LResReplacer.exe");

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                WorkingDirectory = baseDir,
                Arguments = "--enable-custom",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using var proc = Process.Start(psi);
            if (proc != null)
            {
                await proc.WaitForExitAsync();
            }
        }
        catch (Exception ex)
        {
            //Nothing
        }
    }
    await Task.Run(() => LoginClient.Connect(Constants.ConnectEndPoint));
    Properties.Settings.Default.username = tb_Username.Text;
    Properties.Settings.Default.password = tb_Password.Password;
    Properties.Settings.Default.Save();

    try
    {
        string baseDir = AppContext.BaseDirectory;

        string exePath = Path.Combine(baseDir, "S4LResReplacer.exe");

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            WorkingDirectory = baseDir,
            Arguments = "--clean-only",
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        Process.Start(psi);
    }
    catch (Exception ex)
    {
        //Nothing
    }
}
```

# Libraries
- [S4Zip](https://github.com/wtfblub/NetspherePirates/blob/dev/src/Netsphere.Resource/S4Zip.cs)
- [BlubLib](https://gitlab.com/wtfblub/BlubLib/-/tree/dev/src/BlubLib)
