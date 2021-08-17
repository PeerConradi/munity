# Resolutions

Resolutions are documents that are worked with inside the committees. They can be connected to a conference a specific committee or be created without any of those.

Note that because of the nature of how the Database stores information (relational database). We worked woth mongoDB before but decieded against it later on because changing only one paragraph took way to much effort in always identifing the whole resolution and loading it.

## Basic information

The information of the resolution are stored inside the __ResaElement__ for example the Topic, Name, AgendaItem, Session SubmitterName, CommitteeName etc.

> The CommitteeName is not necessarily the same as the committee that currently has access to this document. The access to a document is handled inside the ResolutionAuth

You can simply change any of those values by changing the property:

```c#
var resolution = _context.Resolutions.FirstOrDefault(n => n.ResaElementId == "ID HERE");
resolution.Name = "New Name";
_context.SaveChanges();
```

## Preamble Paragraphs

### Adding a preamble Paragraph

This code shows how you can fast add a new Preamble Paragraph to a resolution.

```C#
var tmpResolution = _context.Resolutions
                .FirstOrDefault();
var newPreambleParagraph = new ResaPreambleParagraph()
{
    Comment = "",
    IsCorrected = false,
    IsLocked = false,
    OrderIndex = 0,
    ResaElement = tmpResolution,
    Text = "New Paragraph"
};
_context.PreambleParagraphs.Add(newPreambleParagraph);
_context.SaveChanges();
```

> IMPORTAND: When working with the preamble paragraphs or operative paragraphs without using the MUNityServices remember that you have to take care of the OrderIndex whenever you insert or remove a new paragraph.

## Operative Paragraphs

### Adding an operative paragraph

```c#
var paragraph = new ResaOperativeParagraph()
{
    Resolution = this.resolution
};
this._context.OperativeParagraphs.Add(paragraph);
this._context.SaveChanges();
```