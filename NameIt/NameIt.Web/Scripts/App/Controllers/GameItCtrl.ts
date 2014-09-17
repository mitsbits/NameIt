/// <reference path="../_all.ts" />
module NameIt {
    'use strict';

    export class GameItCtrl extends BaseCtrl {
        public static $inject = [
            '$scope', '$http', '$routeParams', '$location'
        ];

        private nextVoteAdvances: boolean = false;

        constructor(
            public $scope: IGameItScope,
            public $http: ng.IHttpService,
            public $routeParams: ng.route.IRouteParamsService,
            private $location: ng.ILocationService) {
            super();
            $scope.vm = this;
            var taxonomy = <number>this.$routeParams["game"];
            this.showGame(taxonomy);
        }

        showGame(taxonomy: number): void {
            this.$http.get<Game>(this.rel2Abs('api/games/') + taxonomy).success(data => {
                this.$scope.game = this.prepareGame(data);
                this.showPart(0);
            });
        }

        showPart(indx: number): void {
            //if (indx == this.$scope.game.Parts.length) {
                
            //    this.$location.path( indx.toString() + "/results");

            //} else {
                this.$scope.game.Selected = indx;
                var gg = this.$scope.game.Parts[indx];
                this.$scope.game.SelectedPart = gg;
                this.nextVoteAdvances = false;
            //}
        }

        vote(option: string): void {
            var part = this.$scope.game.SelectedPart;
            if (this.nextVoteAdvances) {
                this.showPart(part.Order + 1);
            }
            var correctValue = part.Block.Name;
            if (option == correctValue) {
                this.handleSuccess(part);
            } else {
                this.handleFail(part, option);
            }
        }


        private handleSuccess(part: Part): void {
            this.$scope.game.Score += part.Score;
            part.Completed = true;
            this.$scope.game.Parts[part.Order] = part;
            if (part.Score > 0) {
                //alert("You have won " + part.Score + " points!\nMoving on...");
            }
            this.showPart(part.Order + 1);
        }
        private handleFail(part: Part, option: string): void {
            if (part.Score == 1) {
                part.Score = 0;
                this.nextVoteAdvances = true;
                //alert("Click on the correct answer to advance.\n:(");
            } else {
                if (part.Score == 3) {
                    part.Score = 1;
                }
            }
            var indx = part.AlternateNames.indexOf(option);
            part.AlternateNames.splice(indx, 1);
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