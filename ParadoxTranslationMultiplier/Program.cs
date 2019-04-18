using System;
using System.IO;
using System.Text;

namespace ParadoxTranslationMultiplier
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = GetDefaultLanguages();
            }

            var dir = new DirectoryInfo(@".");
            foreach (var file in dir.GetFiles("*_l_*.yml", SearchOption.AllDirectories))
            {
                // parse original language
                var orgLanguage = file.Name.Substring(file.Name.IndexOf("_l_", StringComparison.Ordinal) + 3).Replace(".yml", String.Empty);
                foreach (var targetLanguage in args)
                {
                    // skip if the target equals the original language
                    if (targetLanguage.Equals(orgLanguage, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    var targetFileName = file.FullName.Replace(orgLanguage, targetLanguage);
                    // skip copy process if the target file already exists
                    if (File.Exists(targetFileName))
                    {
                        continue;
                    }

                    // modify language definition in file
                    var originalContent = File.ReadAllText(file.FullName);

                    File.WriteAllText(targetFileName, originalContent.Replace($"l_{orgLanguage}", $"l_{targetLanguage}"), Encoding.UTF8);
                }
            }
        }

        private static string[] GetDefaultLanguages()
        {
            // taken form https://stellaris.paradoxwikis.com/Localisation_modding
            return new[] { "braz_por", "english", "french", "german", "polish", "russian", "spanish" };
        }
    }
}