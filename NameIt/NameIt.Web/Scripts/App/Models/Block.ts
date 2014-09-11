module NameIt {
    'use strict';
    export class Block {
        constructor(
            public id: number,
            public name: string,
            public visual: Visual,
            public taxonomy: Taxonomy
            ){}
    }
} 