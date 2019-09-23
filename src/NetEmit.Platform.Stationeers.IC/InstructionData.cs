using System;
using System.ComponentModel;
using System.Xml.Serialization;

using NetEmit.Emiters;

namespace NetEmit.Platform.Stationeers.IC
{
	[Serializable]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true)]
	[XmlRoot(Namespace = "", IsNullable = false)]
	public class InstructionData : EmitableBase<StringEmiter>
	{
		public InstructionData(ulong dateTime = 0,
						 string gameVersion = "",
						 string title = "",
						 string description = "",
						 string author = "",
						 byte workshopFileHandle = 0,
						 IEmitable<StringEmiter> instructions = null!)
		{
			DateTime = dateTime;
			GameVersion = gameVersion;
			Title = title;
			Description = description;
			Author = author;
			WorkshopFileHandle = workshopFileHandle;
			Instructions = instructions;
		}

		public ulong DateTime { get; set; }

		public string GameVersion { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Author { get; set; }

		public byte WorkshopFileHandle { get; set; }

		public IEmitable<StringEmiter> Instructions { get; set; }

		public override StringEmiter Emit()
		{
			Emiter.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
			Emiter.AppendLine("<InstructionData xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
			Emiter.AppendLine($"<{nameof(DateTime)}>{DateTime}</{nameof(DateTime)}>");
			Emiter.AppendLine($"<{nameof(GameVersion)}>{GameVersion}</{nameof(GameVersion)}>");
			Emiter.AppendLine($"<{nameof(Title)}>{Title}</{nameof(Title)}>");
			Emiter.AppendLine($"<{nameof(Description)}>{Description}</{nameof(Description)}>");
			Emiter.AppendLine($"<{nameof(Author)}>{Author}</{nameof(Author)}>");
			Emiter.AppendLine($"<{nameof(WorkshopFileHandle)}>{WorkshopFileHandle}</{nameof(WorkshopFileHandle)}>");
			Emiter.AppendLine($"<{nameof(Instructions)}>{Instructions.Emit().Result().ToString()}</{nameof(Instructions)}>");
			Emiter.AppendLine("</InstructionData>");
			return base.Emit();
		}
	}
}
