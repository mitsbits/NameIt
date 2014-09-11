/// <reference path="_all.ts" />
module NameIt {
    'use strict';
    var nameIt = angular.module('nameIt', [])
        .controller('nameItCtrl', NameItCtrl)
        .factory('GameResource',
        ['$resource', ($resource: ng.resource.IResourceService): IGameResource => {
            return <IGameResource> $resource('/api/games/:id', { id: '@id' });
        }]);
} 