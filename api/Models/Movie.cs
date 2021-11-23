public class Movie 
{
    public string Title { get; set; } = "";
    public string[] Characters { get; set; } = new string[0];
    public string[] Scenes { get; set; } = new string[0];
    public Line[] Lines { get; set; } = new Line[0];
}