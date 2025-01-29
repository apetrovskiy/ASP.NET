# Sequence of actions accordingly to the lesson

1. Nugets
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.Design
   - Microsoft.EntityFrameworkCore.Tools
   - drivers
2. Context
   - services, add context (.UseSqlite, .UserNpgsql(), etc)
3. Connection string
   - link to a DB file (for Sqlite)
4. Context consists of two parts: constructor, collection sets (DbSet), OnModelCreating
5. SaveChanges on any change
6. The Repository pattern
7. Entity statuses (Added, Unchanged, Modfied, Deleted, Detached)
8. filtering DTOs
9. Lazy and not laxy methods
   - some methods require access to DB (ToListAsync, etc)
   - IQueryable is lazy
10. Migrations
    - migration is code
    - should be 1) created (check the Snapshot) 2) applied to DB (check the MigrationHistory)
    - dotnet-ef for migtations
    - (dotnet tool install --global dotnet-ef --version 8.0.0; dotnet tool uninstall --global dotnet-ef)
    - 4 parameter sets
    - class inherited from Migration with methods Up and Down
    - seeding (data)
    - 