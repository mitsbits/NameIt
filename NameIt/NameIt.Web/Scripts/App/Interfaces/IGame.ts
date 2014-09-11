module NameIt {
    'use strict';
    export interface IGame extends ng.resource.IResource<IGame> {
        parts: Array<Part>;
        score: number;
    }
}
