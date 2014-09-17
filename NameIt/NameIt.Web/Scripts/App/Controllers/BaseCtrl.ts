/// <reference path="../_all.ts" />
module NameIt {
    'use strict';
    
    export class BaseCtrl {
        private root: string = "http://localhost:53458/";
        rel2Abs(path: string): string {
            var result: string = this.root + path;
            return result;
        }
    }
} 