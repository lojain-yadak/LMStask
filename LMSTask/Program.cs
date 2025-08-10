using System;
using System.Collections.Generic;
using System.Linq;

public abstract class AbstractPerson
{
    public int Id { get; set; }
    public string Name { get; set; }

    protected List<Book> BorrowedBooks { get; set; } = new List<Book>();

    public AbstractPerson(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public abstract void DisplayInfo();

    public virtual void BorrowBook(Book book)
    {
        if (book == null)
        {
            Console.WriteLine("Book is null.");
            return;
        }

        if (!book.IsAvailable)
        {
            Console.WriteLine($"Sorry, the book '{book.Title}' is not available.");
            return;
        }

        BorrowedBooks.Add(book);
        book.IsAvailable = false;
        Console.WriteLine($"{Name} has successfully borrowed '{book.Title}'.");
    }

    public virtual void ReturnBook(Book book)
    {
        if (book == null)
        {
            Console.WriteLine("Book is null.");
            return;
        }

        if (BorrowedBooks.Contains(book))
        {
            BorrowedBooks.Remove(book);
            book.IsAvailable = true;
            Console.WriteLine($"{Name} has returned '{book.Title}'.");
        }
        else
        {
            Console.WriteLine($"{Name} does not have the book '{book.Title}' borrowed.");
        }
    }

    public void DisplayBorrowedBooks()
    {
        Console.WriteLine($"\n--- Books borrowed by {Name} ---");
        if (BorrowedBooks.Any())
        {
            foreach (var book in BorrowedBooks)
            {
                book.DisplayInfo();
            }
        }
        else
        {
            Console.WriteLine("No books borrowed.");
        }
    }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public bool IsAvailable { get; set; } = true;

    public Book(int id, string title, string authorName)
    {
        Id = id;
        Title = title;
        AuthorName = authorName;
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"Book ID: {Id}, Title: {Title}, Author: {AuthorName}, Available: {(IsAvailable ? "Yes" : "No")}");
    }
}

public class Member : AbstractPerson
{
    public Member(int id, string name) : base(id, name)
    {
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"Member ID: {Id}, Name: {Name}");
    }
}

public class Library
{
    private List<Book> Books { get; set; } = new List<Book>();
    public List<AbstractPerson> Members { get; set; } = new List<AbstractPerson>();

    public void AddBook(Book book)
    {
        Books.Add(book);
        Console.WriteLine($"Book '{book.Title}' added to the library.");
    }

    public void AddMember(AbstractPerson member)
    {
        Members.Add(member);
        Console.WriteLine($"Member '{member.Name}' added to the library.");
    }

    public Book FindBookById(int id)
    {
        return Books.FirstOrDefault(b => b.Id == id);
    }

    public AbstractPerson FindMemberById(int id)
    {
        return Members.FirstOrDefault(m => m.Id == id);
    }

    public void DisplayAllBooks()
    {
        Console.WriteLine("\n--- All Books in Library ---");
        if (Books.Any())
        {
            foreach (var book in Books)
            {
                book.DisplayInfo();
            }
        }
        else
        {
            Console.WriteLine("No books in the library.");
        }
    }

    public void DisplayAllMembers()
    {
        Console.WriteLine("\n--- All Library Members ---");
        if (Members.Any())
        {
            foreach (var member in Members)
            {
                member.DisplayInfo();
            }
        }
        else
        {
            Console.WriteLine("No members in the system.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();

        library.AddBook(new Book(1, "1984", "George Orwell"));
        library.AddBook(new Book(2, "The Hobbit", "J.R.R. Tolkien"));
        library.AddBook(new Book(3, "Clean Code", "Robert C. Martin"));

        library.AddMember(new Member(1, "Ali"));
        library.AddMember(new Member(2, "Sara"));

        while (true)
        {
            Console.WriteLine("\n--- Library Menu ---");
            Console.WriteLine("1. Show all books");
            Console.WriteLine("2. Show all members");
            Console.WriteLine("3. Borrow book");
            Console.WriteLine("4. Return book");
            Console.WriteLine("5. Show member borrowed books");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");
            string choice = Console.ReadLine();

            int mid, bid;
            AbstractPerson absMember;
            Book book;

            switch (choice)
            {
                case "1":
                    library.DisplayAllBooks();
                    break;

                case "2":
                    library.DisplayAllMembers();
                    break;

                case "3":
                    Console.Write("Enter member ID: ");
                    if (!int.TryParse(Console.ReadLine(), out mid))
                    {
                        Console.WriteLine("Invalid ID format.");
                        break;
                    }

                    Console.Write("Enter book ID: ");
                    if (!int.TryParse(Console.ReadLine(), out bid))
                    {
                        Console.WriteLine("Invalid ID format.");
                        break;
                    }

                    absMember = library.FindMemberById(mid);
                    book = library.FindBookById(bid);

                    if (absMember is Member member && book != null)
                        member.BorrowBook(book);
                    else
                        Console.WriteLine("Invalid member or book ID.");
                    break;

                case "4":
                    Console.Write("Enter member ID: ");
                    if (!int.TryParse(Console.ReadLine(), out mid))
                    {
                        Console.WriteLine("Invalid ID format.");
                        break;
                    }

                    Console.Write("Enter book ID: ");
                    if (!int.TryParse(Console.ReadLine(), out bid))
                    {
                        Console.WriteLine("Invalid ID format.");
                        break;
                    }

                    absMember = library.FindMemberById(mid);
                    book = library.FindBookById(bid);

                    if (absMember is Member member2 && book != null)
                        member2.ReturnBook(book);
                    else
                        Console.WriteLine("Invalid member or book ID.");
                    break;

                case "5":
                    Console.Write("Enter member ID: ");
                    if (!int.TryParse(Console.ReadLine(), out mid))
                    {
                        Console.WriteLine("Invalid ID format.");
                        break;
                    }

                    absMember = library.FindMemberById(mid);
                    if (absMember is Member member3)
                        member3.DisplayBorrowedBooks();
                    else
                        Console.WriteLine("Invalid member ID.");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}