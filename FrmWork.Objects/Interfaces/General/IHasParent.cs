namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasParent<T>
    {
        T ParentId { get; set; }
    }
}