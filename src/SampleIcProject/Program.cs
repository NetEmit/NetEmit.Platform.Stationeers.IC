using NetEmit.Platform.Stationeers.IC;

namespace SampleIcProject
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var testIc = new IC10(@"Y:\SteamLibrary\steamapps\common\Stationeers");
			testIc.Title = "TestScript1";
			testIc.Author = "Stelzi";
			testIc.Description = "This is a testing script!!!";
			testIc.Emit();
		}
	}
}
