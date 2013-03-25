/// <reference path="~/Scripts/jquery-1.9.1.js" />
/// <reference path="~/Scripts/jquery-ui-1.10.1.js" />

$(function () {
	$("#gamesContainer").sortable({
		placeholder: "ui-state-highlight",
		forcePlaceholderSize: true
	});
});

function DisplayMessage(msg) {
	var $msg = $("#msgContainer");
	var $div = $("<div />");

	$div.html(msg);
	$msg.append($div);
	$div.effect("highlight", {}, 5000, function () { $(this).hide().remove() });
}

function CreateNewGame(game) {
	var $gameLi = $("#gamesContainer").children().first().clone();
	$gameLi.attr("gameID", game.GameID);
	$gameLi.find("button.editGame").attr("onclick", "CreateScenarioDialog(" + game.GameID + ")");
	$("#gamesContainer").append($gameLi);
	UpdateGame(game);
}

function UpdateGame(game) {
	var $gameLi = $("li[gameID='" + game.GameID + "']", $("#gamesContainer"));
	$gameLi.find(".gameTeams").text(game.GameTeams);
	$gameLi.find(".scoreSummary").text(game.ScoreSummary);
	$gameLi.find(".inningLabel").text(game.CurrentGameScenario.InningLabel);
	$gameLi.find(".outs").text(game.CurrentGameScenario.Outs);
	$gameLi.find(".countOnBatter").text(game.CurrentGameScenario.CountOnBatter);
	$gameLi.find(".runnersOnBase").text(game.CurrentGameScenario.RunnersOnBase);
	$gameLi.find(".lastPlay").text(game.CurrentGameScenario.LastPlay);

	$gameLi.effect("highlight", {}, 5000);
}

function CreateNewGameDialog() {
	var $div = $("<div />").attr("id", "newGame").attr("display", "hidden");
	var $homeTeam = $("<div />").html("<span>Home Team: </span><input id='homeTeam' type='text' />");
	var $awayTeam = $("<div />").html("<span>Away Team: </span><input id='awayTeam' type='text' />");

	$div.append($homeTeam);
	$div.append($awayTeam);

	$div.dialog({
		title: "Create a New Game",
		modal: true,
		buttons: {
			"Create Game": function () {
				var home = $(this).find("#homeTeam").val();
				var away = $(this).find("#awayTeam").val();

				AddNewGame(home, away);
				$(this).dialog("close");
			}
		},
		close: function () {
			$div.remove();
		}
	});
}

function CreateScenarioDialog(gameID) {
	var $div = $("<div />").attr("id", "newScenario").attr("display", "hidden").html("<div>Choose an action to update the game scenario.</div>");

	$div.dialog({
		title: "Edit the Game Scenario",
		modal: true,
		buttons: {
			"Ball": function () {
				AddBall(gameID);
			},
			"Strike": function () {
				AddStrike(gameID);
			},
			"Hit Single": function () {
				BallInPlay(gameID, 1, false);
			},
			"Hit Double": function () {
				BallInPlay(gameID, 2, false);
			},
			"Hit Triple": function () {
				BallInPlay(gameID, 3, false);
			},
			"Hit Homerun": function () {
				BallInPlay(gameID, 4, false);
			},
			"Ground Out": function () {
				BallInPlay(gameID, 1, true);
			},
			"Fly Out": function () {
				BallInPlay(gameID, 0, true);
			}
		},
		close: function () {
			$div.remove();
		}
	});
}

function ResetGameList() {
	$("#gamesContainer").children().not("[gameID='empty']").remove();
}