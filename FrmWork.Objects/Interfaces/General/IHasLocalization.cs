namespace FrmWork.Objects.Interfaces.General
{
    /// <summary>
    /// If using ids for localization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHasLocalizationId<T>
    {
        T LocalizationId { get; set; }
    }

    /// <summary>
    /// If using Named colums for localization
    /// </summary>
    public interface IHasLocalizationName
    {
        string NameRu { get; set; }
        string NameEn { get; set; }
    }
}