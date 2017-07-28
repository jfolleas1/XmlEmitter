using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace XmlForPluginEmitter
{
    public class DocXLauncher
    {
        public static void CreatSignatureDocxAndLaunchIt(string fileName)
        {
            string startPath = Constants.sourceDocxDirectory;
            string zipPath = Constants.tagetDocxDirectory + @"\" + fileName;

            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(startPath, zipPath);
            try
            {
                Process.Start(zipPath);
            }
            catch (Exception e)
            {

            }
        }
    }
}
