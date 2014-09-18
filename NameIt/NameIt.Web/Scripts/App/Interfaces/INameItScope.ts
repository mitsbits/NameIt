/// <reference path="../_all.ts" />

module NameIt {
    export interface INameItScope extends ng.IScope {
        vm: NameItCtrl;
        
    }
    export interface IGameItScope extends ng.IScope {
        vm: GameItCtrl;
        
    }
}