using ElevenNoteData;
using ElevenNoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNoteServices
{
    public class NoteService
    {
        private readonly Guid _userID;
        public NoteService(Guid userID)
        {
            _userID = userID;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userID,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userID)
                        .Select(
                            e =>
                                new NoteListItem
                                {
                                    NoteID = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc
                                }
                        );

                return query.ToArray();
            }
        }

        public NoteDetail GetNoteById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == id && e.OwnerId == _userID);
                return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userID);
                
                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int noteId)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == noteId && e.OwnerId == _userID);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
