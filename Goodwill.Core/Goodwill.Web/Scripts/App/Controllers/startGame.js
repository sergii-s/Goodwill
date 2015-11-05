(function () {
    'use strict';

    angular
        .module('goodwill')
        .controller('startGame', startGameController);

    startGameController.$inject = [
        '$scope', '$linq', '$interval', 'gameService'
    ];

    function startGameController($scope, $linq, $interval, gameService) {
        var players = [{ Type: 'Humain', Name: '', State: 'Connected', Host: true }];
        var playerToken = '';
        var gameStateId = '';

        gameService.initializeGame()
            .success(function (token) {
                playerToken = token;
                $interval(refresh, 3000);
                console.log('Token generated ', token);
            })
            .error(function () {
                console.log('Token generation failed');
            });


        $scope.players = players;
        $scope.readyPlayers = 1;
        $scope.gameStarted = false;

        function refreshPlayersInfo() {
            //var players = gameService.getPlayers();
            var connectedPlayers = $linq.Enumerable().From(players)
                .Where(function (x) {
                    return x.State == 'Connected';
                }).Count();
            $scope.readyPlayers = connectedPlayers;
        }

        $scope.addPlayer = function (playerEmail) {
            gameService.invitePlayer(playerToken, playerEmail);
            refreshPlayersInfo();
        };

        $scope.addComputer = function () {
            gameService.addComputer(playerToken);
            refreshPlayersInfo();
        };

        $scope.startGame = function () {
            gameService.startGame(playerToken);
        };

        function applicateGameInfo(gameInfo) {
            $scope.gameStarted = gameInfo.Started;
            players[0].Name = gameInfo.Name;
            gameInfo.Players.forEach(function (player) {
                if ($linq.Enumerable().From(players).Any(function(x) { x.PlayerPublicId == player.PlayerPublicId })) {
                    players.push({
                        Type: player.Humain ? 'Humain' : 'Computer',
                        Name: player.Name == null ? player.Email : player.Name,
                        State: player.Connected ? 'Connected' : 'Waiting',
                        Host: player.Host
                    });
                } else {
                    players[player.PlayerPublicId].Type = player.Humain ? 'Humain' : 'Computer';
                    players[player.PlayerPublicId].Name = player.Name == null ? player.Email : player.Name;
                    players[player.PlayerPublicId].State = player.Connected ? 'Connected' : 'Waiting';
                    players[player.PlayerPublicId].Host = player.Host;
                }
            });
        }

        function refresh() {
            gameService.getGameInfo(playerToken, gameStateId)
                .success(function (gameInfos) {
                    gameInfos.forEach(function (gameInfo) {
                        gameStateId = gameInfo.GameStateId;
                        applicateGameInfo(gameInfo);
                        console.log('Latest game state id ', gameStateId);
                    });
                })
                .error(function () {
                    console.log('Get game info failed');
                });
        }
    }

})();
