/// <reference path="../_all.ts" />
module NameIt {
    'use strict';
    export class Game implements IGame {
        public SelectedPart: Part;
        constructor(
            public Parts: Array<Part>,
            public Score?: number,
            public Selected?: number) {
            if (Selected == null) {
                this.Selected = -1;
                this.SelectedPart = null;
            }
            if (Score == null) {
                this.Score = 0;
            }
        }

    }
   
} 