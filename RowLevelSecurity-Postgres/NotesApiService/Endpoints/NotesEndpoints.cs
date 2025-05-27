using Microsoft.EntityFrameworkCore;
using NotesApiService.Data;
using NotesApiService.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace NotesApiService.Endpoints;

public static class NotesEndpoints
{
    private static readonly ActivitySource ActivitySource = new("NotesApiService");
    public static void MapNotesEndpoints(this IEndpointRouteBuilder route)
    {
        var app = route.MapGroup("/notes").WithTags("Notes");

        app.MapGet("/", async (ClaimsPrincipal claimsPrincipal, ApplicationDbContext db) =>
        {
            using var activity = ActivitySource.StartActivity("NotesApiService.GetNotes");
            var userId = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            activity?.AddTag("user.id", userId);
            activity?.AddTag("notes.action", "list");

            var notes = await db.Notes
                                //.Where(n=> n.UserId == userId)
                                .OrderByDescending(n=> n.UpdatedAt)
                                .ToListAsync();

            activity?.Stop();
            return Results.Ok(notes);
        });

        app.MapGet("/{id}", async (int id, ClaimsPrincipal claimsPrincipal, ApplicationDbContext db) =>
        {
            using var activity = ActivitySource.StartActivity("NotesApiService.GetNoteById");
            var userId = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
            activity?.AddTag("user.id", userId);
            activity?.AddTag("note.id", id);
            activity?.AddTag("notes.action", "get");

            var note = await db.Notes
                               .Where(n => n.UserId == userId)
                               .FirstOrDefaultAsync(n => n.Id == id);
            activity?.Stop();
            if (note is null)
                return Results.NotFound();

            return Results.Ok(note);
        });

        app.MapPost("/", async (Note note, ClaimsPrincipal claimsPrincipal, ApplicationDbContext db) =>
        {
            var userId = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            note.UserId = userId;
            note.CreatedAt = DateTime.UtcNow;
            note.UpdatedAt = DateTime.UtcNow;

            db.Notes.Add(note);
            
            await db.SaveChangesAsync();
            
            return Results.Created($"/notes/{note.Id}", note);
        });

        app.MapPut("/{id}",async(int id, Note note, 
                                        ClaimsPrincipal claimsPrincipal, 
                                        ApplicationDbContext db) =>
        {
            var userId = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var existingNote = await db.Notes
                                        .Where(n => n.UserId == userId)
                                        .FirstOrDefaultAsync(n => n.Id == id);
            if (existingNote is null)
                return Results.NotFound();

            existingNote.Title = note.Title;
            existingNote.Content = note.Content;
            existingNote.UpdatedAt = DateTime.UtcNow;
            
            await db.SaveChangesAsync();
            
            return Results.Ok(existingNote);
        });

        app.MapDelete("/{id}", async (int id, ClaimsPrincipal claimsPrincipal, ApplicationDbContext db) =>
        {
            var userId = int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);
           
            var note = await db.Notes
                               .Where(n => n.UserId == userId)
                               .FirstOrDefaultAsync(n => n.Id == id);
            if (note is null)
                return Results.NotFound();
            
            db.Notes.Remove(note);
            
            await db.SaveChangesAsync();
            
            return Results.NoContent();
        });
    }
}
