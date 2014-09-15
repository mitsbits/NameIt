/// <reference path="../_all.ts" />
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
            var taxonomy = <number>this.$routeParams["game"];
            this.showGame(taxonomy);
        }

        showGame(taxonomy: number):void {
            this.$http.get<Game>('api/games/' + taxonomy).success(data => {
                this.$scope.game = data;
                var total = this.$scope.game.Parts.length;
                for (var i = 0; i < total; i++) {
                    var part = this.$scope.game.Parts[i];
                    part.Order = i;
                    part.Completed = false;
                    part.Score = 3;
                    part.Total = total;
                    this.$scope.game.Parts[i] = part;                    
                }
                this.$scope.game.Score = 0;
                this.showPart(0);
            });           
        }

        showPart(indx: number):void {
            this.$scope.game.Selected = indx;
            var gg = this.$scope.game.Parts[indx];
            this.$scope.game.SelectedPart = gg;        
        }

        vote(option: string):void {
            var part = this.$scope.game.SelectedPart;
            var correctValue = part.Block.Name;
            var iscorrect = option == correctValue;
            if (iscorrect) {
                this.handleSuccess(part);
            } else {
                this.handleFail(part);
            }
        }

        private handleSuccess(part: Part):void {
            this.$scope.game.Score += part.Score;
            part.Completed = true;
            this.$scope.game.Parts[part.Order] = part;
            this.showPart(part.Order + 1);
        }
        private handleFail(part: Part): void {
            part.AlternateNames.splice(part.Order, 1);
        }
    }
} 