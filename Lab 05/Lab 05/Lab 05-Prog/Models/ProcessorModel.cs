namespace Lab_05_Prog.Models
{
    public class ProcessorModel
    {
        public IFormFile Data { get; set; }

        public string ErrorValue { get; set; }
        public string Response { get; set; }
        public bool Calculated { get; set; }
    }
}
