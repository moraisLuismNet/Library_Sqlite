namespace Library.Services
{
    public interface ICommonServiceBase<T, TI, TU>
    {
        List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Update(int id, TU tUpdateDTO);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
    }

}

