module NameIt {
    'use strict';

    export class NameItCtrl {

        public static $inject = [
            '$scope', '$http'

        ];

        constructor(
            private $scope: IGameItScope,
            private $http: ng.IHttpService
 
            ) {
            $scope.vm = this;
            $scope.taxonomies = new Array<Taxonomy>();
            this.getTaxonomies();
        }

        getTaxonomies() {
            if (this.$scope.taxonomies != undefined && this.$scope.taxonomies.length > 0) return;
            this.$http.get<Array<Taxonomy>>('api/taxonomies').success(data => {
                this.$scope.taxonomies = data;
            });
 
        }

    }
} 