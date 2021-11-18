using Microsoft.Data.Sqlite;

public static class MovieService
{
    static SqliteConnection connection;

    static MovieService()
    {
        connection = new SqliteConnection("Data Source=movies.db");
        connection.Open();
    }

    public static List<Movie> GetMovies()
    {
        List<Movie> movies = new List<Movie>();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
        @"
            SELECT
                title
            FROM
                movies
        ";

        var reader = command.ExecuteReader();
        while(reader.Read())
        {
            Movie movie = new Movie();
            movie.Title = reader.GetString(0);
            

            movies.Add(movie);
        }
    }

    public static List<Line> GetMovieLines(string movieTitle)
    {
        List<Line> lines = new List<Line>();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
        @"
            SELECT
                line,
                character,
                scene
            FROM
                lines
            WHERE
                movie = @movie
        ";
        command.Parameters.AddWithValue("@movie", movieTitle);

        var reader = command.ExecuteReader();
        while(reader.Read())
        {
            Line line = new Line();
            line.Qoutes = reader.GetString(0).Split(". ").Select(s => s + '.').ToArray();
            line.Character = reader.GetString(1);
            line.Scene = reader.GetString(2);

            lines.Add(line);
        }

        return lines;
    }

    public static List<string> GetMovieCharacters(string movieTitle) 
    {
        List<string> characters = new List<string>();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
        @"
            SELECT DISTINCT
                character
            FROM
                lines
            WHERE
                movie = @movie
        ";
        command.Parameters.AddWithValue("@movie", movieTitle);

        var reader = command.ExecuteReader();
        while(reader.Read())
        {
            characters.Add(reader.GetString(0));
        }

        return characters;
    }
}