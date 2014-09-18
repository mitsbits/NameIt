/// <reference path="../_all.ts" />
module NameIt {
    'use strict';

    export class NameItCtrl extends BaseCtrl {

        public taxonomies: Array<Taxonomy> = new Array<Taxonomy>();

        public static $inject = [
            '$scope', '$http', 'nameItService'

        ];

        constructor(
            private $scope: INameItScope,
            private $http: ng.IHttpService,
            private nameItService: INameItService) {
            
            super();
            $scope.vm = this;
            this.getTaxonomies();
        }

        private getTaxonomies() {
            if (this.taxonomies != undefined && this.taxonomies.length > 0) return;
            this.nameItService.getTaxonomies().then(data => {
                this.taxonomies = data;
            });
        }

    }

} 