module NameIt {
    'use strict';

    export class NameItCtrl {

        public static $inject = [
            '$scope',
            '$location',
            'service',
            'games'
        ];

        constructor(
            private $scope: ITodoScope,
            private $location: ng.ILocationService,
            private service: INameItService,
            private games: IGameResource
            ) {

        }
    }
} 