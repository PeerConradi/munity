namespace MUNity.Schema.Project
{
    public class CreateProjectResponse
    {
        public enum CreateProjectStatus
        {
            Success,
            Error,
            NameTaken,
            ShortTaken,
            OrganizationNotFound,
            NoRights
        }

        public CreateProjectStatus Status { get; set; }

        public string ProjectId { get; set; }
    }
}
