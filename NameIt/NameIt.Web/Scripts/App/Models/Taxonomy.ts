module NameIt {
    'use strict';
    export class Taxonomy {
        constructor(
            public id: number,
            public display: string,
            public parentid? : number){}
    }
} 