using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Elders.Skynet.Core.Packages
{
    public class FileDictionary
    {
        private static object locker = new object();

        private string fileName;

        public FileDictionary(string fileName)
        {
            this.fileName = fileName;
        }

        public Dictionary<string, string> Load()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var line in File.ReadAllLines(fileName))
            {
                var entry = line.Split(new string[] { "<|>" }, StringSplitOptions.None);
                dictionary.Add(entry[0], entry[1]);
            }
            return dictionary;
        }

        public void Add(string key, string value)
        {
            lock (locker)
            {
                var dict = Load();
                dict.Add(key, value);
                File.WriteAllLines(fileName, dict.Select(x => x.Key + "<|>" + x.Value));
            }
        }

        public void Remove(string key)
        {
            lock (locker)
            {
                var dict = Load();
                dict.Remove(key);
                File.WriteAllLines(fileName, dict.Select(x => x.Key + "<|>" + x.Value));
            }
        }
    }
}