module NameIt {
    'use strict';

    export class NameItService implements INameItService {

        public static $inject = [
            '$cacheFactory',
            '$resource'
        ];

        constructor(
            private $cacheFactory: ng.ICacheFactoryService,
            private $resource: ng.resource.IResourceService) { }
            
            getTaxonomies() {
                var taxonomies = this.$resource('api/taxonomies');
                taxonomies.query((d, s) => {
                    alert(s + " " + d);
                });
            }
       
    }

} 