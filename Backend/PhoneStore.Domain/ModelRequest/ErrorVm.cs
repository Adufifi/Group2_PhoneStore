namespace PhoneStore.Domain.ViewModel
{
    public class ErrorVm
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = default!;
        public string Path { get; set; } = default!;
    }
}
