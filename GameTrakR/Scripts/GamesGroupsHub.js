/// <reference path="../../Scripts/jquery-1.9.1.js" />
/// <reference path="../../Scripts/jquery-ui-1.10.1.js" />

var gameHub = $.connection.gameHub;

$(function () {
	gameHub.client.groupGameUpdate = function (game) {
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

function SubscribeToGame(gameID) {
	gameHub.server.subscribe(gameID);
}

function UnsubscribeToGame(gameID) {
	gameHub.server.unsubscribe(gameID);
}