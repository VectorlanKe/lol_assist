using System.Linq.Expressions;
using LOL.Assist.Core.DbModels;
using LOL.Assist.Core.Enums;

namespace LOL.Assist.Core.IServices;

/// <summary>
/// 数据持久化服务
/// </summary>
public interface IDbService
{
    /// <summary>
    /// 增改数据
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    bool InsertOrUpdate<T>(T request) where T : class, new();

    /// <summary>
    /// 根据key获取配置信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Config GetConfig(ConfigKeyEnum key);

    /// <summary>
    /// 获取全部信息
    /// </summary>
    /// <returns></returns>
    IList<T> GetAll<T>() where T : class, new();

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    bool Delete<T>(Expression<Func<T,bool>> where) where T : class, new();

    /// <summary>
    /// 获取全部配置信息
    /// </summary>
    /// <returns></returns>
    IList<T> GetAll<T>(Expression<Func<T, bool>> where) where T : class, new();

    /// <summary>
    /// 根据key获取配置信息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    bool UpdateConfigDelayTime(ConfigKeyEnum key, int delayTime);

    /// <summary>
    /// 获取全部配置信息
    /// </summary>
    /// <returns></returns>
    T? Get<T>(Expression<Func<T, bool>> where) where T : class, new();
}