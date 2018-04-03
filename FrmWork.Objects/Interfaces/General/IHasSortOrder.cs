namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasSortOrder<T>
    {
        T SortOrder { get; set; }
    }
}