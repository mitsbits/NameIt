module NameIt {
    'use strict';

    export class NameItCtrl {

        public static $inject = [
            '$scope', '$http'

        ];

        constructor(
            public $scope: INameItScope,
            public  $http: ng.IHttpService
 
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

    export class GameItCtrl extends NameItCtrl
    {
        public static $inject = [
            '$scope', '$http','$routeParams'

        ];

        constructor(
            public $scope: IGameItScope,
            public $http: ng.IHttpService,
            public $routeParams: ng.route.IRouteParamsService

            ) {
            super($scope, $http);
            var game = <number>this.$routeParams["game"];
            this.showGame(game);
        }

        showGame(game: number):void {
            this.$http.get<Game>('api/games/' + game).success(data => {
                this.$scope.game = data;
                for (var i = 0; i < this.$scope.game.Parts.length; i++) {
                    var part = this.$scope.game.Parts[i];
                    part.Order = i;
                    part.Completed = false;
                    part.Score = 3;
                    this.$scope.game.Parts[i] = part;
                    
                }
                this.showPart(0);
            });           
        }

        showPart(indx: number):void {
            this.$scope.game.Selected = indx;
            var gg = this.$scope.game.Parts[indx];
            gg.PrepareNameOptions();
            this.$scope.game.SelectedPart = gg;
            
        }
    }
} 