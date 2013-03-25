using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GameTrakR.Code;

namespace GameTrakR.Hubs
{
	public class GameHub : Hub
	{
		#region Groups
		public void Subscribe(string gameID)
		{
			Groups.Add(Context.ConnectionId, gameID);
			Game game = Game.GetForID(Convert.ToInt32(gameID));

			Clients.Caller.groupGameUpdate(game);
		}

		public void Unsubscribe(string gameID)
		{
			Groups.Remove(Context.ConnectionId, gameID);
		}
		#endregion		

		public void AddNewGame(string homeTeam, string awayTeam)
		{
			Game game = new Game(homeTeam, awayTeam);
			game.AddGame();
			Clients.All.allNewGame(game);
		}

		public void AddBall(string gameID)
		{
			Game game = Game.GetForID(Convert.ToInt32(gameID));
			GameScenario gs = game.CurrentGameScenario;
			gs.AddBall();

			Clients.All.allGameUpdate(game);
			Clients.Group(gameID).groupGameUpdate(game);
		}

		public void AddStrike(string gameID)
		{
			Game game = Game.GetForID(Convert.ToInt32(gameID));
			GameScenario gs = game.CurrentGameScenario;
			gs.AddStrike();

			Clients.All.allGameUpdate(game);
			Clients.Group(gameID).groupGameUpdate(game);
		}

		public void BatterBallInPlay(string gameID, int numOfBases, bool hitterOut)
		{
			Game game = Game.GetForID(Convert.ToInt32(gameID));
			GameScenario gs = game.CurrentGameScenario;
			gs.BatterBallInPlay(numOfBases, hitterOut);

			Clients.All.allGameUpdate(game);
			Clients.Group(gameID).groupGameUpdate(game);
		}

		public void ResetGamesList()
		{
			Game.ResetGamesInstance();
			Clients.All.resetGameList();
			Clients.All.resetSubscriptions();
		}
	}
}