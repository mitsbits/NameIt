module NameIt {
    'use strict';
    export interface IGameResource extends ng.resource.IResourceClass<IGame> {
        get():IGame
    }
}