namespace CustomerLibCore.Api.Dtos.Notes
{
    public class NoteResponse : IResponse, INoteDetails
    {
        public string Self { get; set; }
        public string Content { get; set; }
    }
}
