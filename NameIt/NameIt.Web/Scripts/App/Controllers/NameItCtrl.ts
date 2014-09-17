/// <reference path="../_all.ts" />
module NameIt {
    'use strict';

    export class NameItCtrl extends BaseCtrl {

        public static $inject = [
            '$scope', '$http'

        ];

        constructor(
            public $scope: INameItScope,
            public  $http: ng.IHttpService) {
            super();
            $scope.vm = this;
            $scope.taxonomies = new Array<Taxonomy>();
            this.getTaxonomies();
        }

        getTaxonomies() {
            if (this.$scope.taxonomies != undefined && this.$scope.taxonomies.length > 0) return;
            this.$http.get<Array<Taxonomy>>(this.rel2Abs('api/taxonomies')).success(data => {
                this.$scope.taxonomies = data;
            });
 
        }

    }

} 