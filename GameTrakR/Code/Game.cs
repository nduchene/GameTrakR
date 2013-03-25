using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameTrakR.Code
{
	public class Game
	{
		#region Properties
		public int? GameID { get; set; }
		public string HomeTeam { get; set; }
		public string AwayTeam { get; set; }
		public GameScenario CurrentGameScenario { get; set; }

		public string GameTeams
		{
			get
			{
				return String.Format("{0} at {1}", this.AwayTeam, this.HomeTeam);
			}
		}

		public string ScoreSummary
		{
			get
			{
				return String.Format("{0} {1}, {2} {3}", this.AwayTeam, this.CurrentGameScenario.AwayScore, this.HomeTeam, this.CurrentGameScenario.HomeScore);
			}
		}
		#endregion

		#region Constructors
		public Game(string homeTeam, string AwayTeam)
		{
			this.HomeTeam = homeTeam;
			this.AwayTeam = AwayTeam;
			this.CurrentGameScenario = new GameScenario(1, true, 0, 0, 0, false, false, false, 0, 0, "Start of Game.");
		}
		#endregion

		#region Public Methods
		public void AddGame()
		{
			this.GameID = gamesInstance.Value.Count() + 1;
			gamesInstance.Value.Add(this);
		}
		#endregion

		#region Games Instance Object & Methods
		private static Lazy<List<Game>> gamesInstance = new Lazy<List<Game>>();

		public static List<Game> GamesInstance
		{
			get { return gamesInstance.Value; }
		}

		public static Game GetForID(int id)
		{
			return gamesInstance.Value.Where(g => g.GameID == id).FirstOrDefault();
		}

		public static void ResetGamesInstance()
		{
			gamesInstance = new Lazy<List<Game>>();
		}
		#endregion
	}

	public class GamesViewModel
	{
		public List<Game> Games
		{
			get
			{
				return Game.GamesInstance;
			}
		}
	}
}