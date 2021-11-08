namespace MUNity.Schema.Organization
{
    public class CreateOrganizationResponse
    {
        public enum CreateOrgaStatusCodes
        {
            Success,
            NameTaken,
            ShortTaken,
            Error
        }

        public CreateOrgaStatusCodes Status { get; set; }

        public string OrganizationId { get; set; }
    }
}
