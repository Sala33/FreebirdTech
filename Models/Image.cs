namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Image Object <see cref="Image"/> which can be used for creating and acessing 
    /// the Image in other part of the software
    /// </summary> 
    public class Image
    {
        public int Id { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
    }
}
