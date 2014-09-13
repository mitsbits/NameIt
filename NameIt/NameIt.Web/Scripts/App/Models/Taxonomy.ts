module NameIt {
    'use strict';
    export class Taxonomy {
        constructor(
            public Id: number,
            public Display: string,
            public ParentId? : number){}
    }
} 