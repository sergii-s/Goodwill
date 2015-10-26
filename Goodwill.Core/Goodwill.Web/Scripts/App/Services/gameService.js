(function () {
    'use strict';
    
    angular
        .module('goodwill')
        .factory('gameService', gameServiceFactory);

    gameServiceFactory.$inject = ['$http'];

    function gameServiceFactory($http) {
        var gameService = {};

        gameService.startGame = function(token) {
            return "/xxxx/";
        }

        gameService.addComputer = function (token) {
            return "/xxxx/";
        }

        gameService.invitePlayer = function (token) {
            return "/xxxx/";
        }

        gameService.initializeGame = function () {
            return $http.get("/api/game/initialize");
        }

        gameService.getGameInfo = function (token, gameStateId) {
            return $http.get("/api/game/initialize");
        }

        return gameService;
    }
})();