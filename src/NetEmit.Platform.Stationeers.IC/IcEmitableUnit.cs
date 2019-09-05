using System.IO;
using System.Linq;

namespace NetEmit.Platform.Stationeers.IC
{
    public abstract class IcEmitableUnit : IEmitableUnit
    {
        private string[]? _SettingXml;

        protected string SaveFolder { get; }

        protected string ScriptFolder { get; }

        protected string GameVersion { get; }

        public IcEmitableUnit(string settingPath)
        {
            SaveFolder = ResolveSaveFolder(settingPath);
            GameVersion = ResolveGameVersion(settingPath);
            ScriptFolder = Path.Combine(SaveFolder, "scripts");
        }

        private string ResolveGameVersion(string settingPath)
        {
            var versionIni = Directory.GetFiles(settingPath, "version.ini", SearchOption.AllDirectories).FirstOrDefault();
            var versionLine = File.ReadAllLines(versionIni).Where(l => l.Contains("UPDATEVERSION")).FirstOrDefault();

            return versionLine.Replace("UPDATEVERSION=Update ", string.Empty).Trim();
        }

        private string GetXmlTag(string[] inputs, string tagName)
            => inputs.Where(l => l.Contains($"<{tagName}>")).FirstOrDefault()
                .Replace($"<{tagName}>", string.Empty)
                .Replace($"</{tagName}>", string.Empty)
                .Trim();


        private string ResolveSaveFolder(string settingPath)
        {
            var settingXml = Directory.GetFiles(settingPath, "setting.xml", SearchOption.AllDirectories).FirstOrDefault();
            _SettingXml = File.ReadAllLines(settingXml);

            return GetXmlTag(_SettingXml, "SavePath");
        }
    }
}
