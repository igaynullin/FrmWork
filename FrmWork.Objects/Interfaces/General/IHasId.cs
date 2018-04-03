namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasId<TId>
    {
        TId Id { get; set; }
    }
}