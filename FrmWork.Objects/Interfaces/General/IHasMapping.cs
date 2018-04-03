namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasMapping<in TObject, out TResult>
    {
        TResult Map(TObject obj);
    }
}