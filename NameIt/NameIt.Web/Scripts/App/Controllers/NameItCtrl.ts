module NameIt {
    'use strict';

    export class NameItCtrl {

        public static $inject = [
            '$scope'//,
            , '$http'
            //'$resource'
            //'nameItService'
        ];

        constructor(
            private $scope: IGameItScope//,
            , private $http: ng.IHttpService
        //private $resource: ng.resource.IResourceService
        //private nameItService : NameItService
            ) {
            $scope.vm = this;

        }

        getTaxonomies() {
            this.$scope.taxonomies = new Array<Taxonomy>();
            this.$http({
                method: 'GET',
                url: 'api/taxonomies'
            }).success(data => {
                for (var i = data.length - 1; i >= 0; i--) {
                    var _disp: string = data[i].Display;
                    var _id: number = data[i].Id;
                    var _pid: number = 0;
                    if (data[i].ParentId) {
                        _pid = data[i].ParentId;
                    }
                    this.$scope.taxonomies.push(new Taxonomy(_id, _disp, _pid));
                }


            });

  

        }

    }
} 