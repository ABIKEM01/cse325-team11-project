
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ServeTogether.Data;
using ServeTogether.Models;

namespace ServeTogether.Services;

public record SignUpResult(bool Success, string Message);

public class SignUpService(ApplicationDbContext db)
{
    public async Task<SignUpResult> SignUpAsync(int slotId, string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return new(false, "You must be logged in to sign up.");
        }

        await db.Database.OpenConnectionAsync();

        var connection = (SqliteConnection)db.Database.GetDbConnection();

        // Acquire SQLite's write lock before checking available spots.
        await using var sqliteTransaction =
            connection.BeginTransaction(deferred: false);

        await using var transaction =
            await db.Database.UseTransactionAsync(sqliteTransaction);

        try
        {
            var slot = await db.Slots
                .FirstOrDefaultAsync(s => s.Id == slotId);

            if (slot is null)
            {
                return new(false, "This time slot could not be found.");
            }

            var alreadySignedUp = await db.SignUps
                .AnyAsync(s => s.SlotId == slotId && s.UserId == userId);

            if (alreadySignedUp)
            {
                return new(false, "You have already signed up for this time slot.");
            }

            var signUpCount = await db.SignUps
                .CountAsync(s => s.SlotId == slotId);

            if (signUpCount >= slot.Capacity)
            {
                return new(false, "Sorry, this time slot is already full.");
            }

            db.SignUps.Add(new SignUp
            {
                SlotId = slotId,
                UserId = userId,
                SignedUpAt = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
            await transaction!.CommitAsync();

            return new(true, "You have successfully signed up!");
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqliteException sqliteException &&
                  (sqliteException.SqliteExtendedErrorCode == 2067 ||
                   sqliteException.SqliteExtendedErrorCode == 1555))
        {
            return new(false, "You have already signed up for this time slot.");
        }
    }
}
