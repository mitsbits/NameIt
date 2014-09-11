/// <reference path="../_all.ts" />
module NameIt {
    'use strict';
    export class Game implements IGame {
        constructor(
            public parts: Array<Part>,
            public score: number){}
    }
} 