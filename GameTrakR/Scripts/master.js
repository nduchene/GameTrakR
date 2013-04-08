/// <reference path="jquery-1.9.1.js" />
/// <reference path="jquery-ui-1.10.1.js" />
/// <reference path="jquery.jgrowl.js" />

$(function () {
	$("#ulMenu").menu();

	$("#gamesContainer").sortable({
		placeholder: "ui-state-highlight",
		forcePlaceholderSize: true
	});
});

function DisplayMessage(msg) {
	$.jGrowl(msg);
}

function CreateNewGame(game) {
	var $gameLi = $("#gamesContainer").children("[gameID='empty']").clone();
	$gameLi.attr("gameID", game.GameID);
	$gameLi.find("button.editGame").attr("onclick", "CreateScenarioDialog(" + game.GameID + ")");
	$gameLi.find("input.manageSubscription").attr("onchange", "ManageSubscription(" + game.GameID + ")");
	$("#gamesContainer").prepend($gameLi);
	UpdateGame(game);
}

function UpdateGame(game) {
	var $gameLi = $("li[gameID='" + game.GameID + "']", $("#gamesContainer"));

	$gameLi.fadeOut("slow", function () {
		$(this).find(".awayTeam").text(game.AwayTeam);
		$(this).find(".homeTeam").text(game.HomeTeam);
		$(this).find(".awayScore").text(game.CurrentGameScenario.AwayScore);
		$(this).find(".homeScore").text(game.CurrentGameScenario.HomeScore);
		$(this).find(".inningLabel").text(game.CurrentGameScenario.InningLabel);
		$(this).find(".outs").text(game.CurrentGameScenario.OutsLabel);
		$(this).find(".countOnBatter").text(game.CurrentGameScenario.CountOnBatter);
		$(this).find(".runnersOnBase").text(game.CurrentGameScenario.RunnersOnBase);
		$(this).find(".lastPlay").text(game.CurrentGameScenario.LastPlay);
		$(this).fadeIn("slow");
	});
}

function ManageSubscription(gameID) {
	var $cbSubscription = $("li[gameID='" + gameID + "']").find(".manageSubscription");

	if ($cbSubscription.is(":checked"))
		SubscribeToGame(gameID);
	else
		UnsubscribeToGame(gameID);
}

function ResetGameList() {
	$("#gamesContainer").children().not("[gameID='empty']").remove();
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