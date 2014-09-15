module NameIt {
    'use strict';
    export class Part implements IPart {
        Score: number;
        Order: number;
        Total: number;
        Completed: boolean;

        public NameBucket: Array<string> = new Array<string>();

        constructor(
            public Block: Block,
            public AlternateNames: Array<string>) {
            this.Score = 3;
            this.Order = 0;
            this.Total = 0;
            this.Completed = false;



        }

        PrepareNameOptions(): void {
            var bucket = new Array<string>();
            bucket.push(this.Block.Name,
                this.AlternateNames[0],
                this.AlternateNames[1]);
            bucket.sort();
            this.NameBucket = bucket;
            alert(this.NameBucket.length);
        }

    }
} 