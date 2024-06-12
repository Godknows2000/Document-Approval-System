using DocumentApprovalSystem.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using wCyber.Helpers.Identity;

namespace DocumentApprovalSystem.Data
{
    public static class DataExtMethods
    {
        public static List<Note> GetNotes(this INotesContainer container)
        {
            if (container.NotesJson == null) return new List<Note>();
            return JsonSerializer.Deserialize<List<Note>>(container.NotesJson, JsonHelper.SerializerOptions);
        }

        public static void AddNotes(this INotesContainer container, Note note)
        {
            var notes = container.GetNotes();
            notes.Add(note);
            container.NotesJson = JsonSerializer.Serialize(notes, JsonHelper.SerializerOptions);
        }
        public static List<JAttachment> GetAttachments(this IAttachmentsContainer container)
        {
            if (container.AttachmentsJson == null) return new List<JAttachment>();
            return JsonSerializer.Deserialize<List<JAttachment>>(container.AttachmentsJson, JsonHelper.SerializerOptions);
        }
        public static void AddAttachment(this IAttachmentsContainer container, JAttachment attachment)
        {
            var attachments = container.GetAttachments();
            attachments.Add(attachment);
            container.AttachmentsJson = JsonSerializer.Serialize(attachments, JsonHelper.SerializerOptions);
        }



        public static byte[] ToBytes(this Stream stream)
        {
            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

public static EmailServerOptions GetOptions(this EmailConfig config)
   => new()
   {
       Host = config.Host,
       Password = config.Hash.GetPassword(config.Id.ToString()),
       Port = config.Port,
       SenderID = config.SenderId,
       SenderName = config.SenderDisplayName,
       Username = config.Username,
       UseSSL = config.EnableSsl
   };
public static string Letter(this int Year)
{
    var alphabet = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z" };
    var diff = Year - 2022;
    var myLetter = alphabet[diff];
    return myLetter;
}
    }

}
