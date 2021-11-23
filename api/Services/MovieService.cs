using Microsoft.Data.Sqlite;

public static class MovieService
{
    static SqliteConnection connection;

    static MovieService()
    {
        connection = new SqliteConnection("Data Source=..\\data.db");
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
            Movie movie = GetMovie(reader.GetString(0));
            movies.Add(movie);
        }

        return movies;
    }
    
    public static List<string> GetMovieTitles() 
    {
        List<string> titles = new List<string>();

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
            titles.Add(reader.GetString(0));
        }

        return titles;
    }

    public static Movie GetMovie(string movieTitle) 
    {
        Movie movie = new Movie();
        movie.Title = movieTitle;
        movie.Characters = GetMovieCharacters(movie.Title).ToArray();
        movie.Scenes = GetMovieScenes(movie.Title).ToArray();
        movie.Lines = GetMovieLines(movie.Title).ToArray();

        return movie;
    }

    public static MovieMeta GetMovieMeta(string movieTitle) 
    {
        MovieMeta meta = new MovieMeta();
        meta.Title = movieTitle;
        meta.Characters = GetMovieCharacters(movieTitle).ToArray();
        meta.Scenes = GetMovieScenes(movieTitle).ToArray();
        
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
            SELECT
                COUNT(*)
            FROM
                lines
            WHERE
                movie = @movie
        ";
        command.Parameters.AddWithValue("@movie", movieTitle);

        var reader = command.ExecuteReader();
        meta.LineCount = reader.GetInt32(0);

        return meta;
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
            SELECT
                DISTINCT character
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


    public static List<string> GetMovieScenes(string movieTitle) 
    {
        List<string> scenes = new List<string>();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = 
        @"
            SELECT DISTINCT
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
            scenes.Add(reader.GetString(0));
        }

        return scenes;
    }

    public static List<Line> GetLines(string movieTitle, int amount)
    {
        
    }

    public static Quote GetQuote(string movieTitle)
    {
        Quote quote = new Quote();
        
        var lines = GetMovieLines(movieTitle);
        var random = new Random();
        Line line = lines[random.Next(lines.Count)];

        quote.Character = line.Character;
        quote.Scene = line.Scene;
        quote.Text = line.Qoutes[random.Next(line.Qoutes.Length)];

        return quote;
    }
}