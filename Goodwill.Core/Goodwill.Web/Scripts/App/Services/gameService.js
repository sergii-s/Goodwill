(function () {
    'use strict';
    
    angular
        .module('goodwill')
        .factory('gameService', gameServiceFactory);

    gameServiceFactory.$inject = ['$http'];

    function gameServiceFactory($http) {
        var gameService = {};

        gameService.startGame = function(token) {
            return $http.get("/api/game/start?token=" + token);
        }

        gameService.addComputer = function (token) {
            return $http.get("/api/game/addComputer?token=" + token);
        }

        gameService.invitePlayer = function (token, email) {
            return $http.get("/api/game/invitePlayer?token="+token+"&email="+email);
        }

        gameService.initializeGame = function () {
            return $http.get("/api/game/initialize");
        }

        gameService.getGameInfo = function (token, gameStateId) {
            return $http.get("/api/game/info?token="+token+"&state="+gameStateId);
        }

        return gameService;
    }
})();