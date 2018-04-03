namespace FrmWork.Objects.Interfaces.General
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TDate"></typeparam>
    public interface IHasAudit<TUser, TDate> : IHasCreate<TUser, TDate>, IHasModify<TUser, TDate>
    {
    }

    public interface IHasCreate<TUser, TDate>
    {
        TUser Creator { get; set; }
        TDate CreatedAt { get; set; }
    }

    public interface IHasModify<TUser, TDate>
    {
        TUser Modifier { get; set; }
        TDate ModifiedAt { get; set; }
    }
}