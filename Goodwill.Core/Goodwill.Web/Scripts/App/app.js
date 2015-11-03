(function () {
    'use strict';

    var app = angular.module('goodwill', [

        // Angular modules 

        // Custom modules 
        'angular-linq'
        // 3rd Party Modules
        
    ]);

    app.directive('highlighter', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            scope: {
                model: '=highlighter'
            },
            link: function (scope, element) {
                scope.$watch('model', function (nv, ov) {
                    if (nv !== ov) {
                        // apply class
                        element.addClass('highlight');

                        // auto remove after some delay
                        $timeout(function () {
                            element.removeClass('highlight');
                        }, 1000);
                    }
                });
            }
        };
    }]);
})();