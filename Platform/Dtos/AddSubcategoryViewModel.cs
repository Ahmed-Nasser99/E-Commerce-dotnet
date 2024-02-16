using Platform.Model;

namespace Platform.Dtos
{
    public class AddSubcategoryViewModel
    {
        public string name { get; set; }
        public IFormFile image { get; set; }

        public Guid categoryid { get; set; }
    }
}
