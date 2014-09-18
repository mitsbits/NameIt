/// <reference path="../_all.ts" />
module NameIt {
    'use strict';

    export class NameItService implements INameItService {

        private root: string = "http://localhost:53458/";


        public static $inject = [
            '$q','$http'
        ];

        constructor(
            private $q:ng.IQService,
            private $http: ng.IHttpService) { }

        getTaxonomies(): ng.IPromise<Array<Taxonomy>> {
            var deferred = this.$q.defer <Array<Taxonomy>>();
            this.$http.get<Array<Taxonomy>>(this.rel2Abs('api/taxonomies')).success(data => {
                deferred.resolve(data);
            });
            return deferred.promise;
        }

        getGame(taxonomy: number): ng.IPromise<Game>{
            var deferred = this.$q.defer<Game>();
            this.$http.get<Game>(this.rel2Abs('api/games/') + taxonomy).success(data => {
                deferred.resolve(this.prepareGame(data));
            });
            return deferred.promise;
        }


        private rel2Abs(path: string): string {
            var result: string = this.root + path;
            return result;
        }
        private prepareGame(data: Game): Game {
            var total = data.Parts.length;
            for (var i = 0; i < total; i++) {
                var part = data.Parts[i];
                part.Order = i;
                part.Completed = false;
                part.Score = 3;
                part.Total = total;
                data.Parts[i] = part;
            }
            data.Score = 0;
            return data;
        }
    }
} 