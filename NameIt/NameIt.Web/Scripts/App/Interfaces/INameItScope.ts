/// <reference path="../_all.ts" />

module NameIt {
    export interface INameItScope extends ng.IScope {
        vm: NameItCtrl;
        taxonomies : Array<Taxonomy>;
    }
    export interface IGameItScope extends ng.IScope {
        vm: GameItCtrl;
        game:IGame
    }
}