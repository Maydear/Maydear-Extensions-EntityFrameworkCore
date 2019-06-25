/*****************************************************************************************
 * Copyright (c) 2008-2019 kelvin(kelvin@onloch.com)
 * 梁乔峰版权所有2008-2019。
 * 本软件文档资料是梁乔峰的资产,任何人士阅读和使用本资料必
 * 须获得相应的书面授权,承担保密责任和接受相应的法律束.
 ****************************************************************************************
 * FileName: BaseRepository.cs
 * Author:梁乔峰(5768534@qq.com)
 * CreateDate:2019/05/08 22:41:46
 * Description:
 *     仓储基础类
 * <version>|<author>            |<time>                                    |<desc>
 * 1        |梁乔峰              |2019/05/08 22:41:46                       |创建仓储基础类
*****************************************************************************************/
using Maydear.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Maydear.Extensions.EntityFrameworkCore
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 日志组件
        /// </summary>
        protected readonly ILogger logger;

        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected virtual DbContext Context { get; private set; }

        /// <summary>
        /// 数据集对象
        /// </summary>
        protected abstract DbSet<T> DbSetEntities { get; }

        /// <summary>
        /// 默认页码
        /// </summary>
        protected const int DEFAULT_PAGE_INDEX = 1;

        /// <summary>
        /// 默认每页数量
        /// </summary>
        protected const int DEFAULT_PAGE_SIZE = 50;

        /// <summary>
        /// 构造仓储
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="logger">日志组件</param>
        protected BaseRepository(DbContext context, ILogger logger)
        {
            this.Context = context;
            this.logger = logger;
        }

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="entity">待添加的实体</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public virtual bool Add(T entity)
        {
            DbSetEntities.Add(entity);
            return Context.SaveChanges() > 0;
        }

        /// <summary>
        /// 增加实体集合
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public virtual bool AddRange(IList<T> entities)
        {
            DbSetEntities.AddRange(entities);
            return Context.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="func">实体加工参数</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public abstract bool Change(T entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="condition">待更新实体的搜索条件</param>
        /// <param name="actionEntity">搜索后所做的操作</param>
        /// <returns></returns>
        public virtual bool Change(Expression<Func<T, bool>> condition, Action<T> actionEntity)
        {
            if(condition==null)
            {
               throw new ArgumentNullException(nameof(condition));
            }
        
            T dbEntity = DbSetEntities.FirstOrDefault(condition);
            actionEntity(dbEntity);
            return Context.SaveChanges() > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="func">实体加工参数</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public abstract bool ChangeRange(IList<T> entities);

        /// <summary>
        /// 更新实体集合
        /// </summary>
        /// <param name="condition">待更新实体的搜索条件</param>
        /// <param name="actionEntity">对查询的实体进行操作</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public virtual bool ChangeRange(Expression<Func<T, bool>> condition, Action<IList<T>> actionEntities)
        {
            IList<T> dbEntities = DbSetEntities.Where(condition).ToList();
            actionEntities(dbEntities);
            return Context.SaveChanges() > 0;
        }
        /// <summary>
        /// 按照指定条件获取数量
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回满足当前条件的实体数量</returns>
        public virtual long Count(Expression<Func<T, bool>> condition)
        {
            if(condition != null)
            {
               return DbSetEntities.Count(condition);
            }
            else
            {
               return DbSetEntities.Count();
            }
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="condition">判断条件</param>
        /// <returns>如果存在则返回true，反之则为false</returns>
        public virtual bool Exists(Expression<Func<T, bool>> condition)
        {
            if(condition == null)
            {
               throw new ArgumentNullException(nameof(condition));
            }
            return DbSetEntities.Any(condition);
        }

        /// <summary>
        /// 获取指定条件的实体集合
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回实体集</returns>
        public virtual IEnumerable<T> GetEntities(Expression<Func<T, bool>> condition)
        {
            if(condition != null)
            {
               return DbSetEntities.Where(condition);
            }
            else
            {
               return DbSetEntities;
            }
            
        }

        /// <summary>
        /// 获取指定条件的实体集合
        /// </summary>
        /// <param name="queryAction">条件</param>
        /// <returns>返回实体集</returns>
        public virtual IEnumerable<T> GetEntities(Func<IQueryable<T>, IEnumerable<T>> queryAction)
        {
            if (queryAction == null)
            {
                throw new ArgumentNullException(nameof(queryAction));
            }

            return queryAction(DbSetEntities);
        }

        /// <summary>
        /// 根据升序规则获取指定条件的实体集合
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBySelector">升序排序键</param>
        /// <returns>返回实体集</returns>
        public virtual IEnumerable<T> GetEntities<TKey>(Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBySelector)
        {
            IQueryable<T> query = DbSetEntities;
            if(condition != null)
            {
               query= query.Where(condition);
            }
            
            if(orderBySelector != null)
            {
               query = query.OrderBy(orderBySelector);
            }
            return query;
        }

        /// <summary>
        /// 根据降序规则获取指定条件的实体集合
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBySelector">降序排序键</param>
        /// <returns>返回实体集</returns>
        public virtual IEnumerable<T> GetEntitiesOrderByDescending<TKey>(Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBySelector)
        {
            IQueryable<T> query = DbSetEntities;
            if(condition != null)
            {
               query= query.Where(condition);
            }
            
            if(orderBySelector != null)
            {
               query = query.OrderByDescending(orderBySelector);
            }
            return query;
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回单个实体，如果存在多个实体时返回第一个</returns>
        public virtual T GetEntity(Expression<Func<T, bool>> condition)
        {
            if(condition == null)
            {
               throw new ArgumentNullException(nameof(condition));
            }
            return DbSetEntities.FirstOrDefault(condition);
        }

        /// <summary>
        /// 获取实体并分页，
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="page">分页规则</param>
        /// <returns>返回带分页信息的实体集</returns>
        public virtual IPageCollection<T> GetPageEntities(Page page, Expression<Func<T, bool>> condition)
        {
            IQueryable<T> entities = DbSetEntities;
            if (condition != null)
            {
                entities = entities.Where(condition);
            }
            long count = entities.LongCount();

            if (page.Limit.HasValue)
            {
                if (!page.Offset.HasValue)
                {
                    page.PageIndex = DEFAULT_PAGE_INDEX;
                }
                entities = entities.Skip(page.Offset.Value).Take(page.Limit.Value);
            }
            return new PageCollection<T>(entities, count, page.PageIndex, page.PageSize ?? DEFAULT_PAGE_SIZE);
        }

        /// <summary>
        /// 根据升序规则获取实体并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="page">分页规则</param>
        /// <param name="orderBySelector">升序排序键</param>
        /// <returns>返回带分页信息的实体集</returns>
        public virtual IPageCollection<T> GetPageEntities<TKey>(Page page, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBySelector)
        {
            IQueryable<T> entities = DbSetEntities;
            if (condition != null)
            {
                entities = entities.Where(condition);
            }
            long count = entities.LongCount();

            if (orderBySelector != null)
            {
                entities = entities.OrderBy(orderBySelector);
            }

            if (page.Limit.HasValue)
            {
                if (!page.Offset.HasValue)
                {
                    page.PageIndex = DEFAULT_PAGE_INDEX;
                }
                entities = entities.Skip(page.Offset.Value).Take(page.Limit.Value);
            }
            return new PageCollection<T>(entities, count, page.PageIndex, page.PageSize ?? DEFAULT_PAGE_SIZE);
        }

        /// <summary>
        /// 根据降序规则获取实体并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="page">分页规则</param>
        /// <param name="orderBySelector">降序排序键</param>
        /// <returns>返回带分页信息的实体集</returns>
        public virtual IPageCollection<T> GetPageEntitiesOrderByDescending<TKey>(Page page, Expression<Func<T, bool>> condition, Expression<Func<T, TKey>> orderBySelector)
        {
            IQueryable<T> entities = DbSetEntities;
            if (condition != null)
            {
                entities = entities.Where(condition);
            }
            long count = entities.LongCount();

            if (orderBySelector != null)
            {
                entities = entities.OrderByDescending(orderBySelector);
            }

            if (page.Limit.HasValue)
            {
                if (!page.Offset.HasValue)
                {
                    page.PageIndex = DEFAULT_PAGE_INDEX;
                }
                entities = entities.Skip(page.Offset.Value).Take(page.Limit.Value);
            }
            return new PageCollection<T>(entities, count, page.PageIndex, page.PageSize ?? DEFAULT_PAGE_SIZE);
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        /// <param name="entity">移除的实体</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public abstract bool Remove(T entity);

        /// <summary>
        /// 按指定条件移除实体
        /// </summary>
        /// <param name="condition">待删除实体的条件</param>
        /// <returns>成功则返回True，失败则返回false</returns>
        public virtual bool Remove(Expression<Func<T, bool>> condition)
        {
            if(condition == null)
            {
               throw new ArgumentNullException(nameof(condition));
            }
        
            var dbEntities = DbSetEntities.Where(condition).ToList();
            if (dbEntities.Any())
            {
                DbSetEntities.RemoveRange(dbEntities);
                return Context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
