/// <reference path='../_all.ts' />

module NameIt {
    export interface INameItService {
        getTaxonomies(): ng.IPromise<Array<Taxonomy>>;
        getGame(taxonomy: number): ng.IPromise<Game>;
    }
}