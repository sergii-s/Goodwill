(function () {
    'use strict';

    angular
        .module('goodwill')
        .controller('startGame',
            function ($scope,$linq) {
                var players = [{ Type: 'Humain', Name: '', State: 'Connected', Host: true }];
                var computersNames = ["Julien", "Jeremie", "Mohamed", "Alexandre"];
                $scope.players = players;
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
                };
                $scope.addComputer = function () {
                    players.push({
                        Type: 'Computer',
                        Name: computersNames.pop(),
                        State: 'Connected'
                    });
                };
                $scope.startGame = function () {
                    var connectedPlayers = $linq.Enumerable.From(players)
                        .Where(function(x) {
                            return x.State == 'Connected';
                        }).ToArray();


                };
            });

})();
