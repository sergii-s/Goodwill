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
        $scope.companies = companies;
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

            if (gameInfo.Companies != null) {
                gameInfo.Companies.forEach(function (company) {
                    var companiesEnum = $linq.Enumerable().From(companies);
                    var existingCompany = companiesEnum.FirstOrDefault(null, 'x => x.Name == "' + company.Name + '"');
                    if (existingCompany == null) {
                        companies.push({
                            Name: company.Name
                        });
                    } else {
                        existingCompany.Name = company.Name;
                    }
                });
            }
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
