using System.Text;

using Bogus;

namespace AI.NetProject
{
	public class GenerateData
	{
		public void Generate()
		{
			string[] ageSertifications = 
			{
				"G", "PG", "PG-13", "R", "NC-17", "U", "U/A", "A", 
				"S", "AL", "6", "9", "12", "12A", "15", "18", 
				"18R", "R18", "R21", "M", "MA15+", "R16", "R18+", 
				"X18", "T", "E", "E10+", "EC", "C", "CA", "GP", 
				"M/PG", "TV-Y", "TV-Y7", "TV-G", "TV-PG", "TV-14", "TV-MA"
			};

			string[] roles =
			{
				"Director", 
				"Producer", 
				"Screenwriter", 
				"Actor", 
				"Actress", 
				"Cinematographer", 
				"Film Editor", 
				"Production Designer", 
				"Costume Designer", 
				"Music Composer"
			};

			int titleIds = 1;
			var testTitles = new Faker<Title>()
			.RuleFor(t => t.TitleId, f => titleIds++)
			.RuleFor(t => t.TitleName, f => f.Random.String2(5, 50))
			.RuleFor(t => t.Description, f => f.Random.Utf16String(200, 600))
				.RuleFor(t => t.ReleaseYear, f => f.Random.Number(1800, (int)DateTime.Now.Year))
				.RuleFor(t => t.AgeSertification, f => f.PickRandom(ageSertifications))
				.RuleFor(t => t.Runtime, f => f.Random.Number(1, 500))
				.RuleFor(t => t.Genres, f => Enumerable.Range(1, f.Random.Number(1, 10)).Select(x => f.Random.String2(5, 15)).ToList())
				.RuleFor(t => t.ProductionCountry, f => f.Random.String2(3).ToUpper())
				.RuleFor(t => t.Seasons, f => f.Random.Number(1, 10));

			int creditIds = 1;
			var testCredits = new Faker<Credit>()
				.RuleFor(c => c.CreditId, f => creditIds++)
				.RuleFor(c => c.TitleId, f => f.Random.Number(1, titleIds))
				.RuleFor(c => c.RealName, f => f.Random.String2(3, 13) + " " + f.Random.String2(3, 13))
				.RuleFor(c => c.CharacterName, f => f.Random.String2(3, 13))
				.RuleFor(c => c.Role, f => f.PickRandom(roles));

			var titles = testTitles.Generate(5);
			var credits = testCredits.Generate(15);

			WriteInFile(titles, credits);
		}

		public void WriteInFile(List<Title> titles, List<Credit> credits)
		{
			string titlesFilePath = @"C:\Users\lhavl\source\repos\AI.Net\TitlesOutput.csv";
			string creditsFilePath = @"C:\Users\lhavl\source\repos\AI.Net\CreditsOutput.csv";
			string separator = ",";
			StringBuilder titlesOutput = new StringBuilder();
			StringBuilder creditsOutput = new StringBuilder();

			string[] titleHeadings = { "Title Id", "Title Name", "Description", "Release Year", "Age Sertification", "Runtime", "Genres", "Production Country", "Seasons" };
			string[] creditHeadings = { "Credit Id", "Title Id", "Real Name", "Character Name", "Role" };
			titlesOutput.AppendLine(string.Join(separator, titleHeadings));
			creditsOutput.AppendLine(string.Join(separator, creditHeadings));

			foreach (Title title in titles)
			{
				string[] newLine =
				{
					title.TitleId.ToString(),
					title.TitleName,
					title.Description,
					title.ReleaseYear.ToString(),
					title.AgeSertification,
					title.Runtime.ToString(),
					string.Join(" ", title.Genres),
					title.ProductionCountry,
					title.Seasons.ToString()
				};

				titlesOutput.AppendLine(string.Join(separator, newLine));
			}

			foreach (Credit credit in credits)
			{
				string[] newLine =
				{
					credit.CreditId.ToString(),
					credit.TitleId.ToString(),
					credit.RealName,
					credit.CharacterName,
					credit.Role
				};

				creditsOutput.AppendLine(string.Join(separator, newLine));
			}

			try
			{
				File.AppendAllText(titlesFilePath, titlesOutput.ToString());
				File.AppendAllText(creditsFilePath, creditsOutput.ToString());
			}
			catch (Exception ex)
			{
				Console.WriteLine("Data could not be written to the CSV file.");
				Console.WriteLine(ex);
				return;
			}

			Console.WriteLine("The data has been successfully saved to the CSV file");
		}
	}
}
