using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace GameTrakR.Code
{
	public class GameScenario
	{
		private StringBuilder _sbLastPlay = new StringBuilder();

		#region Properties
		public int Inning { get; set; }
		public bool IsTopHalfOfInning { get; set; }
		public int Outs { get; set; }
		public int Balls { get; set; }
		public int Strikes { get; set; }
		public bool RunnerOnFirst { get; set; }
		public bool RunnerOnSecond { get; set; }
		public bool RunnerOnThird { get; set; }
		public int HomeScore { get; set; }
		public int AwayScore { get; set; }
		public string LastPlay { get; set; }

		public string RunnersOnBase
		{
			get
			{
				string _runnersString = String.Empty;
				List<string> runnersList = new List<string>();
				if (this.RunnerOnFirst)
					runnersList.Add("First");
				if (this.RunnerOnSecond)
					runnersList.Add("Second");
				if (this.RunnerOnThird)
					runnersList.Add("Third");

				_runnersString = String.Join(", ", runnersList.ToArray());

				return !String.IsNullOrEmpty(_runnersString) ? _runnersString : "None";
			}
		}

		public string CountOnBatter
		{
			get
			{
				if (this.Balls > 3 || this.Strikes > 2)
				{
					this.Balls = 0;
					this.Strikes = 0;
				}
				return String.Format("{0} Balls, {1} Strikes", this.Balls, this.Strikes);
			}
		}

		public string InningLabel
		{
			get
			{
				string _topBottom = this.IsTopHalfOfInning ? "Top" : "Bottom";
				string _inningSuffix = String.Empty;
				if (this.Inning % 10 == 1 && this.Inning != 11)
					_inningSuffix = "st";
				else if (this.Inning % 10 == 2 && this.Inning != 12)
					_inningSuffix = "nd";
				else if (this.Inning % 10 == 3 && this.Inning != 13)
					_inningSuffix = "rd";
				else
					_inningSuffix = "th";

				return String.Format("{0} of {1}{2} Inning", _topBottom, this.Inning, _inningSuffix);
			}
		}
		#endregion

		#region Constructors
		public GameScenario(int inning, bool isTopHalf, int outs, int balls, int strikes,
							bool onFirst, bool onSecond, bool onThird, int homeScore,
							int awayScore, string lastPlay)
		{
			this.Inning = inning;
			this.IsTopHalfOfInning = isTopHalf;
			this.Outs = outs;
			this.Balls = balls;
			this.Strikes = strikes;
			this.RunnerOnFirst = onFirst;
			this.RunnerOnSecond = onSecond;
			this.RunnerOnThird = onThird;
			this.HomeScore = homeScore;
			this.AwayScore = awayScore;
			this.LastPlay = lastPlay;
		}
		#endregion

		#region Public Methods
		public void AddStrike()
		{
			_sbLastPlay = new StringBuilder();

			if (this.Strikes + 1 > 2)
			{
				this.Strikes = 0;
				_sbLastPlay.Append("Batter struck out.");
				AddOut();
			}
			else
			{
				this.Strikes++;
				_sbLastPlay.AppendFormat("Strike {0} on batter.", this.Strikes);
			}

			UpdateLastPlay();
		}

		public void AddBall()
		{
			_sbLastPlay = new StringBuilder();

			if (this.Balls + 1 > 3)
			{
				this.Balls = 0;
				_sbLastPlay.Append("Batter walked.");
				AdvanceRunners(1, true, false);
			}
			else
			{
				this.Balls++;
				_sbLastPlay.AppendFormat("Ball {0} on batter.", this.Balls);
			}

			UpdateLastPlay();
		}

		public void BatterBallInPlay(int numOfBases, bool hitterOut)
		{
			_sbLastPlay = new StringBuilder();

			bool _canRunnersAdvance = true;
			_sbLastPlay.Append("Batter put ball in play.");
			if (hitterOut)
			{
				_sbLastPlay.Append(" Batter is out.");
				_canRunnersAdvance = AddOut();
			}

			if (_canRunnersAdvance)
				AdvanceRunners(numOfBases, true, hitterOut);

			UpdateLastPlay();
			ResetBatterCount();
		}

		public void StolenBases(int numOfBases)
		{
			_sbLastPlay = new StringBuilder();

			_sbLastPlay.AppendFormat("Runners stole {0} bases.", numOfBases);
			AdvanceRunners(numOfBases, false, false);

			UpdateLastPlay();
		}
		#endregion

		#region Private Methods
		private bool AddOut()
		{
			bool _canRunnersAdvance = false;
			if (this.Outs + 1 > 2)
			{
				this.Outs = 0;
				EndOfHalfInning();
				_sbLastPlay.AppendFormat(" Third Out Recorded.  Next up: {0}.", this.InningLabel);
			}
			else
			{
				this.Outs++;
				_sbLastPlay.Append(" Out Recorded.");
				_canRunnersAdvance = true;
			}

			ResetBatterCount();

			return _canRunnersAdvance;
		}

		private void AdvanceRunners(int numOfBases, bool hitterToRunner, bool hitterOut)
		{
			List<int> _basesOccupied = new List<int>();
			if (hitterToRunner && !hitterOut)
				_basesOccupied.Add(0); //Hitter

			if (this.RunnerOnFirst)
			{
				_basesOccupied.Add(1);
				this.RunnerOnFirst = false;
			}

			if (this.RunnerOnSecond)
			{
				_basesOccupied.Add(2);
				this.RunnerOnSecond = false;
			}

			if (this.RunnerOnThird)
			{
				_basesOccupied.Add(3);
				this.RunnerOnThird = false;
			}

			if (_basesOccupied.Count(r => r != 0) > 0)
				_sbLastPlay.AppendFormat(" {0} advance {1} bases.", hitterToRunner && !hitterOut ? "Batter and runners" : "Runners", numOfBases);
			else if (_basesOccupied.Count(r => r != 0) == 0 && !hitterOut)
				_sbLastPlay.AppendFormat(" Batter advances {0} bases.", numOfBases);


			int _runsScored = 0;
			foreach (int bo in _basesOccupied)
			{
				int _advanceBase = bo + numOfBases;
				switch (_advanceBase)
				{
					case 1:
						this.RunnerOnFirst = true;
						break;
					case 2:
						this.RunnerOnSecond = true;
						break;
					case 3:
						this.RunnerOnThird = true;
						break;
					default:
						if (this.IsTopHalfOfInning)
							this.AwayScore++;
						else
							this.HomeScore++;

						_runsScored++;
						break;
				}
			}

			if (_runsScored > 0)
				_sbLastPlay.AppendFormat(" {0} run(s) scored.", _runsScored);
		}

		private void EndOfHalfInning()
		{
			if (this.IsTopHalfOfInning)
				this.IsTopHalfOfInning = false;
			else
			{
				this.IsTopHalfOfInning = true;
				this.Inning++;
			}

			ResetBatterCount();
			EmptyBases();
		}

		private void ResetBatterCount()
		{
			this.Balls = 0;
			this.Strikes = 0;
		}

		private void EmptyBases()
		{
			this.RunnerOnFirst = false;
			this.RunnerOnSecond = false;
			this.RunnerOnThird = false;
		}

		private void UpdateLastPlay()
		{
			this.LastPlay = _sbLastPlay.ToString();
		}
		#endregion
	}
}