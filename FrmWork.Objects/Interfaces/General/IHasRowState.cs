using FrmWork.Objects.Models;

namespace FrmWork.Objects.Interfaces.General
{
    public interface IHasRowState<TRowState>
    {
        TRowState RowState { get; set; }
    }

    public interface IHasRowState
    {
        RowState RowState { get; set; }
    }
}