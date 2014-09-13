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
 
    }

    var nameItApp = new Module('nameIt', []);
    //nameItApp.addService('nameItService', NameIt.NameItService);
    nameItApp.addController('nameItCtrlr', NameIt.NameItCtrl);

} 