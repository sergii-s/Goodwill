(function () {
    'use strict';

    angular
        .module('goodwill')
        .controller('startGame', startGameController);

    startGameController.$inject = [
        '$scope', '$linq', 'gameService'
    ];

    function startGameController($scope,$linq, gameService) {
        var players = [{ Type: 'Humain', Name: '', State: 'Connected', Host: true }];
        var computersNames = ["Julien", "Jeremie", "Mohamed", "Alexandre"];
        var playerToken = gameService.initializeGame();

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
            gameService.InvitePlayer(playerToken, playerEmail);
            refreshPlayersInfo();
        };
        $scope.addComputer = function () {
            gameService.AddComputer(playerToken);
            refreshPlayersInfo();
        };
        $scope.startGame = function () {
            gameService.StartGame(playerToken);
        };
    }

})();
