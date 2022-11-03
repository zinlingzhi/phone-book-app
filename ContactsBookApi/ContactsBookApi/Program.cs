
using ContactsBookApi.models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container
// Link the database for our minimal REST API
builder.Services.AddDbContext<ContactListDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("ContactAPIContext")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/", () => "Hello World");

app.MapGet("/list-contact", async (ContactListDb db) => await db.ContactList.ToListAsync());
app.MapPost("/search-contact", async (ContactListDb db, string findText) => await (db.ContactList.Where(contact => contact.Name.Contains(findText)).ToListAsync()));

app.MapPost("/create-contact", async(ContactList contact, ContactListDb db) =>
{
    db.ContactList.Add(contact);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/delete-contact", async (int id, ContactListDb db) =>
{
    if (await db.ContactList.FindAsync(id) is ContactList contact)
    {
        db.ContactList.Remove(contact);
        await db.SaveChangesAsync();
        return Results.Ok(contact);
    }

    return Results.NotFound();
});


app.Run();

class ContactListDb : DbContext
{
    public ContactListDb(DbContextOptions<ContactListDb> options) : base(options)
    {

    }

    public DbSet<ContactList> ContactList => Set<ContactList>(); 
}