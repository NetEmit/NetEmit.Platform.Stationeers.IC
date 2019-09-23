using System;
using System.IO;
using System.Linq;
using System.Text;

using NetEmit.Emiters;

namespace NetEmit.Platform.Stationeers.IC
{
	public abstract class IcEmitableUnit : EmitableUnitBase<StringEmiter>
	{
		private string[]? _SettingXml;

		protected string SaveFolder { get; }

		protected string ScriptFolder { get; }

		protected string GameVersion { get; }

		public string Title { get; set; } = string.Empty;

		public string Author { get; set; } = string.Empty;

		public byte WorkshopFileHandle { get; set; } = 0;

		public string Description { get; set; } = string.Empty;

		public IEmitable<StringEmiter>? Instructions { get; protected set; } = default;

		public IcEmitableUnit(string settingPath,
						string title = "",
						string author = "",
						string description = "",
						byte workshopFileHandle = 0)
			: base(new InstructionData())
		{
			SaveFolder = ResolveSaveFolder(settingPath);
			GameVersion = ResolveGameVersion(settingPath);
			ScriptFolder = Path.Combine(SaveFolder, "scripts");
			Title = title;
			Author = author;
			Description = description;
			WorkshopFileHandle = workshopFileHandle;
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

			return GetXmlTag(_SettingXml, "SavePath").Replace('/', Path.DirectorySeparatorChar);
		}

		public virtual new bool Emit()
		{

			var filePath = Path.Combine(ScriptFolder, Title);
			var emitHandledFile = Path.Combine(filePath, "emit.handled");
			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}
			else
			{
				if (!File.Exists(emitHandledFile))
				{
					throw new ArgumentException($"Script with the title '{Title}' already exists and is not by this EmitFramework handled!");
				}
			}

			File.WriteAllText(emitHandledFile, DateTime.Now.ToLongDateString() + Environment.NewLine + DateTime.Now.ToLongTimeString());

			var xmlFilePath = Path.Combine(filePath, "instruction.xml");

			var instructionData = (InstructionData)EmitableUnit;
			instructionData.DateTime = (ulong)DateTime.Now.ToFileTime();
			instructionData.GameVersion = GameVersion;
			instructionData.Title = Title;
			instructionData.Description = Description;
			instructionData.Author = Author;
			instructionData.WorkshopFileHandle = WorkshopFileHandle;
			instructionData.Instructions = Instructions;

			var emitString = (string)instructionData.Emit().Result();
			File.WriteAllText(xmlFilePath, emitString, Encoding.UTF8);

			return true;
		}
	}
}
