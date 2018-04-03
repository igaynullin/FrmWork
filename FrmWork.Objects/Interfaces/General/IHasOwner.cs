namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasOwner<TOwner>
    {
        TOwner Owner { get; set; }
    }
}