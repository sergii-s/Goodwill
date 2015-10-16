(function () {
    'use strict';
    
    angular
        .module('goodwill')
        .factory('gameService', gameService);

    gameService.$inject = ['$http'];

    function gameService($http) {
        var service = {
            startGame: startGame
        };

        return service;

        function startGame(players) {
            return "/xxxx/";
        }
    }
})();