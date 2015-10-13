(function () {
    'use strict';

    angular
        .module('goodwill')
        .controller('startGame',
            function ($scope) {
                var players = [
                    { Name: "Julien" },
                    { Name: "Jeremie" },
                    { Name: "Mohamed" },
                    { Name: "Alexandre" }
                ];
                $scope.players = players;
                $scope.addPlayer = function () {
                    players.push({ Name: "Player " + (players.length + 1) });
                };

            });

})();
