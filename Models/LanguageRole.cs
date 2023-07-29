namespace webapi.Models;

public partial class LanguageRole
{
    public int Id { get; set; }

    public string LanguageRole1 { get; set; } = null!;

    public virtual ICollection<MovieLanguage> MovieLanguages { get; set; } = new List<MovieLanguage>();
}
