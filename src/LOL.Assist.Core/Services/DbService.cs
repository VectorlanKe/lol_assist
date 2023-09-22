using LOL.Assist.Core.Enums;
using LOL.Assist.Core.IServices;
using System.Linq.Expressions;
using LOL.Assist.Core.DbModels;

namespace LOL.Assist.Core.Services;

/// <summary>
/// 数据持久化服务
/// </summary>
public class DbService : IDbService
{
    private readonly IFreeSql _freeSql;
    public DbService(IFreeSql freeSql)
    {
        _freeSql = freeSql;
    }

    /// <summary>
    /// 增改数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public bool InsertOrUpdate<T>(T request) where T : class, new()
    {
        int result = _freeSql.InsertOrUpdate<T>().SetSource(request).ExecuteAffrows();
        return result > 0;
    }

    /// <summary>
    /// 获取全部配置信息
    /// </summary>
    /// <returns></returns>
    public T? Get<T>(Expression<Func<T, bool>> where) where T : class, new() => _freeSql.Select<T>().Where(where).ToOne();

    /// <summary>
    /// 获取全部配置信息
    /// </summary>
    /// <returns></returns>
    public IList<T> GetAll<T>() where T : class, new() => _freeSql.Select<T>().ToList();
    /// <summary>
    /// 获取全部配置信息
    /// </summary>
    /// <returns></returns>
    public IList<T> GetAll<T>(Expression<Func<T, bool>> where) where T : class, new() => _freeSql.Select<T>().Where(where).ToList();

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public bool Delete<T>(Expression<Func<T, bool>> where) where T : class, new()
    {
        int result = _freeSql.Delete<T>().Where(where).ExecuteAffrows();
        return result > 0;
    }

    /// <summary>
    /// 根据key获取配置信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Config GetConfig(ConfigKeyEnum key)
    {
        return _freeSql.Select<Config>().Where(p => p.Key == key).ToOne();
    }

    /// <summary>
    /// 根据key获取配置信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public bool UpdateConfigDelayTime(ConfigKeyEnum key, int delayTime)
    {
        int execute = _freeSql.Update<Config>()
            .Set(p => p.DelayTime, delayTime)
            .Where(p => p.Key == key)
            .ExecuteAffrows();
        return execute > 0;
    }
}