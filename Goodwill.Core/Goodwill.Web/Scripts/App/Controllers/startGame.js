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

        $scope.players = players;
        $scope.readyPlayers = 1;

        function refreshPlayersCount() {
            var connectedPlayers = $linq.Enumerable().From(players)
                .Where(function (x) {
                    return x.State == 'Connected';
                }).Count();
            $scope.readyPlayers = connectedPlayers;
        }

        $scope.addPlayer = function () {
            players.push({
                Type: 'Humain',
                Name: '',
                Email: '',
                State: 'Waiting'
            });
        };
        $scope.addComputer = function () {
            players.push({
                Type: 'Computer',
                Name: computersNames.pop(),
                State: 'Connected'
            });
            refreshPlayersCount();
        };
        $scope.startGame = function () {
            var connectedPlayers = $linq.Enumerable.From(players)
                .Where(function(x) {
                    return x.State == 'Connected';
                }).ToArray();
            var gameid = gameService.StartGame(connectedPlayers);
        };
    }

})();
