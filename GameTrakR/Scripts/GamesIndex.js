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

	$gameLi.effect("highlight", {}, 750);
}

function ResetGameList() {
	$("#gamesContainer").children().not("[gameID='empty']").remove();
}