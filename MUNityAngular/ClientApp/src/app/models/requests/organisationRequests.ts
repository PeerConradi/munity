export class CreateOrganisationRequest {
    public organisationName: string;

    public abbreviation: string;

    /**
     *
     */
    constructor(name: string, abbreviation: string) {
        this.organisationName = name;
        this.abbreviation = abbreviation;
    }
}