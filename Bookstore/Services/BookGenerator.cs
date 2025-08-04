using Bogus;
using Bookstore.Models;

public class BookGenerator
{
    public List<Book> GenerateBooks(int seed, int page, string locale, double avgLikes, double avgReviews, 
        int count = 10)
    {
        Randomizer.Seed = new Random(123);
        var faker = new Faker(locale);

        var bookFaker = new Faker<Book>(locale)
            .RuleFor(b => b.ISBN, f => f.Random.Replace("###-###-###"))
            .RuleFor(b => b.Title, f =>
            {
                var words = f.Random.WordsArray(f.Random.Int(2, 7));
                return string.Join(" ", words.Select(w => char.ToUpper(w[0]) + w.Substring(1)));
            })
            .RuleFor(b => b.Authors, f => f.Name.FullName())
            .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
            .RuleFor(b => b.CoverUrl, (f, b) =>
            $"https://picsum.photos/seed/{seed}/120/160");

        var reviewFaker = new Faker<Review>(locale)
            .RuleFor(r => r.Reviewer, f => f.Name.FullName())
            .RuleFor(r => r.Text, f => f.Hacker.Phrase());

        var books = new List<Book>();

        for (int i = 0; i < count; i++)
        {
            var book = bookFaker.Generate();
            book.Index = (page - 1) * count + i + 1;
            // cover photo
            book.CoverUrl = $"https://picsum.photos/seed/{seed + book.Index}/120/160";
            // Likes
            int likes = (int)Math.Floor(avgLikes);
            if (new Random().NextDouble() < avgLikes - likes)
                likes += 1;
            book.Likes = likes;

            // Reviews
            int revs = (int)Math.Floor(avgReviews);
            if (new Random().NextDouble() < avgReviews - revs)
                revs += 1;
            book.Reviews = reviewFaker.Generate(revs);

            books.Add(book);
        }

        return books;
    }
}
