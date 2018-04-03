namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasCode<TCode>
    {
        TCode Code { get; set; }
    }
}