namespace Shared.ResultModels
{
    public class DeleteResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
