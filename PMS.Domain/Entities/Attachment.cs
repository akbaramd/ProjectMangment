using SharedKernel.DomainDrivenDesign.Domain;

namespace PMS.Domain.Entities;

public abstract class Attachment : Entity<Guid>
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; } // Path or URL where the file is stored

    protected Attachment() { }

    public Attachment(string fileName, string filePath)
    {
        FileName = fileName;
        FilePath = filePath;
    }
}