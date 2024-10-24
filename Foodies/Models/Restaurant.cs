namespace Foodies.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string ImgPath { get; set; }// Update from ImgName to ImgPath
    }

}
