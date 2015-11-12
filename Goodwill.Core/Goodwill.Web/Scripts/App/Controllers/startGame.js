(function () {
    'use strict';

    angular
        .module('goodwill')
        .controller('startGame', startGameController);

    startGameController.$inject = [
        '$scope', '$linq', '$interval', 'gameService'
    ];

    function startGameController($scope, $linq, $interval, gameService) {
        var players = [
            {
                PlayerPublicId: 0,
                Type: 'Humain',
                Name: '',
                State: 'Connected',
                Host: true
            }
        ];
        var companies = [];
        var opponents = [];
        var currentPlayer = {};
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
        $scope.opponents = opponents;
        $scope.companies = companies;
        $scope.currentPlayer = currentPlayer;
        $scope.readyPlayers = 1;
        $scope.gameStarted = false;

        $scope.addPlayer = function (playerEmail) {
            gameService.invitePlayer(playerToken, playerEmail);
        };

        $scope.addComputer = function () {
            gameService.addComputer(playerToken);
        };

        $scope.startGame = function () {
            gameService.startGame(playerToken);
        };

        function applicateGameInfo(gameInfo) {
            $scope.gameStarted = gameInfo.Started;
            players[0].Name = gameInfo.Name;

            gameInfo.Players.forEach(function (player) {
                var playersEnum = $linq.Enumerable().From(players);
                var existingPlayer = playersEnum.FirstOrDefault(null, 'x => x.PlayerPublicId == ' + player.PlayerPublicId);
                if (existingPlayer == null) {
                    players.push({
                        PlayerPublicId: player.PlayerPublicId,
                        Type: player.Humain ? 'Humain' : 'Computer',
                        Name: player.Name == null ? player.Email : player.Name,
                        State: player.Connected ? 'Connected' : 'Waiting',
                        Host: player.Host
                    });
                } else {
                    existingPlayer.Type = player.Humain ? 'Humain' : 'Computer';
                    existingPlayer.Name = player.Name == null ? player.Email : player.Name;
                    existingPlayer.State = player.Connected ? 'Connected' : 'Waiting';
                    existingPlayer.Host = player.Host;
                }
            });

            var connectedPlayers = $linq.Enumerable().From(players)
                .Where(function (x) {
                    return x.State == 'Connected';
                }).Count();
            $scope.readyPlayers = connectedPlayers;

            if (gameInfo.Started) {
                //Players
                gameInfo.Players.forEach(function (player) {
                    var opponentsEnum = $linq.Enumerable().From(opponents);
                    var existingOpponent = opponentsEnum.FirstOrDefault(null, 'x => x.PlayerPublicId == ' + player.PlayerPublicId);
                    if (existingOpponent == null) {
                        opponents.push({
                            PlayerPublicId: player.PlayerPublicId,
                            Type: player.Humain ? 'Humain' : 'Computer',
                            Name: player.Name == null ? player.Email : player.Name,
                            State: player.Connected ? 'Connected' : 'Waiting',
                            Host: player.Host,
                            Money: player.GameInfo.Money,
                            Actions: transformActions(player.GameInfo.Actions)
                        });
                    } else {
                        existingOpponent.Type = player.Humain ? 'Humain' : 'Computer';
                        existingOpponent.Name = player.Name == null ? player.Email : player.Name;
                        existingOpponent.State = player.Connected ? 'Connected' : 'Waiting';
                        existingOpponent.Host = player.Host;
                        existingOpponent.Money = player.GameInfo.Money;
                        existingOpponent.Actions = transformActions(player.GameInfo.Actions);
                    }
                });
                //Personal
                currentPlayer.Name = gameInfo.GameInfo.Name;
                currentPlayer.Money = gameInfo.GameInfo.Money;
                currentPlayer.Actions = transformActions(gameInfo.GameInfo.Actions);
                currentPlayer.Events = gameInfo.GameInfo.Events;
                //Global
                //Companies
                gameInfo.Companies.forEach(function (company) {
                    var companiesEnum = $linq.Enumerable().From(companies);
                    var existingCompany = companiesEnum.FirstOrDefault(null, 'x => x.Name == "' + company.Name + '"');
                    if (existingCompany == null) {
                        companies.push({
                            Name: company.Name,
                            Money: company.Money,
                            MarketShare: company.MarketShare,
                            Manager: company.Manager,
                            Ressources: company.RessourceDependencies
                        });
                    } else {
                        existingCompany.Name = company.Name;
                        existingCompany.Money = company.Money;
                        existingCompany.MarketShare = company.MarketShare;
                        existingCompany.Manager = company.Manager;
                        existingCompany.Ressources = company.RessourceDependencies;
                    }
                });
            }
        }

        function transformActions(actions) {
            return $linq.Enumerable().From(actions)
                            .GroupBy("$.Company", null,
                                     function (key, g) {
                                         return {
                                             Company: key,
                                             Count: g.Count()
                                         }
                                     })
                            .ToArray();
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
