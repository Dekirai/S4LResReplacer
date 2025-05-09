using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using NeoNetsphere.Resource;

namespace S4LResReplacer
{
    class Program
    {
        private const string EnableCustomArg = "--enable-custom";
        private const string CleanOnlyArg = "--clean-only";

        private static readonly string RootDir = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string ResourceFile = Path.Combine(RootDir, "resource.s4hd");
        private static readonly string BackupFile = Path.Combine(RootDir, "resource.s4hd.bak");
        private static readonly string CustomDir = Path.Combine(RootDir, "Custom");
        private static readonly string CacheDir = Path.Combine(RootDir, "_resources");

        private static S4Zip _zipFile;

        static void Main(string[] args)
        {
            if (args.Any(a => a.Equals(EnableCustomArg, StringComparison.OrdinalIgnoreCase)))
                ApplyCustomizations();

            if (args.Any(a => a.Equals(CleanOnlyArg, StringComparison.OrdinalIgnoreCase)))
            {
                Thread.Sleep(10000); //Make sure S4Client is open
                WaitForClientExit();
                RevertResourceFile();
                CleanupUnusedResources();
            }
        }

        private static void ApplyCustomizations()
        {
            if (!Directory.Exists(CustomDir))
                return;

            if (!File.Exists(ResourceFile))
                return;

            if (!File.Exists(BackupFile))
                File.Copy(ResourceFile, BackupFile);

            _zipFile = S4Zip.OpenZip(ResourceFile);
            if (_zipFile == null)
                return;

            var entryMap = _zipFile.Values
                                   .ToDictionary(e => e.FullName,
                                                 StringComparer.OrdinalIgnoreCase);

            var files = Directory.EnumerateFiles(CustomDir, "*.*", SearchOption.AllDirectories)
                                 .ToArray();

            int replaced = 0, added = 0, skipped = 0;

            foreach (var file in files)
            {
                var rel = file.Substring(CustomDir.Length)
                              .TrimStart('\\', '/')
                              .Replace('\\', '/');

                long newLength = new FileInfo(file).Length;
                byte[] newData = File.ReadAllBytes(file);

                if (entryMap.TryGetValue(rel, out var existing))
                {
                    if (existing.Length == newLength)
                    {
                        byte[] oldData = existing.GetData();
                        if (oldData.Length == newLength && oldData.SequenceEqual(newData))
                        {
                            skipped++;
                            continue;
                        }
                    }
                    existing.SetData(newData);
                    replaced++;
                }
                else
                {
                    _zipFile.CreateEntry(rel, newData);
                    added++;
                }
            }

            _zipFile.Save();
        }

        private static void RevertResourceFile()
        {
            if (File.Exists(BackupFile))
            {
                if (File.Exists(ResourceFile))
                    File.Delete(ResourceFile);
                File.Move(BackupFile, ResourceFile);

            }
        }

        private static void CleanupUnusedResources()
        {
            var _zipFile2 = S4Zip.OpenZip(ResourceFile);
            string resourceDir = _zipFile2.ResourcePath;
            if (!Directory.Exists(CacheDir))
                return;

            var usedChecksums = new HashSet<string>(
                            _zipFile2.Values.Select(ent => ent.Checksum.ToString("x")),
                            StringComparer.OrdinalIgnoreCase);


            var onDiskFiles = Directory.GetFiles(resourceDir);
            var unused = onDiskFiles
                .Where(path => !usedChecksums.Contains(Path.GetFileName(path)))
                .ToList();

            int count = unused.Count;
            if (count == 0)
            {
                Console.WriteLine("No unused resources found.");
            }
            else
            {
                foreach (var file in unused)
                    File.Delete(file);
            }
        }

        private static void WaitForClientExit()
        {
            Process[] procs;
            while ((procs = Process.GetProcessesByName("S4Client")).Length == 0)
            {
                Thread.Sleep(2000);
            }

            foreach (var p in procs)
            {
                try
                {
                    p.WaitForExit();
                }
                catch { /* might have exited already */ }
            }
        }
    }
}
