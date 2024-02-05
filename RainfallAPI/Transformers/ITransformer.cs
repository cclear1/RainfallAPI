namespace RainfallAPI.Transformers
{
    public interface ITransformer<TInput, TOutput>
    {
        TOutput Transform(TInput input);
    }
}
