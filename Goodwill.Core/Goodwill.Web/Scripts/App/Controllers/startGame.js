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
                console.log('Token generated ', token);
            })
            .error(function () {
                console.log('Token generation failed');
            });

        $interval(refresh, 3000);
        $scope.players = players;
        $scope.readyPlayers = 1;

        function refreshPlayersInfo() {
            var players = gameService.getPlayers();
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

        function refresh() {
            gameService.getGameInfo(playerToken, gameStateId)
                .success(function (gameInfos) {
                    gameInfos.forEach(function (gameInfo, i, arr) {
                        gameStateId = gameInfo.gameStateId;
                        console.log('Latest game state id ', gameStateId);    
                    });
                })
                .error(function () {
                    console.log('Get game info failed');
                });
        }
    }

})();
