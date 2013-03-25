/// <reference path="~/Scripts/jquery-1.9.1.js" />
/// <reference path="~/Scripts/jquery-ui-1.10.1.js" />

var gameHub = $.connection.gameHub;

$(function () {
	gameHub.client.allGameUpdate = function (game) {
		UpdateGame(game);
	};

	gameHub.client.allNewGame = function (game) {
		CreateNewGame(game);
	};

	gameHub.client.resetGameList = function () {
		ResetGameList();
	};

	$.connection.hub.start()
			.done(function () {
				DisplayMessage("SUCCESS: Connected to GameHub!");
			})
			.fail(function () {
				DisplayMessage("ERROR: Cannot connect to GameHub.");
			});
});

function AddNewGame(homeTeam, awayTeam) {
	gameHub.server.addNewGame(homeTeam, awayTeam);
}

function Reset() {
	gameHub.server.resetGamesList();
}

function AddBall(gameID) {
	gameHub.server.addBall(gameID);
}

function AddStrike(gameID) {
	gameHub.server.addStrike(gameID);
}

function BallInPlay(gameID, numOfBases, hitterOut) {
	gameHub.server.batterBallInPlay(gameID, numOfBases, hitterOut);
}