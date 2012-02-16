using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace Common
{
    [RunInstaller(true)]
    public class CustomInstaller : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            string oldPath = GetPath();
            stateSaver.Add("previousPath", oldPath);
            
            string newPath = AddPath(oldPath, MyPath());
            if (oldPath == newPath)
            {
                stateSaver.Add("changedPath", false);
                return;
            }
                
            stateSaver.Add("changedPath", true);
            SetPath(newPath);

            WindowsMessageHelper.BroadcastEnvironment();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            if ((bool)savedState["changedPath"])
                SetPath(RemovePath(GetPath(), MyPath()));

            WindowsMessageHelper.BroadcastEnvironment();
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);

            if ((bool)savedState["changedPath"])
                SetPath((string)savedState["previousPath"]);

            WindowsMessageHelper.BroadcastEnvironment();
        }

        private static string MyPath()
        {
            string myFile = Assembly.GetExecutingAssembly().Location;
            string myPath = Path.GetDirectoryName(myFile);
            return myPath;
        }

        private static RegistryKey GetPathRegKey(bool writable)
        {
            // for the user-specific path...
            return Registry.CurrentUser.OpenSubKey("Environment", writable);

            // for the system-wide path...
            //return Registry.LocalMachine.OpenSubKey(
            //    @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", writable);
        }

        private static void SetPath(string value)
        {
            using (var reg = GetPathRegKey(true))
            {
                reg.SetValue("Path", value, RegistryValueKind.ExpandString);
            }
        }

        private static string GetPath()
        {
            using (RegistryKey reg = GetPathRegKey(false))
            {
                return (string)reg.GetValue("Path", "", RegistryValueOptions.DoNotExpandEnvironmentNames);
            }
        }

        private static string AddPath(string list, string item)
        {
            var paths = new List<string>(list.Split(';'));

            foreach (string path in paths)
            {
                if (string.Compare(path, item, true) == 0)
                {
                    // already present
                    return list;
                }
            }

            paths.Add(item);
            return string.Join(";", paths.ToArray());
        }

        private static string RemovePath(string list, string item)
        {
            var paths = new List<string>(list.Split(';'));

            for (int i = 0; i < paths.Count; i++)
            {
                if (string.Compare(paths[i], item, true) == 0)
                {
                    paths.RemoveAt(i);
                    return string.Join(";", paths.ToArray());
                }
            }

            // not present
            return list;
        }


    }
}
