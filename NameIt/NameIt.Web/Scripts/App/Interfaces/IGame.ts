module NameIt {
    'use strict';
    export interface IGame  {
        Parts: Array<Part>;
        Score: number;
        Selected: number;
        SelectedPart:Part;
    }
}
