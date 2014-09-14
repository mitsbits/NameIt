module NameIt {
    'use strict';
    export class Part {
        Score: number;
        Order: number;
        Total: number;
        Completed: boolean;

        constructor(
            public Block: Block,
            public AlternateNames: Array<string>) {
            this.Score = 3;
            this.Order = 0;
            this.Total = 0;
            this.Completed = false;
        }
    }


} 