module NameIt {
    'use strict';

    export class NameItService implements INameItService {

        public static $inject = [
            '$cacheFactory'
        ];

        constructor(private $cacheFactory : ng.ICacheFactoryService){}
    }

} 