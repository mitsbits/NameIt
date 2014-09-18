/// <reference path="../_all.ts" />
module NameIt {
    'use strict';

    export class GameItCtrl extends BaseCtrl {


        private nextVoteAdvances: boolean = false;

        public game : Game;
        public static $inject = [
            '$scope', '$routeParams', '$location', 'nameItService'
        ];
        constructor(
            public $scope: IGameItScope,
            public $routeParams: ng.route.IRouteParamsService,
            private $location: ng.ILocationService,
            private nameItService: INameItService ) {
            super();
            $scope.vm = this;
            var taxonomy = <number>this.$routeParams["game"];
            this.showGame(taxonomy);
        }

        showGame(taxonomy: number): void {
            this.nameItService.getGame(taxonomy).then(data => {
                this.game = data;
                this.showPart(0);
            });
        }

        showPart(indx: number): void {
                this.game.Selected = indx;
                var gg = this.game.Parts[indx];
                this.game.SelectedPart = gg;
                this.nextVoteAdvances = false;
        }

        vote(option: string): void {
            var part = this.game.SelectedPart;
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
            this.game.Score += part.Score;
            part.Completed = true;
            this.game.Parts[part.Order] = part;
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

    }
} 