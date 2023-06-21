using System.Data;
using System.Text;
using AI.NetProject.Enteties;
using Bogus;

namespace AI.NetProject
{
	/// <summary>
	/// Class for generating test data
	/// </summary>
	public class GenerateData
	{
		private int titleIds = 1;
		private int creditIds = 1;
		private bool isFirstLines = true;

		/// <summary>
		/// Generating correct data with normal length
		/// </summary>
		internal void GenerateNormalData()
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

			var testCredits = new Faker<Credit>()
				.RuleFor(c => c.CreditId, f => creditIds++)
				.RuleFor(c => c.TitleId, f => f.Random.Number(1, titleIds))
				.RuleFor(c => c.RealName, f => f.Random.String2(3, 13) + " " + f.Random.String2(3, 13))
				.RuleFor(c => c.CharacterName, f => f.Random.String2(3, 13))
				.RuleFor(c => c.Role, f => f.PickRandom(roles));

			var titles = testTitles.Generate(40);
			var credits = testCredits.Generate(70);

			WriteInFile(titles, credits);
		}

		/// <summary>
		/// Generating different data, include long strings and large numbers
		/// </summary>
		internal void GenerateExtremeData()
		{
			var testTitles = new Faker<Title>()
				.RuleFor(t => t.TitleId, f => titleIds++)
				.RuleFor(t => t.TitleName, f => f.Random.Utf16String(0, 1000))
				.RuleFor(t => t.Description, f => f.Random.Utf16String(0, 10000))
				.RuleFor(t => t.ReleaseYear, f => f.Random.Number((int)DateTime.MinValue.Year, (int)DateTime.MaxValue.Year))
				.RuleFor(t => t.AgeSertification, f => f.Random.Utf16String(0, 1000))
				.RuleFor(t => t.Runtime, f => f.Random.Int(0))
				.RuleFor(t => t.Genres, f => Enumerable.Range(1, f.Random.Number(0, 200)).Select(x => f.Random.Utf16String(0, 500)).ToList())
				.RuleFor(t => t.ProductionCountry, f => f.Random.Utf16String(3).ToUpper())
				.RuleFor(t => t.Seasons, f => f.Random.Int(0));

			var testCredits = new Faker<Credit>()
				.RuleFor(c => c.CreditId, f => creditIds++)
				.RuleFor(c => c.TitleId, f => f.Random.Number(1, titleIds))
				.RuleFor(c => c.RealName, f => f.Random.Utf16String(0, 1000) + " " + f.Random.Utf16String(0, 1000))
				.RuleFor(c => c.CharacterName, f => f.Random.Utf16String(0, 1000))
				.RuleFor(c => c.Role, f => f.Random.Utf16String(0, 1000));

			var titles = testTitles.Generate(40);
			var credits = testCredits.Generate(70);

			WriteInFile(titles, credits);
		}

		/// <summary>
		/// Generating invalid data, include dublicate PKs, foreign keys to nonexistent PKs, null values, etc.
		/// </summary>
		internal void GenerateInvalidData()
		{
			var testTitles = new Faker<Title>()
				.RuleFor(t => t.TitleId, f => titleIds++.OrDefault(f, .5f, f.PickRandom(titleIds)))
				.RuleFor(t => t.TitleName, f => f.Random.Utf16String(0, 50).OrNull(f, .5f))
				.RuleFor(t => t.Description, f => f.Random.Utf16String(0, 600).OrNull(f, .5f))
				.RuleFor(t => t.ReleaseYear, f => f.Random.Int(-100, 100))
				.RuleFor(t => t.AgeSertification, f => f.Random.Utf16String(0, 10).OrNull(f, .5f))
				.RuleFor(t => t.Runtime, f => f.Random.Int(-100, 100))
				.RuleFor(t => t.Genres, f => Enumerable.Range(1, f.Random.Int(0, 100)).Select(x => f.Random.Utf16String(0, 100).OrNull(f, .5f)).ToList())
				.RuleFor(t => t.ProductionCountry, f => f.Random.String2(3).OrNull(f, .5f))
				.RuleFor(t => t.Seasons, f => f.Random.Int(-100, 100));

			var testCredits = new Faker<Credit>()
				.RuleFor(c => c.CreditId, f => creditIds++.OrDefault(f, .5f, f.PickRandom(creditIds)))
				.RuleFor(c => c.TitleId, f => f.Random.Number(1, titleIds).OrDefault(f, .5f, titleIds + f.Random.Number(1, 50)))
				.RuleFor(c => c.RealName, f => f.Random.Utf16String(0, 100).OrNull(f, .5f))
				.RuleFor(c => c.CharacterName, f => f.Random.Utf16String(0, 100).OrNull(f, .5f))
				.RuleFor(c => c.Role, f => f.Random.Utf16String(0, 100).OrNull(f, .5f));

			var titles = testTitles.Generate(40);
			var credits = testCredits.Generate(70);

			WriteInFile(titles, credits);
		}

		/// <summary>
		/// Writing test data to files
		/// </summary>
		/// <param name="titles">Titles data</param>
		/// <param name="credits">Credits data</param>
		public void WriteInFile(List<Title> titles, List<Credit> credits)
		{
			string titlesFilePath = @"C:\Users\lhavl\source\repos\AI.Net\TitlesOutput.csv";
			string creditsFilePath = @"C:\Users\lhavl\source\repos\AI.Net\CreditsOutput.csv";
			string separator = ",";
			StringBuilder titlesOutput = new StringBuilder();
			StringBuilder creditsOutput = new StringBuilder();

			if(isFirstLines == true)
			{
				string[] titleHeadings = { "Title Id", "Title Name", "Description", "Release Year", "Age Sertification", "Runtime", "Genres", "Production Country", "Seasons" };
				string[] creditHeadings = { "Credit Id", "Title Id", "Real Name", "Character Name", "Role" };
				titlesOutput.AppendLine(string.Join(separator, titleHeadings));
				creditsOutput.AppendLine(string.Join(separator, creditHeadings));

				isFirstLines = false;
			}

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
				return;
			}

			Console.WriteLine("The data has been successfully saved to the CSV file");
		}
	}
}
