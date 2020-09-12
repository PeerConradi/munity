import { AbstractAmendment } from "./abstract-amendment.model";

export class DeleteAmendment extends AbstractAmendment {
    /**
     *
     */
    constructor() {
        super();
        this.type = 'DeleteAmendment';
    }
}
