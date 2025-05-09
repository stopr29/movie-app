namespace MovieApp.Infrastructure.Caching;

public interface IAppCache
{
    T? Get<T>(string key) where T : notnull;
    void Set<T>(string key, T value);
}