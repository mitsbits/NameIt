/// <reference path="_all.ts" />
module NameIt {
    'use strict';

    class Module {
        app: ng.IModule;

        constructor(name: string, modules: Array<string>) {
            this.app = angular.module(name, modules);
        }

        addController(name: string, controller: Function) {
            this.app.controller(name, controller);
        }
        addService(name: string, service: Function): void {
            this.app.service(name, service);
        }

        addConfig(configFn: any) {
            this.app.config(configFn);
        }
    }

    var nameItApp = new Module('nameIt', ['ngRoute']);
    nameItApp.addController('nameItCtrl', NameIt.NameItCtrl);
    nameItApp.addController('gameItCtrl', NameIt.GameItCtrl);
    nameItApp.addConfig(['$routeProvider',
        '$locationProvider',
        ($routeProvider: ng.route.IRouteProvider,
            $locationProvider: ng.ILocationProvider) => {
            $routeProvider.when('/', {
                controller: 'nameItCtrl',
                templateUrl: 'Scripts/App/Partials/NameIt/Index.html'
            });
            $routeProvider.when('/:game', {
                controller: 'gameItCtrl',
                templateUrl: 'Scripts/App/Partials/NameIt/Game.html'
            });
            $routeProvider.otherwise({
                redirectTo: '/'
            });
            $locationProvider.html5Mode(true);
        }]);

    //nameItApp.addService('nameItService', NameIt.NameItService);

} 