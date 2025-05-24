# S4LResReplacer 🔄

> **S4LResReplacer** – a lightweight tool to read and write raw resource files in-place for S4 League.

---

## 🚀 Features

- **Write & Save** any changed or new file into `resource.s4hd`
- **Auto-Cleanup**: restores original resources after S4Client exits
- **CLI Flags**: `--enable-custom` to write, `--clean-only` to revert

---

## ▶️ Usage

1. Download `S4LResReplacer.zip` from [Releases](https://github.com/Dekirai/S4LResReplacer/releases)  
2. Extract into your S4 League root folder  
3. Create two .bat scripts (e.g., `res_write.bat` & `res_clean.bat`):
   - **Write**: `S4LResReplacer.exe --enable-custom`
   - **Clean**: `S4LResReplacer.exe --clean-only`
4. Create a `Custom` folder in the S4 League root  
5. Place your resource files in `Custom` mirroring their in-game paths  
6. Run `res_write.bat` to apply overrides  
7. When exiting S4, run `res_clean.bat` (or rely on `--clean-only` auto-check) to restore originals

> Tip: Bind these scripts to your launcher to auto-enable custom resources before launch and auto-clean on exit.

---

## 📹 Demo Video

[![ResReplacer Demo](https://img.youtube.com/vi/LKlhf-vbCb4/0.jpg)](https://www.youtube.com/watch?v=LKlhf-vbCb4&ab_channel=Dekirai)

---

## 💻 Launcher Integration Example
```csharp
private async void bt_StartGame_Click(object sender, RoutedEventArgs e)
{
    bt_StartGame.IsEnabled = false;
    if (chbk_CustomResources.IsChecked == true)
    {
        var psi = new ProcessStartInfo
        {
            FileName = Path.Combine(AppContext.BaseDirectory, "S4LResReplacer.exe"),
            Arguments = "--enable-custom",
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        using var proc = Process.Start(psi);
        if (proc != null) await proc.WaitForExitAsync();
    }
    await Task.Run(() => LoginClient.Connect(Constants.ConnectEndPoint));
    Properties.Settings.Default.Save();
    try
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(AppContext.BaseDirectory, "S4LResReplacer.exe"),
            Arguments = "--clean-only",
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        });
    }
    catch {}
}
````

---

## 📚 Libraries

* [S4Zip](https://github.com/wtfblub/NetspherePirates/blob/dev/src/Netsphere.Resource/S4Zip.cs)
* [BlubLib](https://gitlab.com/wtfblub/BlubLib/-/tree/dev/src/BlubLib)

---

## 📄 License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.
