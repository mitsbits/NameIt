module NameIt {
    'use strict';
    export class Part {
        constructor(
            public block: Block,
            public alternatenames: Array<string>,
            public score: number,
            public order: number,
            public total: number,
            public completed: boolean) { }
    }


} 